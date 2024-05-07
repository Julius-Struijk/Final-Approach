using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
//using Physics;

public class MyGame : Game {
    public MyGame() : base(1920, 1080, false,false,-1,-1,true)
	{
        targetFps = 30; // Consistent, non variable framerate
        AddChild(new Level(width / 2, height / 2, 100));
    }

	void Update()
	{

    }

    static void Main()
	{
        new MyGame().Start();
	}
}