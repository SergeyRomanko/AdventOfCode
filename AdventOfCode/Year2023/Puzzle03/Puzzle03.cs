using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle03 : Puzzle
    {
        private char[,] _data;
        
        private List<Vec2> _offsets = new List<Vec2>
        {
            new Vec2(-1, -1), new Vec2(+0, -1), new Vec2(+1, -1), 
            new Vec2(-1, +0), new Vec2(+0, +0), new Vec2(+1, +0), 
            new Vec2(-1, +1), new Vec2(+0, +1), new Vec2(+1, +1)
        };
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var inputList = input.ToList();

            var sizeX = inputList[0].Length;
            var sizeY = inputList.Count;
            
            _data = new char[sizeX, sizeY];

            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                    _data[y, x] = inputList[x][y];
                }
            }

            var part1 = GetNumberGroups(_data)
                       .Where(x => x.Any(y => CheckCoords(y, _data)))
                       .Select(x => CoordsToNumber(x, _data))
                       .Sum();
            
            return new[]
            {
                part1.ToString(), PartTwo(GetNumberGroups(_data), _data).ToString()
            };
        }

        private int PartTwo(IEnumerable<List<Vec2>> groups, char[,] data)
        {
            var dic = new Dictionary<Vec2, HashSet<List<Vec2>>>();

            foreach (var group in groups)
            {
                foreach (var coord in group)
                {
                    foreach (var target in _offsets.Select(x => x + coord))
                    {
                        if (GetDataSafe(target, data) != '*')
                        {
                            continue;
                        }

                        if (!dic.ContainsKey(target))
                        {
                            dic[target] = new HashSet<List<Vec2>>();
                        }

                        dic[target].Add(group);
                    }
                }
            }

            var result = dic.Values
                            .Where(x => x.Count == 2)
                            .Select(x => 
                            {
                                return x
                                       .Select(y => CoordsToNumber(y, data))
                                       .ToList();
                            })
                            .Select(x => x[0] * x[1])
                            .Sum();

            return result;
        }

        private bool CheckCoords(Vec2 pos, char[,] data)
        {
            return _offsets
                   .Select(x => GetDataSafe(x + pos, data))
                   .Any(x => !char.IsNumber(x) && x != '.');
        }

        private char GetDataSafe(Vec2 pos, char[,] data)
        {
            var sizeX = data.GetLength(0);
            var sizeY = data.GetLength(1);

            if (pos.x < 0)      return '.';
            if (pos.y < 0)      return '.';
            if (pos.x >= sizeX) return '.';
            if (pos.y >= sizeY) return '.';

            return data[pos.x, pos.y];
        }

        private IEnumerable<List<Vec2>> GetNumberGroups(char[,] data)
        {
            var prev  = new Vec2(-1, -1);
            var group = 0;

            return GetNumberCoords(data)
                   .Select(x =>
                   {
                       if (prev != new Vec2(x.x - 1, x.y))
                       {
                           group++;
                       }

                       prev = x;
                       
                       return (x, group);
                   })
                   .GroupBy(x => x.group)
                   .Select(x => x.Select(data => data.x).ToList())
                   .ToList();
        }

        private IEnumerable<Vec2> GetNumberCoords(char[,] data)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                for (int x = 0; x < data.GetLength(0); x++)
                {
                    if (char.IsNumber(data[x, y]))
                    {
                        yield return new Vec2(x, y);
                    }
                }
            }
        }
        
        private int CoordsToNumber(List<Vec2> coords, char[,] data)
        {
            return int.Parse(string.Join("", coords.Select(z => data[z.x, z.y])));
        }
    }
}