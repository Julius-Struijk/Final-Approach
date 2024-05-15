using System;
using System.Drawing.Text;
using System.Reflection.Emit;
using GXPEngine;
using TiledMapParser;
//using Physics;
public class MyGame : Game {

    static Level level;
    static PanoramaManager panoramaManager;

    public MyGame() : base(1920, 1080, false,false,-1,-1,true)
	{
        targetFps = 60; // Consistent, non variable framerate

        panoramaManager = new PanoramaManager(new Vec2(0, 0));
        AddChild(panoramaManager);
        // This loads the level at the start.
        ResetLevel();
    }

	void Update()
    {
        // Checks whether the level has been won.
        if(level.winCheck())
        {
            //Console.WriteLine("Level won!");
        }

        // Checks whether the player has died.
        if (level.deathCheck())
        {
            //Console.WriteLine("Player died.");
        }
    }

    void ResetLevel()
    {
        if (level != null)
        {
            level.Destroy();
            level = null;
        }

        level = new Level(new Vec2(width / 2, height / 2), "level 1.tmx");
        AddChild(level);
    }


    static void Main()
	{
        new MyGame().Start();
	}
}