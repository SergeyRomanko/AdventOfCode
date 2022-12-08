using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle08 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var size = input[0].Length;
            var items = input
                .SelectMany(x => x.ToCharArray())
                .Select(x => int.Parse(x.ToString()))
                .ToList();

            var trees = new int[size,size];
            for (var i = 0; i < items.Count; i++)
            {
                trees[i % size, i / size] = items[i];
            }
            
            var a = new int[size,size];
            var b = new int[size,size];
            var c = new int[size,size];
            var d = new int[size,size];

            int x = 0;
            int y = 0;
            
            for (x = 0; x < size; x++)
            {
                for (y = 0; y < size; y++)
                {
                    a[x, y] = (x == 0)
                        ? trees[x,y]
                        : Math.Max(a[x - 1, y], trees[x,y]);
                }
            }
            
            
            for (x = size - 1; x >= 0; x--)
            {
                for (y = 0; y < size; y++)
                {
                    b[x, y] = (x == size - 1)
                        ? trees[x,y]
                        :Math.Max(b[x + 1, y], trees[x,y]);
                }
            }
            
            for (x = 0; x < size; x++)
            {
                for (y = 0; y < size; y++)
                {
                    c[x, y] = (y == 0) 
                        ? trees[x,y]
                        : Math.Max(c[x , y - 1], trees[x,y]);
                }
            }
            
            for (x = 0; x < size; x++)
            {
                for (y = size - 1; y >= 0; y--)
                {
                    d[x, y] = (y == size -1)
                        ? trees[x,y]
                        : Math.Max(d[x , y + 1], trees[x,y]);
                }
            }

            var result = 0;
            for (x = 0; x < size; x++)
            {
                for (y = 0; y < size; y++)
                {
                    if (x == 0 || y == 0 || x == (size - 1) || y == (size - 1))
                    {
                        result++;
                        continue;
                    }

                    if (trees[x, y] > a[x-1,y] || trees[x, y] > b[x+1,y] || trees[x, y] > c[x,y-1] || trees[x, y] > d[x,y+1])
                    {
                        result++;
                    }
                }
            }

            var result2 = -1;
            
            for (x = 0; x < size; x++)
            {
                for (y = 0; y < size; y++)
                {
                    result2 = Math.Max(TestTree(trees, (x, y), size), result2);
                }
            }
            
            return new[]
            {
                result.ToString(),
                result2.ToString()
            };
        }

        private (int, int)[] offsets = new (int, int)[]
        {
            (+1, 0),
            (-1, 0),
            (0, +1),
            (0, -1),
        };

        private int TestTree(int[,] trees, (int, int) pos, int size)
        {
            var locks = new bool[4];
            var dists = new int[4];
            var orig = trees[pos.Item1, pos.Item2];
            
            for (int z = 1; z < size - 1; z++)
            {
                for (int i = 0; i < 4; i++)
                {
                    var (x,y) = (pos.Item1 + offsets[i].Item1 * z, pos.Item2 + offsets[i].Item2 * z);
                    
                    if (x < 0 || x >= size || y < 0 || y >= size)
                    {
                        locks[i] = true;
                        continue;
                    }
                    
                    if (locks[i])
                    {
                        continue;
                    }
                    
                    dists[i]++;
                    
                    var tree = trees[x, y];
                    if (tree >= orig)
                    {
                        locks[i] = true;
                    }
                }
            }

            return dists.Aggregate((a, b) => a*b);
        }
    }
}
