using GXPEngine;
using TiledMapParser;
public class Spring : AnimationSprite
{
    BouncyWall Top;
    BouncyWall Bottom;
    BouncyWall left;
    BouncyWall right;

    float radiusWidth;
    float radiusHeight;
    public Spring(string filename, int colls, int rows, TiledObject obj = null) : base(filename, colls, rows)
    {
        SetOrigin(width / 2, height / 2);
    }

    public void AddObjects()
    {
        //Rotation screws over the width and height of the spikes so this is the hacky workaround.
        if (rotation == 90 || rotation == -90)
        {
            radiusWidth = width / 4;
            radiusHeight = height;
        }
        else
        {
            radiusWidth = width / 2;
            radiusHeight = height / 2;
        }

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

        Top = new BouncyWall(new Vec2(x + radiusWidth, y - radiusHeight), new Vec2(x - radiusWidth, y - radiusHeight), 0.2f, 0xff00ff00, 3);
        parent.AddChild(Top);

        Bottom = new BouncyWall(new Vec2(x - radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y + radiusHeight), 0.2f, 0xff00ff00, 3);
        parent.AddChild(Bottom);

        left = new BouncyWall(new Vec2(x - radiusWidth, y - radiusHeight), new Vec2(x - radiusWidth, y + radiusHeight), 0.2f, 0xff00ff00, 3);
        parent.AddChild(left);

        right = new BouncyWall(new Vec2(x + radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y - radiusHeight), 0.2f, 0xff00ff00, 3);
        parent.AddChild(right);
    }

    void Update()
    {
        SetCycle(0, 6);
        Animate();
    }
}

