using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle10 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            return new[]
            {
                Run1(1, input.SelectMany(Preprocess).ToList()).Sum().ToString(),   //14420
                Run2(1, input.SelectMany(Preprocess).ToList())
            };
        }

        private string Run2(int reg, IReadOnlyList<int> input)
        {
            var result = new string[40, 6];

            for (var i = 0; i < 40 * 6; i++)
            {
                var x = i % 40;
                var y = i / 40;

                result[x, y] = Math.Abs(reg - x) <= 1 ? "#" : ".";

                reg += input[i];
            }

            return "\n" + string.Join("\n", Enumerable.Range(0, 6).Select(
                y => string.Join("", Enumerable.Range(0, 40).Select(x => result[x, y]))
            ));
        }

        private IEnumerable<int> Run1(int reg, IReadOnlyList<int> input)
        {
            for (var i = 1; i <= input.Count; i++)
            {
                if ((i - 20) % 40 == 0)
                {
                    yield return reg * i;
                }
                
                reg += input[i - 1];
            }
        }

        private int[] Preprocess(string s)
        {
            var split = s.Split(' ');
            
            return split[0] switch
            {
                "noop" => new[] { 0 },
                "addx" => new[] { 0, int.Parse(split[1]) },
                
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
