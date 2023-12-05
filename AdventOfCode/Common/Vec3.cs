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
    }
}