
using System;
using System.Collections.Generic;
using System.Media;
using System.Security.AccessControl;
using GXPEngine;
using TiledMapParser;

class Level : GameObject
{
    public float targetAngle { get; private set; }
    Vec2 position = new Vec2();

    public readonly CogWheel[] _movers;
    public readonly LineSegment[] _lines;
    public Level(Vec2 pPosition, string mapName)
    {
        targetAngle = 0;
        position = pPosition;
        x = position.x;
        y = position.y;
        // This is to prevent the bug where the rotation starts out wrong, since it's based off the position of the object which is the rotated the wrong way.
        position = new Vec2(1101, 26);

        TiledLoader loader = new TiledLoader(mapName);
        loader.rootObject = this;
        //loader.LoadTileLayers(0);
        loader.autoInstance = true;
        loader.LoadObjectGroups(0);

        spawnPlatformLines();
        spawnSpikeObjects();

        //spawnSpikes();

        //After all lines have been added to the level they are found and assigned to the lines list
        _lines = FindObjectsOfType<LineSegment>();
        _movers = FindObjectsOfType<CogWheel>();
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

    void spawnSpikeObjects()
    {
        Spikes[] spikes = FindObjectsOfType<Spikes>();
        foreach (Spikes spike in spikes)
        {
            spike.AddObjects();
        }
    }

    void spawnPlatformLines()
    {
        //Platform platform = new Platform(pPosition, pWidth, pHeight);
        //AddChild(platform);
        Platform[] platforms = FindObjectsOfType<Platform>();
        foreach (Platform platform in platforms)
        {
            platform.AddLines();
        }
    }
}
