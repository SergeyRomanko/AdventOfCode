using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle09 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var inputList = input.ToList();
            
            return new[]
            {
                inputList.Select(ReadInts).Select(Part1).Sum().ToString(),
                inputList.Select(ReadInts).Select(Part2).Sum().ToString(),
            };
        }
        
        private int Part1(List<int> args)
        {
            var data = new List<List<int>>{ args };
            var last = data.Last();
            
            while (last.Any(x => x != 0))
            {
                var next = Enumerable.Range(0, last.Count - 1)
                    .Select(x => last[x + 1] - last[x])
                    .ToList();

                data.Add(next);

                last = next;
            }
            
            last.Add(0);

            for (int i = data.Count - 2; i >= 0; i--)
            {
                var lastA = data[i + 0].Last();
                var lastB = data[i + 1].Last();
                
                data[i].Add(lastA + lastB);
            }

            return data[0].Last();
        }
        
        private int Part2(List<int> args)
        {
            var data = new List<List<int>>{ args };
            var last = data.Last();
            
            while (last.Any(x => x != 0))
            {
                var next = Enumerable.Range(0, last.Count - 1)
                    .Select(x => last[x + 1] - last[x])
                    .ToList();

                data.Add(next);

                last = next;
            }
            
            last.Insert(0, 0);

            for (int i = data.Count - 2; i >= 0; i--)
            {
                var firstA = data[i + 0].First();
                var firstB = data[i + 1].First();
                
                data[i].Insert(0, firstA - firstB);
            }

            return data[0].First();
        }
        
        private List<int> ReadInts(string arg)
        {
            return arg
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
        }
    }
}