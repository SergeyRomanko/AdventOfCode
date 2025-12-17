namespace AdventOfCode.Common
{
    public readonly struct Vec2Long : IEquatable<Vec2Long>
    {
        public static IEnumerable<Vec2Long> Adjacent => new[]
        {
            new Vec2Long(+1, +1),
            new Vec2Long(-1, -1),
            new Vec2Long(-1, +1),
            new Vec2Long(+1, -1),
            
            new Vec2Long(0,  -1),
            new Vec2Long(0,  +1),
            new Vec2Long(-1,  0),
            new Vec2Long(+1,  0),
        };
        
        public static readonly Vec2Long Zero  = new Vec2Long(0, 0);
        public static readonly Vec2Long Up    = new Vec2Long(0, -1);
        public static readonly Vec2Long Down  = new Vec2Long(0, +1);
        public static readonly Vec2Long Left  = new Vec2Long(-1, 0);
        public static readonly Vec2Long Right = new Vec2Long(+1, 0);
        
        public readonly long x;
        public readonly long y;

        public Vec2Long(long x, long y)
        {
            this.x = x;
            this.y = y;
        }
        
        public static Vec2Long operator +(Vec2Long a, Vec2Long b) => new(a.x + b.x, a.y + b.y);

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public bool Equals(Vec2Long other)
        {
            return x == other.x && y == other.y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Vec2Long other && Equals(other);
        }

        public static bool operator ==(Vec2Long left, Vec2Long right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vec2Long left, Vec2Long right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"[{x}:{y}]";
        }
        
        public static decimal DistanceSq(Vec2Long a, Vec2Long b)
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
