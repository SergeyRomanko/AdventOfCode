using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle11 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var inputList = input.ToList();
            
            var grid = BuildGrid(inputList);
            
            return new[]
            {
                DoThings(grid, 1).ToString(),
                DoThings(grid, 1000000 - 1).ToString(),
            };
        }

        private long DoThings(char[,] grid, int expansion)
        {
            var sizeX  = grid.GetLength(0);
            var sizeY  = grid.GetLength(1);
            var points = GetPoints(grid).ToList();

            var emptyX = Enumerable.Range(0, sizeX)
                .Where(x => points.All(point => point.x != x))
                .ToList();
            
            var emptyY = Enumerable.Range(0, sizeY)
                .Where(x => points.All(point => point.y != x))
                .ToList();

            var result = 0L;
            var queue  = new Queue<Vec2>(points);

            while (queue.TryDequeue(out var from))
            {
                foreach (var to in queue)
                {
                    result += Vec2.ManhattanDistance(from, to);
                    result += emptyX.Count(x => IsBetween(x, from.x, to.x)) * expansion;
                    result += emptyY.Count(x => IsBetween(x, from.y, to.y)) * expansion;
                }
            }
            
            return result;
        }

        private bool IsBetween(int value, int a, int b)
        {
            var min = Math.Min(a, b);
            var max = Math.Max(a, b);

            return min <= value && value <= max;
        }

        private IEnumerable<Vec2> GetPoints(char[,] grid)
        {
            var sizeX = grid.GetLength(0);
            var sizeY = grid.GetLength(1);
            
            for (var y = 0; y < sizeY; y++)
            {
                for (var x = 0; x < sizeX; x++)
                {
                    if (grid[x, y] != '#')
                    {
                        continue;
                    }

                    yield return new Vec2(x, y);
                }
            }
        }

        private char[,] BuildGrid(List<string> inputList)
        {
            var sizeX = inputList[0].Length;
            var sizeY = inputList.Count;

            var result = new char[sizeX, sizeY];
            
            for (var y = 0; y < sizeY; y++)
            {
                for (var x = 0; x < sizeX; x++)
                {
                    result[x, y] = inputList[y][x];
                }
            }

            return result;
        }
    }
}