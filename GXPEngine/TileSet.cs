using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

class TileSet : AnimationSprite
{
    public string nextLevel {  get; private set; }
    public TileSet(string filename, int colls, int rows, TiledObject obj = null) : base(filename, colls, rows)
    {
        if (obj != null)
        {
            nextLevel = obj.GetStringProperty("nextLevel", "level 1.tmx");
            //Console.WriteLine(levelIndex);
        }
        SetOrigin(width / 2, height / 2);
    }

    public void FixOffset()
    {
        x -= game.width / 2;
        y -= game.height / 2;
    }
}
