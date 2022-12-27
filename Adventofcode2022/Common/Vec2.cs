namespace Adventofcode2022.Common
{
    public readonly struct Vec2
    {
        public readonly int x;
        public readonly int y;

        public Vec2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public static Vec2 operator +(Vec2 a, Vec2 b) => new(a.x + b.x, a.y + b.y);

        public override int GetHashCode() => x + y;
    }
}
