using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle13 : Puzzle
    {

        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var par1Data = ReadPart1Data(input).ToList();
            
            return new[]
            {
                par1Data.Select(Part1).Sum().ToString()
            };
        }

        private int Part1(char[,] data)
        {
            var sizeX = data.GetLength(0);
            var sizeY = data.GetLength(1);
            
            var hashX = Enumerable.Repeat(0, sizeX).ToList();
            var hashY = Enumerable.Repeat(0, sizeY).ToList();
            
            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                    hashX[x] = HashCode.Combine(data[x, y], hashX[x]);
                    hashY[y] = HashCode.Combine(data[x, y], hashY[y]);
                }
            }

            var resultX = GetMirror(hashX);
            if (resultX != -1)
            {
                return resultX;
            }
            
            var resultY = GetMirror(hashY);
            if (resultY != -1)
            {
                return resultY * 100;
            }

            throw new Exception();
        }

        private int GetMirror(List<int> hashes)
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

                if (tmpA.SequenceEqual(tmpB))
                {
                    return i;
                }
            }

            return -1;
        }

        private IEnumerable<char[,]> ReadPart1Data(IReadOnlyList<string> input)
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