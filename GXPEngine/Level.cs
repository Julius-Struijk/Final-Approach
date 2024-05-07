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

    int borderLenght = 585;
    int borderWidth = 35;
    public Level(float pX, float pY, int pBoundarySize)
    {
        boundarySize = pBoundarySize;
        x = pX;
        y = pY;

        AddChild(new Platform(-boundarySize, -boundarySize, borderLenght, borderWidth));
        AddChild(new Platform(-boundarySize, -boundarySize, borderWidth, borderLenght));
        AddChild(new Platform(boundarySize - 50, -boundarySize, borderWidth, borderLenght));
        AddChild(new Platform(-boundarySize, boundarySize - 50, borderLenght, borderWidth));
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
