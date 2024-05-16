namespace GXPEngine
{
    public class BouncyWall : LineSegment
    {
        public float bounciness = 1.4f;

        public BouncyWall(float pStartX, float pStartY, float pEndX, float pEndY, float pBounciness, uint pColor = 4294967295, uint pLineWidth = 1) : base(pStartX, pStartY, pEndX, pEndY, pColor, pLineWidth)
        {
            bounciness = pBounciness;
        }

        public BouncyWall(Vec2 pStart, Vec2 pEnd, float pBounciness, uint pColor = 4294967295, uint pLineWidth = 1) : base(pStart, pEnd, pColor, pLineWidth)
        {
            bounciness = pBounciness;
        }
    }
}
