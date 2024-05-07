using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

class Platform : Sprite
{
    public Platform(float pX, float pY, int pWidth, int pHeight) : base ("Assets/greenButton.png")
    {
        x = pX;
        y = pY;
        width = pWidth;
        height = pHeight;
    }
}
