using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle01 : Puzzle
    {
        public override string[] GetResults(IEnumerable<string> input)
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
