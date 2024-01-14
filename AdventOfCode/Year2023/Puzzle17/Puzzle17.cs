using System.Collections.Immutable;
using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle17 : Puzzle
    {
        private List<Direction> AllDirs = new()
        {
            Direction.Down, Direction.Up, Direction.Left, Direction.Right
        };
        

        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var inputList = input.ToList();
            var data      = ReadData(inputList);
            
            return new[]
            {
                Part1(data).ToString()
            };
        }

        private int _result = int.MaxValue;

        private int Part1(Dictionary<Vec2, int> data)
        {
            var end = data.Keys.MaxBy(x => x.x + x.y);
            
            Step(Vec2.Zero, 0, 0, Direction.Right, end, data, ImmutableHashSet<int>.Empty);
            
            Step(Vec2.Zero, 0, 0, Direction.Down, end, data, ImmutableHashSet<int>.Empty);
            
            return _result;
        }

        private void Step(Vec2 pos, int steps, int heatSum, Direction dir, Vec2 end, Dictionary<Vec2, int> data, ImmutableHashSet<int> hashSet)
        {
            var hash = HashCode.Combine(pos/*, steps, dir*/);
            if (hashSet.Contains(hash))
            {
                return;     //Тут мы уже были с теми же настройками, уходим
            }
            var nextSet = hashSet.Add(hash);
            
            if (!data.TryGetValue(pos, out var heat))
            {
                return;     //Эти координаты вне поля
            }

            heatSum += heat;
            steps++;

            if (heatSum + Vec2.ManhattanDistance(end, pos) > _result)
            {
                return;     //Текущая сумма уже хуже чем наш лучший результат
            }

            if (pos == end)
            {
                _result = Math.Min(_result, heatSum);
                
                Console.WriteLine($"{_result}");
                
                return;
            }
            
            foreach (var nextDir in GetDirs(pos, dir, steps, end, data))
            {
                var nextSteps = nextDir != dir ? 0 : steps;
                
                Step(pos + nextDir.ToVec(), nextSteps, heatSum, nextDir, end, data, nextSet);
            }
        }

        private IEnumerable<Direction> GetDirs(Vec2 pos, Direction dir, int steps, Vec2 end, Dictionary<Vec2, int> data)
        {
            var dirs = new List<(Direction, int)>();
            
            foreach (var nextDir in AllDirs)
            {
                if (nextDir == dir.ToOpposite())
                {
                    continue; //Обрано не идем
                }

                if (nextDir == dir && steps == 3)
                {
                    continue; //Не делаем больше 3х шагов в одном направлении
                }

                var nextPos = pos + nextDir.ToVec();
                if (data.TryGetValue(nextPos, out var heat))
                {
                    dirs.Add((nextDir, Vec2.ManhattanDistance(end, nextPos)));
                }
            }
            
            return dirs.OrderBy(x => x.Item2).Select(x => x.Item1);
        }

        private Dictionary<Vec2, int> ReadData(IReadOnlyList<string> input)
        {
            var inputList = input.ToList();
            var sizeX     = inputList[0].Length;
            var sizeY     = inputList.Count;

            var result = new Dictionary<Vec2, int>();
            
            for (var y = 0; y < sizeY; y++)
            {
                for (var x = 0; x < sizeX; x++)
                {
                    result[new Vec2(x, y)] = int.Parse(inputList[y][x].ToString());
                }
            }
            
            return result;
        }
    }
}