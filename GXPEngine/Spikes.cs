using GXPEngine.Managers;
using TiledMapParser;
using GXPEngine;
using System;

class Spikes : AnimationSprite
{
    //Vec2 position;

    LineSegment Top;
    LineSegment Bottom;
    LineSegment Left;
    LineSegment Right;

    float radiusWidth;
    float radiusHeight;

    public Spikes(string filename, int colls, int rows, TiledObject obj = null) : base(filename, colls, rows)
    {
        //position = pPosition;
        SetOrigin(width / 2, height / 2);
        //width = pWidth;
        //height = pHeight;
        //x = pPosition.x + radiusWidth;
        //y = pPosition.y + radiusHeight;

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

        Top = new SpikeWall(new Vec2(x + radiusWidth, y - radiusHeight), new Vec2(x - radiusWidth, y - radiusHeight), 0.2f, 0xff00ff00, 3);
        parent.AddChild(Top);

        CogWheel topLeft = new CogWheel(0, new Vec2(x - radiusWidth, y - radiusHeight), 0, false);
        topLeft.SetSpawnType(typeof(Spikes));
        parent.AddChild(topLeft);

        CogWheel topRight = new CogWheel(0, new Vec2(x + radiusWidth, y - radiusHeight), 0, false);
        topRight.SetSpawnType(typeof(Spikes));
        parent.AddChild(topRight);

        Bottom = new SpikeWall(new Vec2(x - radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y + radiusHeight), 0.2f, 0xff00ff00, 3);
        parent.AddChild(Bottom);

        CogWheel bottomLeft = new CogWheel(0, new Vec2(x - radiusWidth, y + radiusHeight), 0, false);
        bottomLeft.SetSpawnType(typeof(Spikes));
        parent.AddChild(bottomLeft);

        CogWheel bottomRight = new CogWheel(0, new Vec2(x + radiusWidth, y + radiusHeight), 0, false);
        bottomRight.SetSpawnType(typeof(Spikes));
        parent.AddChild(bottomRight);

        Left = new SpikeWall(new Vec2(x - radiusWidth, y - radiusHeight), new Vec2(x - radiusWidth, y + radiusHeight), 0.2f, 0xff00ff00, 3);
        parent.AddChild(Left);

        Right = new SpikeWall(new Vec2(x + radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y - radiusHeight), 0.2f, 0xff00ff00, 3);
        parent.AddChild(Right);
    }

    void Update()
    {
        SetCycle(0, 6);
        Animate();
    }

}