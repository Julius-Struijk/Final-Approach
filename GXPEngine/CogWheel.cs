using System.Drawing;
using GXPEngine;
class CogWheel: GameObject
{
    private float health;
    private float maxhealth;
    private float drag = 0.05f;
    private float characterMass = 40f;

    int radius = 100;
    int canvas = 110;

    private Vec2 gravity = new Vec2(0, 9.81f);
    private Vec2 velocity;
    private bool isFalling = true;

    EasyDraw circle;
    public CogWheel(Vec2 position, float health) : base(true) 
    {
        x = position.x;
        y = position.y;

        this.health = health;
        this.maxhealth = health;

        
    }

    void Update()
    {
        spawnCogWheel();
        Movement();
    }

    void spawnCogWheel()
    {
        circle = new EasyDraw(canvas, canvas, true);
        circle.Ellipse(canvas/2, canvas/2, radius, radius);
        circle.SetXY(game.width/2, game.height/2);
        circle.SetOrigin(canvas / 2, canvas / 2);
        game.AddChild(circle);
    }

    void Movement()
    {


        if(isFalling)
        {
            float deltaTime = Time.deltaTime / 1000f;
            velocity += gravity * drag * characterMass * deltaTime;
        }
    }
}