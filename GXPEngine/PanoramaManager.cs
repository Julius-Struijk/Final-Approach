using System.Reflection;
using GXPEngine;
using GXPEngine.Managers;

class PanoramaManager : AnimationSprite
{
    int counter;
    int frame;
    public PanoramaManager(Vec2 position) : base("Assets/backgroundPlaceholder.png", 8, 1, -1, false, false)
    {

    }

    void Update()
    {
        Animation();
    }

    void Animation()
    {
        if (counter >= 6)
        {
            counter = 0;
            if (frame >= frameCount)
            {
                frame = 0;
            }
            SetFrame(frame);
            frame++;
        }
        counter++;
    }
}
