using System.Drawing.Text;
using GXPEngine;
//using Physics;
public class MyGame : Game {

    static Level level;
    static CogWheel cogWheel;
    static PanoramaManager panoramaManager;

    public MyGame() : base(1920, 1080, true,false,-1,-1,true)
	{
        targetFps = 60; // Consistent, non variable framerate
/*        AddChild(new Level(new Vec2(width / 2, height / 2), 300));
        cogWheel = new CogWheel(new Vec2(width/2, height/2), 10);
        AddChild(cogWheel);*/

        panoramaManager = new PanoramaManager(new Vec2(0, 0));
        level = new Level(new Vec2(width / 2, height / 2), 300);

        AddChild(panoramaManager);
        AddChild(level);


        level.spawnCharacter();
    }

	void Update()
	{

    }

    static void Main()
	{
        new MyGame().Start();
	}
}