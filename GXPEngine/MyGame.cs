using System.Drawing.Text;
using System.Reflection.Emit;
using GXPEngine;
using TiledMapParser;
//using Physics;
public class MyGame : Game {

    static Level level;
    static CogWheel cogWheel;
    static PanoramaManager panoramaManager;

    public MyGame() : base(1920, 1080, false,false,-1,-1,true)
	{
        targetFps = 60; // Consistent, non variable framerate
        /*        AddChild(new Level(new Vec2(width / 2, height / 2), 300));
                cogWheel = new CogWheel(new Vec2(width/2, height/2), 10);
                AddChild(cogWheel);*/

        panoramaManager = new PanoramaManager(new Vec2(0, 0));
        AddChild(panoramaManager);
        // This loads the level at the start.
        ResetLevel();
    }

	void Update()
    {
    }

    void ResetLevel()
    {
        if (level != null)
        {
            level.Destroy();
            level = null;
        }

        level = new Level(new Vec2(width / 2, height / 2), "Level_1_prototype.tmx");
        AddChild(level);
    }


    static void Main()
	{
        new MyGame().Start();
	}
}