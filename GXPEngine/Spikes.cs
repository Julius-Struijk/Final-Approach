namespace GXPEngine
{
    public class Spikes : LineSegment
    {
        public float bounciness = 0.5f;
        public float damage;

        public Spikes(float pStartX, float pStartY, float pEndX, float pEndY, float bounciness, float damage, uint pColor = 4294967295, uint pLineWidth = 1) : base(pStartX, pStartY, pEndX, pEndY, pColor, pLineWidth)
        {
            this.bounciness = bounciness;
            this.damage = damage;
        }

        public Spikes(Vec2 pStart, Vec2 pEnd, float bounciness, float damage, uint pColor = 4294967295, uint pLineWidth = 1) : base(pStart, pEnd, pColor, pLineWidth)
        {
            this.bounciness = bounciness;
            this.damage = damage;
        }
    }
}