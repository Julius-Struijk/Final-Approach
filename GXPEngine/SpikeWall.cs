namespace GXPEngine
{
    public class SpikeWall : LineSegment
    {
        public float bounciness;

        public SpikeWall(float pStartX, float pStartY, float pEndX, float pEndY, float bounciness, uint pColor = 4294967295, uint pLineWidth = 1) : base(pStartX, pStartY, pEndX, pEndY, pColor, pLineWidth)
        {
            this.bounciness = bounciness;
        }

        public SpikeWall(Vec2 pStart, Vec2 pEnd, float bounciness, uint pColor = 4294967295, uint pLineWidth = 1) : base(pStart, pEnd, pColor, pLineWidth)
        {
            this.bounciness = bounciness;
        }
    }
}
