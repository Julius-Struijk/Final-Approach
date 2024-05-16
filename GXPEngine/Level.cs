
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Media;
using System.Reflection.Emit;
using System.Security.AccessControl;
using GXPEngine;
using TiledMapParser;

class Level : GameObject
{
    private float timer = 0;
    private float musicLoopNumber = 316;
    public float targetAngle { get; private set; }
    public int rotationTracker { get; private set; }
    public string levelTileSet { get; private set; }
    Vec2 position = new Vec2();
    CogWheel cogWheel;
    public TileSet[] tileSets { get; private set; }

    public readonly CogWheel[] _movers;
    public readonly LineSegment[] _lines;
    public Level(Vec2 pPosition, string mapName)
    {
        levelTileSet = mapName;
        targetAngle = 0;
        rotationTracker = 0;
        position = pPosition;
        x = position.x;
        y = position.y;
        // This is to prevent the bug where the rotation starts out wrong, since it's based off the position of the object which is the rotated the wrong way.
        //position = new Vec2(1101, 26);
        position.SetAngleDegrees(targetAngle);

        TiledLoader loader = new TiledLoader(mapName);
        loader.rootObject = this;
        loader.autoInstance = true;

        loader.LoadObjectGroups(0);
        tileSets = FindObjectsOfType<TileSet>();
        foreach (TileSet tileSet in tileSets)
        {
            tileSet.FixOffset();
        }
        cogWheel = FindObjectOfType<CogWheel>();
        cogWheel.SetProperties();
        spawnPlatformObjects();
        spawnSpikeObjects();

        //After all lines have been added to the level they are found and assigned to the lines list
        _lines = FindObjectsOfType<LineSegment>();
        _movers = FindObjectsOfType<CogWheel>();
    }

    void RotateLevel()
    {
        //if (Input.GetKeyDown(Key.UP)) { targetAngle = 0; }

        // This only allows rotations once they're complete and not halfway through
        if (rotationTracker == targetAngle)
        {
            if (Input.GetKeyDown(Key.RIGHT)) { targetAngle = 90;
                SoundManager.rotate_right.play(0.5f, 0);
                rotationTracker = 0;
            }
            if (Input.GetKeyDown(Key.LEFT)) { targetAngle = -90;
                SoundManager.rotate_left.play(0.5f, 0);
                rotationTracker = 0;
            }
        }

        //Fixed 90 degree Rotation
        //if (rotation - targetAngle < -180)
        //{
        //    position.RotateDegrees(-1);
        //    rotation = position.GetAngleDegrees();
        //}

        //else if (targetAngle > rotation + 0.5f || rotation - targetAngle > 180)
        //{
        //    position.RotateDegrees(1);
        //    float prevRotation = rotation;
        //    rotation = position.GetAngleDegrees();
        //    if (prevRotation - rotation > 1 && Approx(targetAngle, 180, 0.5f)) { rotation = 180; }
        //}
        //else if (targetAngle < rotation - 0.5f)
        //{
        //    position.RotateDegrees(-1);
        //    rotation = position.GetAngleDegrees();
        //}

        // 90 degree left and right rotation
        if (targetAngle > rotationTracker)
        {
            position.RotateDegrees(1);
            rotation = position.GetAngleDegrees();
            rotationTracker++;
        }

        else if (targetAngle < rotationTracker)
        {
            position.RotateDegrees(-1);
            rotation = position.GetAngleDegrees();
            rotationTracker--;
        }
    }

    void Update()
    {
        RotateLevel();
        BackgroundMusic();
    }

    void spawnSpikeObjects()
    {
        Spikes[] spikes = FindObjectsOfType<Spikes>();
        foreach (Spikes spike in spikes)
        {
            spike.AddObjects();
        }
    }

    void spawnPlatformObjects()
    {
        Platform[] platforms = FindObjectsOfType<Platform>();
        foreach (Platform platform in platforms)
        {
            platform.AddObjects();
        }
    }

    public bool winCheck()
    {
        // Check that prevents an automatic win when the positions are wrong as they are being spawned in.
        if (cogWheel.x - cogWheel._oldPosition.x > 600) { return false; }
        else
        {
            foreach (TileSet tileSet in tileSets)
            {
                if (cogWheel.x > tileSet.x + tileSet.width || cogWheel.x < tileSet.x - tileSet.width || cogWheel.y > tileSet.y + tileSet.height || cogWheel.y < tileSet.y - tileSet.height)
                {
                    SoundManager.victory_sound.play(0.5f, 0);
                    return true;
                }
            }
        }
        return false;
    }

    public bool deathCheck()
    {
        if(cogWheel.health == 0)
        {
            SoundManager.Death_sound.play(0.5f, 0);
            return true;
        }
        return false;
    }

    static bool Approx(float a, float b, float epsilon = 0.000001f)
    {
        return Mathf.Abs(a - b) < epsilon;
    }

    void BackgroundMusic()
    {
        float deltaTime = Time.deltaTime / 1000f;
        timer -= deltaTime;

        if (timer <= 0)
        {
            SoundManager.background_music.play(1f, 0);
            timer = musicLoopNumber;
        }
    }
}
