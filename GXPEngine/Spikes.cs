using GXPEngine.Managers;

namespace GXPEngine
{
    public class Spikes : Sprite
    {
        Vec2 position;

        LineSegment Top;
        LineSegment Bottom;
        LineSegment Left;
        LineSegment Right;

        float radiusWidth;
        float radiusHeight;

        public Spikes(Vec2 pPosition, int pWidth, int pHeight) : base("Assets/test.png")
        {
            this.position = position;
            SetOrigin(width / 2, height / 2);
            width = pWidth;
            radiusWidth = width / 2;
            height = pHeight;
            radiusHeight = height / 2;
            x = pPosition.x + radiusWidth;
            y = pPosition.y + radiusHeight;
        }

        public void addLines()
        {
            Top = new SpikeWall(new Vec2(x + radiusWidth, y - radiusHeight), new Vec2(x - radiusWidth, y - radiusHeight), 0.2f, 0xff00ff00, 3);
            parent.AddChild(Top);

            Bottom = new SpikeWall(new Vec2(x - radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y + radiusHeight), 0.2f, 0xff00ff00, 3);
            parent.AddChild(Bottom);

            Left = new SpikeWall(new Vec2(x - radiusWidth, y - radiusHeight), new Vec2(x - radiusWidth, y + radiusHeight), 0.2f, 0xff00ff00, 3);
            parent.AddChild(Left);

            Right = new SpikeWall(new Vec2(x + radiusWidth, y + radiusHeight), new Vec2(x + radiusWidth, y - radiusHeight), 0.2f, 0xff00ff00, 3);
            parent.AddChild(Right);
        }
    }
}