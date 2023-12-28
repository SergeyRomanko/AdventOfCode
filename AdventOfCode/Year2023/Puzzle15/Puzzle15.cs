using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle15 : Puzzle
    {
        private class Data
        {
            public string Name;
            public int    Value;
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var inputList = input.First().Split(',').ToList();
            
            return new[]
            {
                inputList.Select(Part1).Sum().ToString(),
                Part2(inputList).ToString()
            };
        }

        private int Part1(string value)
        {
            var current = 0;

            foreach (var c in value)
            {
                current += c;
                current *= 17;
                current %= 256;
            }
            
            return current;
        }

        private int Part2(List<string> value)
        {
            var map = Enumerable.Range(0, 256).Select(x => new List<Data>()).ToList();

            foreach (var data in value)
            {
                if (data.Contains('-'))
                {
                    var name = data.Replace("-", "");
                    var box  = Part1(name);
                    var val  = map[box].FirstOrDefault(x => x.Name == name);

                    if (val != null)
                    {
                        map[box].Remove(val);
                    }
                    
                    continue;
                }

                var split = data.Split("=");
                var name1 = split[0];
                var box1  = Part1(name1);
                var num   = int.Parse(split[1]);
                var val1  = map[box1].FirstOrDefault(x => x.Name == name1);
                
                if (val1 != null)
                {
                    val1.Value = num;
                }
                else
                {
                    map[box1].Add(new Data
                    {
                        Name  = name1,
                        Value = num
                    });
                }
            }

            var result = 0;
            
            for (var x = 0; x < map.Count; x++)
            {
                for (var y = 0; y < map[x].Count; y++)
                {
                    result += (x + 1) * (y + 1) * map[x][y].Value;
                }
            }
            
            return result;
        }
    }
}