using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public enum EState
{
    Idle,
    TakeDamage
}
class CogWheel : AnimationSprite
{
    public int health { get; private set; }
    public Type spawnType { get; private set; }

    private int maxHealth;
    private float drag = 0.04f;
    private float characterMass = 40f;
    private float damageCooldown = 2f;
    float bounciness = 0.98f;
    //float oldRotation = 0;

    bool firstTime = true;
    bool moving;
    public bool takeDamage = false;

    int counter;
    int frame;

    private Vec2 gravity = new Vec2(0, 9.81f);
    private Vec2 velocity;
    //Vec2 extraVelocity;
    Vec2 position;
    public Vec2 _oldPosition { get; private set; }
    public readonly int radius;
    int tiledSpriteRadius;

    private Sprite heartEmpty;
    private Sprite heartFull;


    AnimationSprite currentAnimation;
    AnimationSprite idleAnimation;
    //AnimationSprite takeDamageAnimation = new AnimationSprite("animation", 1, 1);

    public EState eState;
    Level level;

    private List<Sprite> fullHearts = new List<Sprite>();
    private List<Sprite> emptyHearts = new List<Sprite>();

    // This constructor is used by line caps.
    public CogWheel(int pRadius, Vec2 pPosition, int health, bool pMoving = true) : base("Assets/placeholderPlayerNoSpace.png", 8, 1)
    {
        radius = pRadius;
        position = pPosition;
        moving = pMoving;
        alpha = 0;

        idleAnimation = new AnimationSprite("Assets/placeholderPlayerNoSpace.png", 8, 1);
        idleAnimation.width = radius * 2;
        idleAnimation.height = radius * 2;

        this.health = health;
        this.maxHealth = health;

        eState = EState.Idle;

        idleAnimation.visible = false;
        AddChild(idleAnimation);
        UpdateScreenPosition();

        for (int i = 0; i < maxHealth; i++)
        {
            heartEmpty = new Sprite("Assets/heartEmpty.png");
            game.AddChild(heartEmpty);
            emptyHearts.Add(heartEmpty);
            heartEmpty.scale = 0.2f;
            heartEmpty.SetXY(1600 + 100 * i, 25);
            heartEmpty.visible = false;
            heartFull = new Sprite("Assets/heartFull.png");
            game.AddChild(heartFull);
            fullHearts.Add(heartFull);
            heartFull.scale = 0.2f;
            heartFull.SetXY(1600 + 100 * i, 25);
            heartFull.visible = false;
        }
    }

    // This constructor is used for balls in Tiled.
    public CogWheel(string filename, int colls, int rows, TiledObject obj = null) : base(filename, colls, rows)
    {
        if (obj != null)
        {
            // Default values are used for line caps.
            moving = obj.GetBoolProperty("moving", false);
            health = obj.GetIntProperty("health", 0);
            // The width of the object uses the full width of the sprite instead of the width put in Tiled when initializing for some reason, so I set it seperately.
            radius = obj.GetIntProperty("radius", 0);
        }

        //Makes the main Animation invisible which preserves the system you had in place previously.
        alpha = 0;

        SetOrigin(radius * 2, radius * 2);

        maxHealth = health;

        eState = EState.Idle;

        idleAnimation = new AnimationSprite("Assets/placeholderPlayerNoSpace.png", 8, 1);

        // For some reason the size and offset of the animation does need the width of the actual sprite instead of the regular width. But this only happens in Tiled, not through GXP.
        tiledSpriteRadius = width;
        idleAnimation.width = tiledSpriteRadius;
        idleAnimation.height = tiledSpriteRadius;

        idleAnimation.visible = false;
        AddChild(idleAnimation);

        UpdateScreenPosition();

        for (int i = 0; i < maxHealth; i++)
        {
            heartEmpty = new Sprite("Assets/heartEmpty.png");
            game.AddChild(heartEmpty);
            emptyHearts.Add(heartEmpty);
            heartEmpty.scale = 0.2f;
            heartEmpty.SetXY(25, 105 * i + 15);
            heartEmpty.visible = false;
            heartFull = new Sprite("Assets/heartFull.png");
            game.AddChild(heartFull);
            fullHearts.Add(heartFull);
            heartFull.scale = 0.2f;
            heartFull.SetXY(25, 105 * i + 15);
            heartFull.visible = false;
        }
    }

    void Update()
    {
        float deltaTime = Time.deltaTime / 1000f;
        if (damageCooldown >= 0)
        {
            damageCooldown -= deltaTime;
            //Console.WriteLine("cooldown: " + damageCooldown);
        }
        Movement();
        Animation();
        UpdateHearts();
    }

    void UpdateScreenPosition()
    {
        x = position.x;
        y = position.y;
        //Console.WriteLine("Updated position to: {0} {1}", x, y);

        idleAnimation.x = -tiledSpriteRadius / 2;
        idleAnimation.y = -tiledSpriteRadius / 2;
    }

    void Movement()
    {
        // Linecaps will be stationary which is why this exists.
        if (moving)
        {
            level = (Level)parent;

            if (firstTime)
            {
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
        // Check other movers:			
        foreach (CogWheel mover in level._movers)
        {
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


        foreach (LineSegment line in level._lines)
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
        if (col.other is CogWheel)
        {
            position += velocity * col.timeOfImpact;
            Vec2 unitNormal = col.normal.Normalized();
            velocity.Reflect(unitNormal, bounciness);
            CogWheel lineCap = (CogWheel)col.other;
            if (lineCap.spawnType == typeof(Spikes))
            {
                takeDamage = true;
            }
            if (lineCap.spawnType == typeof(Spring))
            {
                position += velocity * col.timeOfImpact;
                unitNormal = col.normal.Normal();
                velocity.Reflect(unitNormal, bounciness * 2);
            }
        }
        else if (col.other is BouncyWall wall)
        {
            position += velocity * col.timeOfImpact;
            Vec2 unitNormal = col.normal.Normal();
            velocity.Reflect(unitNormal, wall.bounciness);
        }
        else if (col.other is SpikeWall spikeWall)
        {
            position += velocity * col.timeOfImpact;
            Vec2 unitNormal = col.normal.Normal();
            velocity.Reflect(unitNormal, spikeWall.bounciness);
            takeDamage = true;
        }
        else if (col.other is LineSegment)
        {
            position += velocity * col.timeOfImpact;
            Vec2 unitNormal = col.normal.Normal();
            velocity.Reflect(unitNormal, bounciness);
        }
    }

    void ChangeGravity()
    {
        //Before Rotation Gravity Change 90 Fixed Degrees
        //if(Approx(level.targetAngle, 0, 0.5f)) { gravity = new Vec2(0, 9.81f); }
        //if(Approx(level.targetAngle, 90, 0.5f)) { gravity = new Vec2(9.81f, 0); }
        //if(Approx(level.targetAngle, 180, 0.5f)) { gravity = new Vec2(0, -9.81f); }
        //if(Approx(level.targetAngle, -90, 0.5f)) { gravity = new Vec2(-9.81f, 0); }

        //Before Rotation Gravity Change 90 Degrees Left or Right
        // This make the gravity change only happen once near the start
        if (level.rotationTracker == 1 || level.rotationTracker == -1)
        {
            if (level.targetAngle == 90)
            {
                // Going to 0
                if (parent.rotation > -90 && parent.rotation < 0) { gravity = new Vec2(0, 9.81f); }
                // Going to 90
                if (parent.rotation > 0 && parent.rotation < 90) { gravity = new Vec2(9.81f, 0); }
                // Going to 180
                if (parent.rotation > 90 && parent.rotation < 180) { gravity = new Vec2(0, -9.81f); }
                // Going to -90
                if (parent.rotation > -180 && parent.rotation < -90) { gravity = new Vec2(-9.81f, 0); }
            }

            if (level.targetAngle == -90)
            {
                // Going to 0
                if (parent.rotation > 0 && parent.rotation < 90) { gravity = new Vec2(0, 9.81f); }
                // Going to 90
                if (parent.rotation > 90 && parent.rotation < 180) { gravity = new Vec2(9.81f, 0); }
                // Going to -180
                if (parent.rotation > -180 && parent.rotation < -90) { gravity = new Vec2(0, -9.81f); }
                // Going to -90
                if (parent.rotation > -90 && parent.rotation < 0) { gravity = new Vec2(-9.81f, 0); }
            }
        }
    }

    public void SetProperties()
    {
        // Make the starting position of the ball match that of what is shown in Tiled.
        position = new Vec2(x - game.width / 2, y - game.height / 2);
    }

    private void UpdateHearts()
    {
        int remainingHealth = health - 1;
        for (int i = 0; i < maxHealth; i++)
        {
            fullHearts[i].visible = i <= remainingHealth;
        }

        for (int i = emptyHearts.Count - 1; i >= 0; i--)
        {
            emptyHearts[i].visible = i > remainingHealth;
        }

        if (takeDamage)
        {
            if (damageCooldown <= 0)
            {
                health--;
                damageCooldown = 2f;
            }
            takeDamage = false;
        }
    }
    // Used to set which object spawned line caps.
    public void SetSpawnType(Type type)
    {
        spawnType = type;
    }

    static bool Approx(float a, float b, float epsilon = 0.000001f)
    {
        return Mathf.Abs(a - b) < epsilon;
    }

}