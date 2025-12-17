namespace AdventOfCode.Common
{
    public readonly struct Vec2 : IEquatable<Vec2>
    {
        public static IEnumerable<Vec2> Adjacent => new[]
        {
            new Vec2(+1, +1),
            new Vec2(-1, -1),
            new Vec2(-1, +1),
            new Vec2(+1, -1),
            
            new Vec2(0,  -1),
            new Vec2(0,  +1),
            new Vec2(-1,  0),
            new Vec2(+1,  0),
        };
        
        public static readonly Vec2 Zero  = new Vec2(0, 0);
        public static readonly Vec2 Up    = new Vec2(0, -1);
        public static readonly Vec2 Down  = new Vec2(0, +1);
        public static readonly Vec2 Left  = new Vec2(-1, 0);
        public static readonly Vec2 Right = new Vec2(+1, 0);
        
        public readonly int x;
        public readonly int y;

        public Vec2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public static Vec2 operator +(Vec2 a, Vec2 b) => new(a.x + b.x, a.y + b.y);

        public static int ManhattanDistance(Vec2 a, Vec2 b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public bool Equals(Vec2 other)
        {
            return x == other.x && y == other.y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Vec2 other && Equals(other);
        }

        public static bool operator ==(Vec2 left, Vec2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vec2 left, Vec2 right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"[{x}:{y}]";
        }
        
        public static decimal DistanceSq(Vec2 a, Vec2 b)
        {
            checked
            {
                decimal dx = a.x - b.x;
                decimal dy = a.y - b.y;
                
                return dx * dx + dy * dy;
            }
        }
    }
}
