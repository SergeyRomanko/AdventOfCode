namespace AdventOfCode.Common
{
    public enum Direction
    {
        None,
        
        Up,
        Down,
        Left,
        Right
    }

    public static class DirectionExtensions
    {
        public static Direction ToOpposite(this Direction dir)
        {
            return dir switch
            {
                Direction.Up    => Direction.Down,
                Direction.Down  => Direction.Up,
                Direction.Left  => Direction.Right,
                Direction.Right => Direction.Left,
                _               => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };
        }
        
        public static Vec2 ToVec(this Direction dir)
        {
            return dir switch
            {
                Direction.Up    => Vec2.Up,
                Direction.Down  => Vec2.Down,
                Direction.Left  => Vec2.Left,
                Direction.Right => Vec2.Right,
                _               => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };
        }
    }
}