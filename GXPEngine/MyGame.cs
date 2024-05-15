using System;
using System.Drawing.Text;
using System.Reflection.Emit;
using GXPEngine;
using TiledMapParser;
//using Physics;
public class MyGame : Game {

    static Level level;
    static PanoramaManager panoramaManager;
    string currentLevel;
    string nextLevel;

    public MyGame() : base(1920, 1080, false,false,-1,-1,true)
	{
        targetFps = 60; // Consistent, non variable framerate

        panoramaManager = new PanoramaManager(new Vec2(0, 0));
        AddChild(panoramaManager);
        // This loads the level at the start.
        currentLevel = "level 1.tmx";
        LoadLevel(currentLevel);
        OnAfterStep += ResetLevel;
    }

	void Update()
    {
        if(level != null)
        {
            // Checks whether the level has been won.
            if (level.winCheck())
            {
                currentLevel = level.tileSet.nextLevel;
                LoadLevel(currentLevel);
            }

            // Checks whether the player has died.
            if (level.deathCheck())
            {
                currentLevel = level.levelTileSet;
                LoadLevel(currentLevel);
            }
        }
    }

    public void LoadLevel(string levelToLoad)
    {
        nextLevel = levelToLoad;
    }

    void ResetLevel()
    {
        if (nextLevel != null)
        {
            if(level != null)
            {
                level.Destroy();
                level = null;
            }
            level = new Level(new Vec2(width / 2, height / 2), nextLevel);
            AddChild(level);
        }
        nextLevel = null;
    }


    static void Main()
	{
        new MyGame().Start();
	}
}