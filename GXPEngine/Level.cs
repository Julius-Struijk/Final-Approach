using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

class Level : GameObject
{
    float boundarySize;
    float Size;
    //public Level(float pX, float pY, int pSize) : base("Assets/metalGrate.jpg")
    public Level(float pX, float pY, int pBoundarySize)
    {
        //SetOrigin(width / 2, height / 2);
        //width = pSize;
        //height = pSize;
        boundarySize = pBoundarySize;
        x = pX;
        y = pY;
        //AddChild(new Platform(-width * 4, -height * 4, 800, 200));
        //AddChild(new Platform(-width * 4, -height * 4, 200, 800));
        //AddChild(new Platform(width * 4 - 200, -height * 4, 200, 800));
        //AddChild(new Platform(-width * 4, height * 4, 800, 200));

        AddChild(new Platform(-boundarySize, -boundarySize, 200, 50));
        AddChild(new Platform(-boundarySize, -boundarySize, 50, 200));
        AddChild(new Platform(boundarySize - 50, -boundarySize, 50, 200));
        AddChild(new Platform(-boundarySize, boundarySize, 200, 50));
        AddChild(new Platform(0, 0, 25, 25));
    }

    void RotateLevel()
    {
        // Get the delta vector to mouse:
        float dx = Input.mouseX - x;
        float dy = Input.mouseY - y;
        Vec2 vx = new Vec2(dx, dy);

        // Get angle to mouse, convert from radians to degrees:
        float targetAngle = vx.GetAngleDegrees();

        rotation = targetAngle;

        if (Input.GetMouseButtonDown(0)) { Console.WriteLine(vx.Length()); }
    }

    void Update()
    {
        // Only moves level if the left mouse button is held.
        if (Input.GetMouseButton(0))
        {
            RotateLevel();
        }
    }
}
