using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
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

    int radius = 50;
    int canvas = 60;
    int counter;
    int frame;

    private Vec2 gravity = new Vec2(0, 9.81f);
    private Vec2 velocity;
    private bool isFalling = true;

/*    Sprite healthBar = new Sprite("sprite");
    Sprite healthBarFrame = new Sprite("sprite");*/

    AnimationSprite currentAnimation;
    AnimationSprite idleAnimation = new AnimationSprite("Assets/placeholderPlayer.png", 8, 1);
    //AnimationSprite takeDamageAnimation = new AnimationSprite("animation", 1, 1);

    public EState eState;

    public CogWheel(Vec2 position, float health) : base(true) 
    {
        x = position.x;
        y = position.y;

        this.health = health;
        this.maxHealth = health;

        eState = EState.Idle;

        idleAnimation.visible = false;
        AddChild(idleAnimation);

        scale = 0.1f;
    }

    void Update()
    {
        Movement();
        Animation();
    }

    void Movement()
    {
        //Here we need to figure out the physics behind the movement of the cog wheel
        //Collision collision = MoveUntilCollision(0, velocity.y);
        //isFalling = collision == null;


        if (isFalling)
        {
            float deltaTime = Time.deltaTime / 1000f;
            velocity += gravity * drag * characterMass * deltaTime;
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