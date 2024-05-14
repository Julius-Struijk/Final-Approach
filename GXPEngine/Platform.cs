using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

class Platform : Sprite
{
    LineSegment Top;
    LineSegment Bottom;
    LineSegment Left;
    LineSegment Right;

    float radiusWidth;
    float radiusHeight;
    //public Platform(Vec2 pPosition, int pWidth, int pHeight) : base("Assets/greenButton.png")
    //{
    //    SetOrigin(width / 2, height / 2);
    //    width = pWidth;
    //    radiusWidth = width / 2;
    //    height = pHeight;
    //    radiusHeight = height / 2;
    //    x = pPosition.x + radiusWidth;
    //    y = pPosition.y + radiusHeight;
    //}

    public Platform(TiledObject obj=null) : base("Assets/greenButton.png")
    {
        SetOrigin(width / 2, height / 2);
        //x = pPosition.x + radiusWidth;
        //y = pPosition.y + radiusHeight;
    }

    // Adding Lines Seperately from the rest of the platform so parent is assigned and the lines can be added there.

    public void AddLines()
    {
        Console.WriteLine("Width: {0} Height: {1}", width, height);
        radiusWidth = width / 2;
        radiusHeight = height / 2;

        Top = new LineSegment(new Vec2(x + radiusWidth, y - radiusHeight), new Vec2(x - radiusWidth, y - radiusHeight), 0xff00ff00, 3);
        parent.AddChild(Top);

        Bottom = new LineSegment(new Vec2(x - radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y + radiusHeight), 0xff00ff00, 3);
        parent.AddChild(Bottom);

        Left = new LineSegment(new Vec2(x - radiusWidth, y - radiusHeight), new Vec2(x - radiusWidth, y + radiusHeight), 0xff00ff00, 3);
        parent.AddChild(Left);

        Right = new LineSegment(new Vec2(x + radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y - radiusHeight), 0xff00ff00, 3);
        parent.AddChild(Right);

/*        Right = new BouncyWall(new Vec2(x + radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y - radiusHeight), 1.1f, 0xff00ff00, 3);
        parent.AddChild(Right);*/
    }
}
