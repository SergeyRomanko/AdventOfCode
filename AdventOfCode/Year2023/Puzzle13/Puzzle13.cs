using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle13 : Puzzle
    {
        private class Info
        {
            public int Line;
            public int Diff;
        }

        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var data = ReadData(input).ToList();
            
            return new[]
            {
                data.Select(Part1).Sum().ToString(),
                data.Select(Part2).Sum().ToString()
            };
        }
        
        private int Part2(char[,] data)
        {
            var sizeX = data.GetLength(0);
            var sizeY = data.GetLength(1);
            
            var hashX = Enumerable.Repeat("", sizeX).ToList();
            var hashY = Enumerable.Repeat("", sizeY).ToList();
            
            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                    hashX[x] += data[x, y];
                    hashY[y] += data[x, y];
                }
            }
            
            foreach (var info in GetMirror(hashX).Where(x => x.Diff == 1))
            {
                return info.Line;
            }
            
            foreach (var info in GetMirror(hashY).Where(x => x.Diff == 1))
            {
                return info.Line * 100;
            }

            throw new Exception();
        }

        private int Part1(char[,] data)
        {
            var sizeX = data.GetLength(0);
            var sizeY = data.GetLength(1);
            
            var hashX = Enumerable.Repeat("", sizeX).ToList();
            var hashY = Enumerable.Repeat("", sizeY).ToList();
            
            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                    hashX[x] += data[x, y];
                    hashY[y] += data[x, y];
                }
            }
            
            foreach (var info in GetMirror(hashX).Where(x => x.Diff == 0))
            {
                return info.Line;
            }
            
            foreach (var info in GetMirror(hashY).Where(x => x.Diff == 0))
            {
                return info.Line * 100;
            }

            throw new Exception();
        }

        private IEnumerable<Info> GetMirror(List<string> hashes)
        {
            for (var i = 1; i < hashes.Count; i++)
            {
                var tmpA = hashes.Take(i).ToList();
                var tmpB = hashes.Skip(i).ToList();

                if (tmpA.Count < tmpB.Count)
                {
                    tmpB = tmpB.Take(tmpA.Count).Reverse().ToList();
                }
                else
                {
                    tmpA = tmpA.TakeLast(tmpB.Count).Reverse().ToList();
                }

                yield return new Info
                {
                    Line = i,
                    Diff = tmpA.Zip(tmpB).Sum(x => DiffCount(x.First, x.Second))
                };
            }
        }

        private int DiffCount(string a, string b)
        {
            return a.GetHashCode() == b.GetHashCode()
                ? 0
                : a.Zip(b).Count(x => x.First != x.Second);
        }

        private IEnumerable<char[,]> ReadData(IReadOnlyList<string> input)
        {
            foreach (var itemText in ReadOneItem(input))
            {
                var sizeX = itemText[0].Length;
                var sizeY = itemText.Count;
                var data  = new char[sizeX, sizeY];
                
                for (var y = 0; y < sizeY; y++)
                {
                    for (var x = 0; x < sizeX; x++)
                    {
                        data[x, y] = itemText[y][x];
                    }
                }

                yield return data;
            }
        }

        private IEnumerable<List<string>> ReadOneItem(IReadOnlyList<string> input)
        {
            var data = new List<string>();
            
            foreach (var s in input)
            {
                if (s == "")
                {
                    yield return data;
                    data = new List<string>();
                }
                else
                {
                    data.Add(s);
                }
            }
            
            yield return data;
        }
    }
}