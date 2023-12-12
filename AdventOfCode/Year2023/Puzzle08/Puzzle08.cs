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
                //Part1(data, dirs).ToString(),
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
        
        private int Part2(Dictionary<string, List<string>> data, List<int> dirs)
        {
            var initial = data.Keys.Where(x => x.EndsWith("A")).ToList();
            var lists   = initial.Select(x => new List<int>()).ToList();
            
            for (int i = 0; i < 3000000; i++)
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
            

            var data1  = lists.Select(x => x[0]).ToArray();
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

        static int FindGCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static int FindLCM(int a, int b)
        {
            return Math.Abs(a * b) / FindGCD(a, b);
        }

        static int FindLCMOfArray(int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
            {
                throw new ArgumentException("Array must contain at least one number");
            }

            int lcm = numbers[0];

            for (int i = 1; i < numbers.Length; i++)
            {
                lcm = FindLCM(lcm, numbers[i]);
            }

            return lcm;
        }
    }
}