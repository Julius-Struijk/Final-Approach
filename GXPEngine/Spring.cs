using GXPEngine;
using System;
using System.Reflection.Emit;
using TiledMapParser;
public class Spring : AnimationSprite
{
    BouncyWall Top;
    BouncyWall Bottom;
    BouncyWall Right;
    BouncyWall Left;
    Sprite idle;

    float radiusWidth;
    float radiusHeight;
    int tiledSpriteRadius;
    bool activateAnimation = false;
    public Spring(string filename, int colls, int rows, TiledObject obj = null) : base(filename, colls, rows)
    {
        SetOrigin(width / 2, height / 2);
        //alpha = 0;

        tiledSpriteRadius = width;
        idle = new Sprite("Assets/springIdle.png");
        //idle.width = tiledSpriteRadius;
        idle.width = 656;
        //idle.height = tiledSpriteRadius;
        idle.height = 620;
        idle.rotation = rotation;
        //AddChild(idle);
    }

    public void AddObjects()
    {

        radiusWidth = width / 2;
        radiusHeight = height / 2;
        idle.x = -656 / 2;
        idle.y = -620 / 2;

        //Fix the rotation issue by placing the level in the correct location but offsetting the position of the rest of the objects.
        x -= game.width / 2;
        y -= game.height / 2;

        CogWheel topLeft = new CogWheel(0, new Vec2(x - radiusWidth, y - radiusHeight), 0, false);
        topLeft.SetSpawnType(typeof(Spring));
        parent.AddChild(topLeft);

        CogWheel topRight = new CogWheel(0, new Vec2(x + radiusWidth, y - radiusHeight), 0, false);
        topRight.SetSpawnType(typeof(Spring));
        parent.AddChild(topRight);

        CogWheel bottomLeft = new CogWheel(0, new Vec2(x - radiusWidth, y + radiusHeight), 0, false);
        bottomLeft.SetSpawnType(typeof(Spring));
        parent.AddChild(bottomLeft);

        CogWheel bottomRight = new CogWheel(0, new Vec2(x + radiusWidth, y + radiusHeight), 0, false);
        bottomRight.SetSpawnType(typeof(Spring));
        parent.AddChild(bottomRight);

        Top = new BouncyWall(new Vec2(x + radiusWidth, y - radiusHeight), new Vec2(x - radiusWidth, y - radiusHeight), 1.2f, 0xff00ff00, 3);
        parent.AddChild(Top);

        Bottom = new BouncyWall(new Vec2(x - radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y + radiusHeight), 1.2f, 0xff00ff00, 3);
        parent.AddChild(Bottom);

        //LineSegment Bottom = new LineSegment(new Vec2(x - radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y + radiusHeight), 0xff00ff00, 3);
        //parent.AddChild(Bottom);

        //LineSegment Left = new LineSegment(new Vec2(x - radiusWidth, y - radiusHeight), new Vec2(x - radiusWidth, y + radiusHeight), 0xff00ff00, 3);
        //parent.AddChild(Left);

        //LineSegment Right = new LineSegment(new Vec2(x + radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y - radiusHeight), 0xff00ff00, 3);
        //parent.AddChild(Right);

        Left = new BouncyWall(new Vec2(x - radiusWidth, y - radiusHeight), new Vec2(x - radiusWidth, y + radiusHeight), 1.2f, 0xff00ff00, 3);
        parent.AddChild(Left);

        Right = new BouncyWall(new Vec2(x + radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y - radiusHeight), 1.2f, 0xff00ff00, 3);
        parent.AddChild(Right);
    }

    void Update()
    {
        SetCycle(0, 12);
        Animate(0.5f);

        //if(activateAnimation)
        //{
        //    SetCycle(0, 12);
        //    Animate();
        //    if (currentFrame == frameCount - 1)
        //    {
        //        Console.WriteLine("Finishing Frame: {0}", currentFrame);
        //        alpha = 0;
        //        activateAnimation = false;
        //    }
        //}
    }

    public void PlayAnimation()
    {
        activateAnimation = true;
        alpha = 1;
    }
}

