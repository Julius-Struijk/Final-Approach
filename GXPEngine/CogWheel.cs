using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using GXPEngine;
using GXPEngine.Core;

public enum EState
{
    Idle,
    TakeDamage
}
class CogWheel: GameObject
{
    private float health;
    private float maxHealth;
    private float drag = 0.05f;
    private float characterMass = 40f;
    float bounciness = 0.98f;
    float oldRotation = 0;

    bool firstTime = true;
    bool moving;

    int counter;
    int frame;

    private Vec2 gravity = new Vec2(0, 9.81f);
    private Vec2 velocity;
    Vec2 extraVelocity;
    Vec2 position;
    Vec2 _oldPosition;
    public readonly int radius;

    /*    Sprite healthBar = new Sprite("sprite");
        Sprite healthBarFrame = new Sprite("sprite");*/

    AnimationSprite currentAnimation;
    AnimationSprite idleAnimation;
    //AnimationSprite takeDamageAnimation = new AnimationSprite("animation", 1, 1);

    public EState eState;
    Level level;

    public CogWheel(int pRadius, Vec2 pPosition, float health, bool pMoving = true) : base(true) 
    {
        radius = pRadius;
        position = pPosition;
        moving = pMoving;

        idleAnimation = new AnimationSprite("Assets/placeholderPlayerNoSpace.png", 8, 1);
        idleAnimation.width = radius * 2;
        idleAnimation.height = radius * 2;

        this.health = health;
        this.maxHealth = health;

        eState = EState.Idle;

        idleAnimation.visible = false;
        AddChild(idleAnimation);

        UpdateScreenPosition();
    }

    void Update()
    {
        Movement();
        Animation();
    }

    void UpdateScreenPosition()
    {
        x = position.x;
        y = position.y;

        idleAnimation.x = -radius;
        idleAnimation.y = -radius;
    }

    void Movement()
    {
        // Linecaps will be stationary which is why this exists.
        if (moving)
        {

            if (firstTime) {
                float deltaTime = Time.deltaTime / 1000f;
                velocity += gravity * drag * characterMass * deltaTime;
                //if(extraVelocity.x == 0 && extraVelocity.y == 0) { extraVelocity = velocity; }
            }
            else { firstTime = true; }
            _oldPosition = position;

            rotation = -parent.rotation;
            // Doesn't quite work right when I assign it to extra velocity Should be small changes but it isn't. 
            // Here the velocity takes the rotation effect which works the best but then it negates the gravity and all that.
            //if (oldRotation != parent.rotation)
            //{
            //    Console.WriteLine("Pre: {0}", velocity);
            //    velocity.RotateDegrees(-parent.rotation + oldRotation);
            //    Console.WriteLine("Post: {0}", velocity);
            //    oldRotation = parent.rotation;
            //}
            ChangeGravity();

            position += velocity;

            CollisionInfo firstCollision = FindEarliestCollision();
            if (firstCollision != null)
            {
                ResolveCollision(firstCollision);
                if (firstCollision.timeOfImpact == 0 && firstTime)
                {
                    firstTime = false;
                }
            }
            UpdateScreenPosition();
        }
    }

    void Animation()
    {
        AnimationSprite previousAnimation = currentAnimation;
        switch (eState)
        {
            case EState.Idle:
                currentAnimation = idleAnimation;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (currentAnimation != previousAnimation)
        {
            if (previousAnimation != null)
            {
                previousAnimation.visible = false;
            }

            currentAnimation.visible = true;
        }

        if (counter >= 6)
        {
            counter = 0;
            if (frame >= currentAnimation.frameCount)
            {
                frame = 0;
            }
            
            currentAnimation.SetFrame(frame);
            frame++;
        }
        counter++;
    }

    CollisionInfo FindEarliestCollision()
    {
        //   This allows the ball to use the parent's(aka the level's) public methods.
       // Level level = (Level)parent;
        // Check other movers:			
        for (int i = 0; i < level.GetNumberOfMovers(); i++)
        {
            CogWheel mover = level.GetMover(i);
            if (mover != this)
            {
                Vec2 relativePosition = position - mover.position;

                Vec2 a = new Vec2(velocity.Dot(velocity));
                Vec2 b = new Vec2(2 * (relativePosition.Dot(velocity)));
                Vec2 c = new Vec2(relativePosition.Dot(relativePosition) - (radius + mover.radius) * (radius + mover.radius));
                float TOI = CalculateTOIBall(a, b, c);
                if (TOI != 0)
                {
                    return new CollisionInfo(relativePosition, mover, TOI);
                }
            }
        }

        foreach(LineSegment line in level._lines)
        {
            //b.1
            Vec2 differenceVectorMovement = position - _oldPosition;

            float TOI = CalculateTOILine(line, differenceVectorMovement);
            if (TOI != -2)
            {
                return new CollisionInfo((line.end - line.start), line, TOI);
            }
        }
        return null;
    }

    float CalculateTOIBall(Vec2 a, Vec2 b, Vec2 c)
    {
        if (a.x != 0)
        {
            float D = b.Dot(b) - 4 * a.Dot(c);
            if (D >= 0)
            {
                float TOI = (b.Dot(new Vec2(-1, -1)) - Mathf.Sqrt(D)) / a.Dot(new Vec2(2, 2));
                if (0 <= TOI && TOI < 1)
                {
                    return TOI;
                }
            }
        }
        return 0;
    }

    float CalculateTOILine(LineSegment line, Vec2 pDifferenceVectorMovement)
    {
        //b.2
        float movementDistance = pDifferenceVectorMovement.Dot((line.end - line.start).Normal());
        movementDistance *= -1;

        //a
        Vec2 differenceVector = new Vec2(position.x - line.start.x, position.y - line.start.y);
        float ballDistance = differenceVector.Dot((line.end - line.start).Normal()) - radius;
        if (movementDistance > 0)
        {
            // Magic impossible number, so the value is assigned for the if check later.
            float TOI = 2;
            if (ballDistance >= 0)
            {
                //t
                TOI = ballDistance / movementDistance;
            }
            else if (ballDistance >= -radius)
            {
                //t
                TOI = 0;
            }
            else { return -2; }

            if (TOI <= 1)
            {
                Vec2 POI = position + TOI * velocity;
                //d
                Vec2 differenceVectorPOI = new Vec2(POI.x - line.start.x, POI.y - line.start.y);
                float lineDistance = differenceVectorPOI.Dot((line.end - line.start).Normalized());
                if (lineDistance >= 0 && lineDistance <= (line.end - line.start).Length())
                {
                    return TOI;
                }
            }
        }
        // Magic number but can't return null
        return -2;
    }

    void ResolveCollision(CollisionInfo col)
    {
        if(col.other is CogWheel)
        {
            position += velocity * col.timeOfImpact;
            Vec2 unitNormal = col.normal.Normalized();
            velocity.Reflect(unitNormal, bounciness);
        } else if(col.other is BouncyWall wall)
        {
            position += velocity * col.timeOfImpact;
            Vec2 unitNormal = col.normal.Normal();
            velocity.Reflect(unitNormal, wall.bounciness);
        } else if(col.other is LineSegment)
        {
            position += velocity * col.timeOfImpact;
            Vec2 unitNormal = col.normal.Normal();
            velocity.Reflect(unitNormal, bounciness);
        }
    }

    static bool Approx(float a, float b, float epsilon = 0.000001f)
    {
        return Mathf.Abs(a - b) < epsilon;
    }

    void ChangeGravity()
    {
        //Before Rotation Gravity Change
        if (level.targetAngle == 0) { gravity = new Vec2(0, 9.81f); }
        //if (Approx(level.targetAngle, 45, 0.5f)) { gravity = new Vec2(4.905f, 4.905f); }
        if (level.targetAngle == 90) { gravity = new Vec2(9.81f, 0); }
        //if (Approx(level.targetAngle, 135, 0.5f)) { gravity = new Vec2(4.905f, -4.905f); }
        if (level.targetAngle == 180) { gravity = new Vec2(0, -9.81f); }
        //if (Approx(level.targetAngle, -45, 0.5f)) { gravity = new Vec2(-4.905f, 4.905f); }
        if (level.targetAngle == -90) { gravity = new Vec2(-9.81f, 0); }
        //if (Approx(level.targetAngle, -135, 0.5f)) { gravity = new Vec2(-4.905f, -4.905f); }


        // After Rotation Gravity change
        //if (Approx(parent.rotation, 0, 0.5f)) { gravity = new Vec2(0, 9.81f); }
        //if (Approx(parent.rotation, 45, 0.5f)) { gravity = new Vec2(4.905f, 4.905f); }
        //if (Approx(parent.rotation, 90, 0.5f)) { gravity = new Vec2(9.81f, 0); }
        //if (Approx(parent.rotation, 135, 0.5f)) { gravity = new Vec2(4.905f, -4.905f); }
        //if (Approx(parent.rotation, 180, 0.5f)) { gravity = new Vec2(0, -9.81f); }
        //if (Approx(parent.rotation, -45, 0.5f)) { gravity = new Vec2(-4.905f, 4.905f); }
        //if (Approx(parent.rotation, -90, 0.5f)) { gravity = new Vec2(-9.81f, 0); }
        //if (Approx(parent.rotation, -135, 0.5f)) { gravity = new Vec2(-4.905f, -4.905f); }
        //Console.WriteLine("Gravity changed to: {0}", gravity);
        //Console.WriteLine("Rotation: {0}", parent.rotation);
    }

    public void SetLevel(Level pLevel)
    {
        //This allows the ball to use the parents(aka the level's) public methods.
        level = pLevel;
    }

    /*    public void renderHealthBar(int offSetX, int offSetY)
        {
            if (!this.game.HasChild(healthBarFrame)) { this.game.AddChild(healthBarFrame); }
            if (!this.game.HasChild(healthBar)) { this.game.AddChild(healthBar); }

            healthBarFrame.scale = 0.20f;
            healthBarFrame.SetXY(this.x + offSetX, this.y - offSetY);

            float healthFraction = health / maxHealth;
            healthBar.scaleX = Mathf.Max(0f, healthFraction * 0.2f);
            healthBar.scaleY = 0.20f;

            healthBar.SetXY(this.x + offSetX + 4, this.y - offSetY + 3);

        }*/
}