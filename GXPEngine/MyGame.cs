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

    private float timer = 0;
    private float musicLoopNumber = 282;

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
        BackgroundMusic();

        if(level != null)
        {
            // Checks whether the level has been won.
            if (level.winCheck())
            {
                foreach(TileSet tileSet in level.tileSets)
                {
                    currentLevel = tileSet.nextLevel;
                    LoadLevel(currentLevel);
                }
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

    void BackgroundMusic()
    {
        float deltaTime = Time.deltaTime / 1000f;
        timer -= deltaTime;

        if(timer <= 0)
        {
            SoundManager.background_music.play(1f, 0);
            timer = musicLoopNumber;
        }
        Console.WriteLine("music cooldown: " + timer);
    }


    static void Main()
	{
        new MyGame().Start();
	}
}