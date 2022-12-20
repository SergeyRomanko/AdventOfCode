using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle20 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var data = input
                .Select(int.Parse)
                .ToList();
            
            return new[]
            {
                DoTask1(data).ToString(),
                ""
            };
        }

        private int DoTask1(List<int> data)
        {
            var clone = new List<int>(data);

            for (var i = 0; i < data.Count; i++)
            {
                var offset = data[i];
                var index = clone.IndexOf(offset);
                
                Move(clone, index, offset);
                
                Console.WriteLine($" >>> {offset,2} {index,2} | {string.Join(" ", clone.Select(x => $"{x, 2}"))}");
            }
            
            return 0;
        }

        private void Move(List<int> data, int index, int offset)
        {
            var rawIndex = (index + offset) % data.Count;

            var newIndex = rawIndex < 0
                ? data.Count + rawIndex
                : rawIndex;
            
            data.RemoveAt(index);
            
            data.Insert(newIndex, offset);
        }
    }
}
