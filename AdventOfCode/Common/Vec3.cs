namespace AdventOfCode.Common
{
    public readonly struct Vec3 : IEquatable<Vec3>
    {
        public readonly int x;
        public readonly int y;
        public readonly int z;

        public Vec3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public bool Equals(Vec3 other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        public override bool Equals(object? obj)
        {
            return obj is Vec3 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }
        
        public static double Distance(Vec3 a, Vec3 b)
        {
            checked
            {
                return Math.Sqrt(
                    (a.x - b.x) * (a.x - b.x) +
                    (a.y - b.y) * (a.y - b.y) +
                    (a.z - b.z) * (a.z - b.z)
                );
            }
        }
        
        public static decimal DistanceSq(Vec3 a, Vec3 b)
        {
            checked
            {
                decimal dx = a.x - b.x;
                decimal dy = a.y - b.y;
                decimal dz = a.z - b.z;
                
                return dx * dx +
                       dy * dy +
                       dz * dz;
            }
        }
    }
}