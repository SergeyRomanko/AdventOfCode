using AdventOfCode.Common;

namespace AdventOfCode.Year2022
{
    public class Puzzle01 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var inputList = input.ToList();
            
            return new[]
            {
                GetElvesCaloriesOrdered(inputList).First().ToString(),
                GetElvesCaloriesOrdered(inputList).Take(3).Sum().ToString()
            };
        }

        private IEnumerable<int> GetElvesCaloriesOrdered(IEnumerable<string> input)
        {
            return string
                .Join("x", input)
                .Split(new[] { "xx" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split('x').Sum(int.Parse))
                .OrderByDescending(x => x);
        }
    }
}
