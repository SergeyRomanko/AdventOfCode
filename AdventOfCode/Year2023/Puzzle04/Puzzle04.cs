
using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle04 : Puzzle
    {
        private class Lists
        {
            public int       Copies;
            public List<int> List1;
            public List<int> List2;
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var inputList = input.ToList();

            var input1 = ReadInput(inputList).ToList();
            
            return new[]
            {
                Part1(input1).Sum().ToString(),
                Part2(input1).Sum().ToString()
            };
        }

        private IEnumerable<int> Part1(List<Lists> data)
        {
            foreach (var lists in data)
            {
                var count = lists.List1.Count(x => lists.List2.Contains(x));
                
                if (count == 0)
                {
                    continue;
                }
                
                yield return (int) Math.Pow(2, count - 1);
            }
        }
        
        private IEnumerable<int> Part2(List<Lists> data)
        {
            for (var i = 0; i < data.Count; i++)
            {
                var lists = data[i];
                var count = lists.List1.Count(x => lists.List2.Contains(x));
                var copies = lists.Copies;
                
                for (var x = 0; x < count; x++)
                {
                    data[i + x + 1].Copies += copies;
                }
                
                yield return copies;
            }
        }

        private IEnumerable<Lists> ReadInput(List<string> inputList)
        {
            foreach (var line in inputList)
            {
                var lists = line.Split(":")[1];
                var split = lists.Split("|");

                yield return new Lists()
                {
                    Copies = 1,
                    List1 = ReadList(split[0]),
                    List2 = ReadList(split[1]),
                };
            }
        }

        private List<int> ReadList(string text)
        {
            return text.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        }
    }
}