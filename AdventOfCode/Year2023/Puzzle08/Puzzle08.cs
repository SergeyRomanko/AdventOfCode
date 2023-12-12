using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle08 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var inputList = input.ToList();

            var dirs = inputList[0].Select(x => x == 'L' ? 0 : 1).ToList();

            var data = inputList
                .Skip(2)
                .ToDictionary(
                    x => x.Split(" = ")[0],
                    x => GetData(x.Split(" = ")[1])
                );
            
            return new[]
            {
                Part1(data, dirs).ToString(),
                Part2(data, dirs).ToString(),
            };
        }

        private int Part1(Dictionary<string, List<string>> data, List<int> dirs)
        {
            var counter = 0;
            var current = "AAA";
            
            while (current != "ZZZ")
            {
                var dir = dirs[counter++ % dirs.Count];
                current = data[current][dir];
            }

            return counter;
        }
        
        private long Part2(Dictionary<string, List<string>> data, List<int> dirs)
        {
            var initial = data.Keys.Where(x => x.EndsWith("A")).ToList();
            var lists   = initial.Select(x => new List<int>()).ToList();
            
            //Тут магия
            for (int i = 0; i < 50000; i++)
            {
                var dir = dirs[i % dirs.Count];

                for (int j = 0; j < initial.Count; j++)
                {
                    initial[j] = data[initial[j]][dir];
                    if (initial[j].EndsWith("Z"))
                    {
                        lists[j].Add(i);
                    }
                }
            }
            
            foreach (var list in lists)
            {
                list.Reverse();
                list[0] -= list[1];
            }
            
            var data1  = lists.Select(x => (long) x[0]).ToArray();
            var result = FindLCMOfArray(data1);
            
            return result;
        }

        private List<string> GetData(string data)
        {
            var split = data.Split(", ");
            return new List<string>
            {
                split[0].Replace("(", ""),
                split[1].Replace(")", ""),
            };
        }

        static long nod(long a, long b)
        {
            return b == 0 ? a : nod (b, a % b);
        }
        
        static long FindLCM(long t1, long t2)
        {
            return t1 * (t2 / nod(t1, t2));
        }
        
        static long FindLCMOfArray(long[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
            {
                throw new ArgumentException("Array must contain at least one number");
            }

            long lcm = numbers[0];

            for (int i = 1; i < numbers.Length; i++)
            {
                lcm = FindLCM(lcm, numbers[i]);
            }

            return lcm;
        }
    }
}