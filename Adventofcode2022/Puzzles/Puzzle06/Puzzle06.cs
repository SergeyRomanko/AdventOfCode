using System;
using System.Collections.Generic;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle06 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            return new[]
            {
                DoThings(input[0], 04).ToString(),
                DoThings(input[0], 14).ToString(),
            };
        }

        private int DoThings(string input, int len)
        {
            for (var i = 0; i < input.Length - len; i++)
            {
                if (IsUnique(input.AsMemory(i, len)))
                {
                    return i + len;
                }
            }

            return -1;
        }

        private bool IsUnique(ReadOnlyMemory<char> span)
        {
            var hash = new HashSet<char>(span.ToArray());

            return span.Length == hash.Count;
        }
    }
}
