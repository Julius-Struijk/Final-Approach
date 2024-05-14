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

        Top = new SpikeWall(new Vec2(x + radiusWidth, y - radiusHeight), new Vec2(x - radiusWidth, y - radiusHeight), 0.2f, 0xff00ff00, 3);
        parent.AddChild(Top);
        
        parent.AddChild(new CogWheel(0, new Vec2(x - radiusWidth, y - radiusHeight), 0, false));
        parent.AddChild(new CogWheel(0, new Vec2(x + radiusWidth, y - radiusHeight), 0, false));


        Bottom = new SpikeWall(new Vec2(x - radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y + radiusHeight), 0.2f, 0xff00ff00, 3);
        parent.AddChild(Bottom);

        parent.AddChild(new CogWheel(0, new Vec2(x - radiusWidth, y + radiusHeight), 0, false));
        parent.AddChild(new CogWheel(0, new Vec2(x + radiusWidth, y + radiusHeight), 0, false));

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