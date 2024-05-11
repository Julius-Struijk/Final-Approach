
using System;
using System.Collections.Generic;
using System.Media;
using GXPEngine;

class Level : GameObject
{
    float boundarySize;
    float targetAngle = 0;
    Vec2 position = new Vec2();

    int borderLenght = 585;
    int borderWidth = 35;

    private CogWheel cogwheel;

    List<CogWheel> _movers;
    public readonly LineSegment[] _lines;
    public Level(Vec2 pPosition, int pBoundarySize) 
    {
        boundarySize = pBoundarySize;
        position = pPosition;
        x = position.x;
        y = position.y;
        // This is to prevent the bug where the rotation starts out wrong, since it's based off the position of the object which is the rotated the wrong way.
        position = new Vec2(1101, 26);

        _movers = new List<CogWheel>();

        spawnPlatform(new Vec2(-boundarySize, -boundarySize), borderLenght, borderWidth);
        spawnPlatform(new Vec2(-boundarySize, -boundarySize), borderWidth, borderLenght);
        spawnPlatform(new Vec2(boundarySize - 50, -boundarySize), borderWidth, borderLenght);
        spawnPlatform(new Vec2(-boundarySize, boundarySize - 50), borderLenght, borderWidth);
        spawnCharacter();

        //After all lines have been added to the level they are found and assigned to the lines list
        _lines = FindObjectsOfType<LineSegment>();
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

    public void spawnCharacter()
    {
        cogwheel = new CogWheel(60, new Vec2(0, 0), 10);
        AddChild(cogwheel);
        _movers.Add(cogwheel);
    }

    void spawnPlatform(Vec2 pPosition, int pWidth, int pHeight)
    {
        Platform platform = new Platform(pPosition, pWidth, pHeight);
        AddChild(platform);
        platform.AddLines();
    }

    public int GetNumberOfMovers()
    {
        return _movers.Count;
    }

    public CogWheel GetMover(int index)
    {
        if (index >= 0 && index < _movers.Count)
        {
            return _movers[index];
        }
        return null;
    }
}
