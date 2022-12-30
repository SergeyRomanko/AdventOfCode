using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle03 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            return new[]
            {
                input
                    .Select(x => (A: x.Substring(0, x.Length/2), B: x.Substring(x.Length/2)))
                    .Sum(x => GetRucksackResult(x.A, x.B))
                    .ToString(),
                
                Enumerable
                    .Range(0, input.Count/3)
                    .Select(i => (A:input[i*3+0], B:input[i*3+1], C:input[i*3+2]))
                    .Sum(x => GetGroupResult(x.A, x.B, x.C))
                    .ToString()
            };
        }

        private int GetGroupResult(string a, string b, string c)
        {
            var arr = new int[52 + 1];
            
            for (var i = 0; i < a.Length; i++)
            {
                var index = (a[i] % 32) + ((a[i] & 32) == 0 ? 26 : 0);
                arr[index] |= 0x001;
            }
            
            for (var i = 0; i < b.Length; i++)
            {
                var index = (b[i] % 32) + ((b[i] & 32) == 0 ? 26 : 0);
                arr[index] |= 0x010;
            }
            
            for (var i = 0; i < c.Length; i++)
            {
                var index = (c[i] % 32) + ((c[i] & 32) == 0 ? 26 : 0);
                arr[index] |= 0x100;
            }
            
            return Array.IndexOf(arr, 0x111);
        }

        private int GetRucksackResult(string a, string b)
        {
            var arr = new int[52 + 1];
            
            for (var i = 0; i < a.Length; i++)
            {
                var indexA = (a[i] % 32) + ((a[i] & 32) == 0 ? 26 : 0);
                var indexB = (b[i] % 32) + ((b[i] & 32) == 0 ? 26 : 0);

                arr[indexA] |= 0x01;
                arr[indexB] |= 0x10;
            }

            return Array.IndexOf(arr, 0x11);
        }
    }
}
