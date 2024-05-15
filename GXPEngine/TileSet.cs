using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

class TileSet : Sprite
{
    public TileSet(TiledObject obj = null) : base("Assets/Tileset Level 1.png")
    {

    }

    public void FixOffset()
    {
        x -= game.width / 2;
        y -= game.height / 2;
    }
}
