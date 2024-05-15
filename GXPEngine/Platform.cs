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
        alpha = 0;
        
    }

    // Adding Lines Seperately from the rest of the platform so parent is assigned and the lines can be added there.

    public void AddObjects()
    {
        radiusWidth = width / 2;
        radiusHeight = height / 2;

        //Fix the rotation issue by placing the level in the correct location but offsetting the position of the rest of the objects.
        x -= game.width / 2;
        y -= game.height / 2;


        Top = new LineSegment(new Vec2(x + radiusWidth, y - radiusHeight), new Vec2(x - radiusWidth, y - radiusHeight), 0xff00ff00, 3);
        parent.AddChild(Top);

        CogWheel topLeft = new CogWheel(0, new Vec2(x - radiusWidth, y - radiusHeight), 0, false);
        topLeft.SetSpawnType(typeof(Platform));
        parent.AddChild(topLeft);

        CogWheel topRight = new CogWheel(0, new Vec2(x + radiusWidth, y - radiusHeight), 0, false);
        topRight.SetSpawnType(typeof(Platform));
        parent.AddChild(topRight);

        Bottom = new LineSegment(new Vec2(x - radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y + radiusHeight), 0xff00ff00, 3);
        parent.AddChild(Bottom);

        CogWheel bottomLeft = new CogWheel(0, new Vec2(x - radiusWidth, y + radiusHeight), 0, false);
        bottomLeft.SetSpawnType(typeof(Platform));
        parent.AddChild(bottomLeft);

        CogWheel bottomRight = new CogWheel(0, new Vec2(x + radiusWidth, y + radiusHeight), 0, false);
        bottomRight.SetSpawnType(typeof(Platform));
        parent.AddChild(bottomRight);

        Left = new LineSegment(new Vec2(x - radiusWidth, y - radiusHeight), new Vec2(x - radiusWidth, y + radiusHeight), 0xff00ff00, 3);
        parent.AddChild(Left);

        Right = new LineSegment(new Vec2(x + radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y - radiusHeight), 0xff00ff00, 3);
        parent.AddChild(Right);

/*        Right = new BouncyWall(new Vec2(x + radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y - radiusHeight), 1.1f, 0xff00ff00, 3);
        parent.AddChild(Right);*/
    }
}
