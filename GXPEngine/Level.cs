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
    public Level(float pX, float pY, int pBoundarySize)
    {
        boundarySize = pBoundarySize;
        x = pX;
        y = pY;

        AddChild(new Platform(-boundarySize, -boundarySize, 200, 50));
        AddChild(new Platform(-boundarySize, -boundarySize, 50, 200));
        AddChild(new Platform(boundarySize - 50, -boundarySize, 50, 200));
        AddChild(new Platform(-boundarySize, boundarySize - 50, 200, 50));
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
