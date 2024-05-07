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
    float targetAngle = 0;
    Vec2 position = new Vec2();

    int borderLenght = 585;
    int borderWidth = 35;
    public Level(Vec2 pPosition, int pBoundarySize) : base(true)
    {
        boundarySize = pBoundarySize;
        position = pPosition;
        x = position.x;
        y = position.y;

        AddChild(new Platform(-boundarySize, -boundarySize, borderLenght, borderWidth));
        AddChild(new Platform(-boundarySize, -boundarySize, borderWidth, borderLenght));
        AddChild(new Platform(boundarySize - 50, -boundarySize, borderWidth, borderLenght));
        AddChild(new Platform(-boundarySize, boundarySize - 50, borderLenght, borderWidth));
    }

    void RotateLevel()
    {
        //// Free Rotation
        //// Get the delta vector to mouse:
        //float dx = Input.mouseX - x;
        //float dy = Input.mouseY - y;
        //Vec2 vx = new Vec2(dx, dy);

        //// Get angle to mouse, convert from radians to degrees:
        //float targetAngle = vx.GetAngleDegrees();

        //rotation = targetAngle;

        //Fixed 90 degree Rotation
        if (Input.GetKeyDown(Key.UP)) { targetAngle = 0; }
        if (Input.GetKeyDown(Key.RIGHT)) { targetAngle = 90; }
        if (Input.GetKeyDown(Key.LEFT)) { targetAngle = -90; }
        if (Input.GetKeyDown(Key.DOWN)) { targetAngle = 180; }

        if (rotation - targetAngle < -180)
        {
            position.RotateDegrees(-1);
            rotation = position.GetAngleDegrees();
        }

        else if (targetAngle > rotation + 0.5f || rotation - targetAngle > 180)
        {
            position.RotateDegrees(1);
            float prevRotation = rotation;
            rotation = position.GetAngleDegrees();
            if (prevRotation - rotation > 1 && targetAngle == 180) { rotation = 180; }
        }
        else if (targetAngle < rotation - 0.5f)
        {
            position.RotateDegrees(-1);
            rotation = position.GetAngleDegrees();
        }
    }

    void Update()
    {
        RotateLevel();
        //// Only moves level if the left mouse button is held.
        //if (Input.GetMouseButton(0))
        //{
        //    RotateLevel();
        //}
    }
}
