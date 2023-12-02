namespace AdventOfCode.Year2022
{
    public static class Puzzle15_2
    {
        private struct Rect
        {
            public int MinX;
            public int MinY;
            public int MaxX;
            public int MaxY;
        }
        
        public static ulong GetResult(List<(Puzzle15.Vec, Puzzle15.Vec)> data, int size)
        {
            var candidates = new Queue<Rect>(200000);
            candidates.Enqueue(new Rect
            {
                MinX = 0,
                MinY = 0,
                MaxX = size,
                MaxY = size,
            });

            Rect rect = default;
            while (true)
            {
                rect = candidates.Dequeue();
                
                var isValid = Test(data, rect);
                if (!isValid)
                {
                    continue;
                }

                var isResult = rect.MaxX - rect.MinX == 0 && 
                               rect.MaxY - rect.MinY == 0;
                    
                if (isResult)
                {
                    break;
                }

                var sizeX = (float) rect.MaxX - rect.MinX + 1;
                var sizeY = (float) rect.MaxY - rect.MinY + 1;

                var dx1 = (int)Math.Ceiling(sizeX / 2.0f) - 1;
                var dx2 = (int)Math.Floor(sizeX / 2.0f) - 1;
                var dy1 = (int)Math.Ceiling(sizeY / 2.0f) - 1;
                var dy2 = (int)Math.Floor(sizeY / 2.0f) - 1;

                candidates.Enqueue(new Rect
                {
                    MinX = rect.MinX,
                    MaxX = rect.MinX + dx1,
                    MinY = rect.MinY,
                    MaxY = rect.MinY + dy1
                });
                    
                candidates.Enqueue(new Rect
                {
                    MinX = rect.MinX + dx1 + 1,
                    MaxX = rect.MinX + dx1 + 1 + dx2,
                    MinY = rect.MinY,
                    MaxY = rect.MinY + dy1
                });
                    
                candidates.Enqueue(new Rect
                {
                    MinX = rect.MinX,
                    MaxX = rect.MinX + dx1,
                    MinY = rect.MinY + dy1 + 1,
                    MaxY = rect.MinY + dy1 + 1 + dy2
                });
                    
                candidates.Enqueue(new Rect
                {
                    MinX = rect.MinX + dx1 + 1,
                    MaxX = rect.MinX + dx1 + 1 + dx2,
                    MinY = rect.MinY + dy1 + 1,
                    MaxY = rect.MinY + dy1 + 1 + dy2
                });
            }
            
            return (ulong)rect.MinX * 4000000UL + (ulong)rect.MinY;
        }

        private static bool Test(List<(Puzzle15.Vec, Puzzle15.Vec)> data, Rect rect)
        {
            foreach (var (s,b) in data)
            {
                var bDist = Math.Abs(b.X - s.X) + Math.Abs(b.Y - s.Y);
                var distA = Math.Abs(rect.MinX - s.X) + Math.Abs(rect.MinY - s.Y);
                var distB = Math.Abs(rect.MinX - s.X) + Math.Abs(rect.MaxY - s.Y);
                var distC = Math.Abs(rect.MaxX - s.X) + Math.Abs(rect.MinY - s.Y);
                var distD = Math.Abs(rect.MaxX - s.X) + Math.Abs(rect.MaxY - s.Y);

                if (distA <= bDist && distB <= bDist && distC <= bDist && distD <= bDist)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
