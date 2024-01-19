using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle17 : Puzzle
    {
        private readonly List<Direction> _allDirs = new()
        {
            Direction.Down, Direction.Up, Direction.Left, Direction.Right
        };
        
        private record Crucible(Vec2 Pos, Direction Dir, int Straight);
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            return new[]
            {
                DoThings(input, 0, 03).ToString(),
                DoThings(input, 4, 10).ToString(),
            };
        }
        
        private int DoThings(IReadOnlyList<string> input, int straightMin, int straightMax)
        {
            var data    = ReadData(input.ToList());
            var max     = data.Keys.MaxBy(x => x.x + x.y);
            var visited = new HashSet<Crucible>();
            var queue   = new PriorityQueue<Crucible, int>();
            
            queue.Enqueue(new Crucible(Vec2.Zero, Direction.Right, 0), 0);
            queue.Enqueue(new Crucible(Vec2.Zero, Direction.Down, 0), 0);

            while (queue.TryDequeue(out var crucible, out var heatlossCurrent))
            {
                if (crucible.Pos == max && crucible.Straight >= straightMin)
                {
                    return heatlossCurrent;
                }
                
                foreach (var next in GetNextSteps(crucible, straightMin, straightMax))
                {
                    if (!visited.Add(next))
                    {
                        continue;   //Уже проверили
                    }
                    
                    if (!data.TryGetValue(next.Pos, out var heatloss))
                    {
                        continue;   //За границами поля
                    }
                    
                    queue.Enqueue(next, heatlossCurrent + heatloss);
                }
            }
            
            return -1;
        }

        private IEnumerable<Crucible> GetNextSteps(Crucible crucible, int straightMin, int straightMax)
        {
            foreach (var dir in _allDirs)
            {
                if (dir == crucible.Dir.ToOpposite())
                {
                    continue;   //Назад не идем
                }

                if (dir == crucible.Dir)
                {
                    if (crucible.Straight == straightMax)
                    {
                        continue;   //Больше максимума шагов не идем
                    }
                    
                    yield return crucible with
                    { 
                        Pos      = crucible.Pos      + dir.ToVec(),
                        Straight = crucible.Straight + 1
                    };
                }
                else
                {
                    if (crucible.Straight < straightMin)
                    {
                        continue;   //Можем идти только прямо
                    }
                    
                    yield return new Crucible(crucible.Pos + dir.ToVec(), dir, 1);
                }
            }
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