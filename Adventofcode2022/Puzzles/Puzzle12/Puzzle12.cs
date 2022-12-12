using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle12 : Puzzle
    {
        private struct Vec2
        {
            public int X;
            public int Y;

            public Vec2(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
        
        private class Node
        {
            public char Height;
            public int Length;
            public Node? Prev;
            public bool IsVisited;
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var sizeX = input[0].Length;
            var sizeY = input.Count;

            var map = new char[sizeX, sizeY];

            var S = default(Vec2);
            var E = default(Vec2);
            
            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                    var tmp = input[y][x];
                    map[x, y] = tmp switch
                    {
                        'S' => 'a',
                        'E' => 'z',
                        _ => tmp
                    };

                    S = (tmp == 'S') ? new Vec2(x, y) : S;
                    E = (tmp == 'E') ? new Vec2(x, y) : E;
                }
            }

            var r1 = DoThings(S, E, map, sizeX, sizeY);

            var r2 = DoThings2(E, map, sizeX, sizeY);
            
            return new[]
            {
                r1.ToString(),
                r2.ToString()
            };
        }

        private int DoThings(Vec2 s, Vec2 e, char[,] map, int sizeX, int sizeY)
        {
            var submap = new Node[sizeX ,sizeY];
            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                    var isStart = s.X == x && s.Y == y;
                    submap[x, y] = new Node
                    {
                        Height = map[x, y],
                        Length = isStart ? 0 : int.MaxValue,
                        IsVisited = false
                    };
                }
            }
            
            var i = 0;
            while (true)
            {
                var cur = Enumerable
                    .Range(0, sizeX * sizeY)
                    .Select(x => new Vec2(x % sizeX, x / sizeX))
                    .Where(x => !submap[x.X, x.Y].IsVisited)
                    .Aggregate((a, b) => submap[a.X, a.Y].Length < submap[b.X, b.Y].Length ? a : b);
                
                var neib = new Vec2[]
                {
                    new Vec2(cur.X+1, cur.Y),
                    new Vec2(cur.X-1, cur.Y),
                    new Vec2(cur.X, cur.Y+1),
                    new Vec2(cur.X, cur.Y-1),
                };

                var next = neib
                    .Where(n => n.X >= 0 && n.X < sizeX && n.Y >= 0 && n.Y < sizeY)
                    .Where(n => !submap[n.X, n.Y].IsVisited)
                    .Where(n => submap[n.X, n.Y].Height <= submap[cur.X, cur.Y].Height + 1)
                    .Where(n => submap[n.X, n.Y].Length > submap[cur.X, cur.Y].Length + 1);

                foreach (var n in next)
                {
                    submap[n.X, n.Y].Length = submap[cur.X, cur.Y].Length + 1;
                    submap[n.X, n.Y].Prev   = submap[cur.X, cur.Y];
                }

                if (submap[e.X, e.Y].Prev != null)
                {
                    break;
                }

                submap[cur.X, cur.Y].IsVisited = true;
            }

            var tmp = submap[e.X, e.Y];
            var result = 0;
            
            while (tmp != null)
            {
                tmp = tmp.Prev;
                result++;
            }
            
            return result - 1;
        }
        
        private int DoThings2(Vec2 e, char[,] map, int sizeX, int sizeY)
        {
            var submap = new Node[sizeX ,sizeY];
            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                    var isEnd = e.X == x && e.Y == y;
                    submap[x, y] = new Node
                    {
                        Height = map[x, y],
                        Length = isEnd ? 0 : int.MaxValue,
                        IsVisited = false
                    };
                }
            }
            
            var i = 0;
            var r = default(Vec2);
            
            while (true)
            {
                var cur = Enumerable
                    .Range(0, sizeX * sizeY)
                    .Select(x => new Vec2(x % sizeX, x / sizeX))
                    .Where(x => !submap[x.X, x.Y].IsVisited)
                    .Aggregate((a, b) => submap[a.X, a.Y].Length < submap[b.X, b.Y].Length ? a : b);
                
                //Console.WriteLine($">>> Pos {cur.X}:{cur.Y} {submap[cur.X, cur.Y].Height}");
                
                var neib = new Vec2[]
                {
                    new Vec2(cur.X+1, cur.Y),
                    new Vec2(cur.X-1, cur.Y),
                    new Vec2(cur.X, cur.Y+1),
                    new Vec2(cur.X, cur.Y-1),
                };

                var next = neib
                    .Where(n => n.X >= 0 && n.X < sizeX && n.Y >= 0 && n.Y < sizeY)
                    .Where(n => !submap[n.X, n.Y].IsVisited)
                    .Where(n => submap[n.X, n.Y].Height >= submap[cur.X, cur.Y].Height - 1)
                    .Where(n => submap[n.X, n.Y].Length > submap[cur.X, cur.Y].Length + 1);

                var toList = next.ToList();
                
                var flag = false;
                foreach (var n in toList)
                {
                    submap[n.X, n.Y].Length = submap[cur.X, cur.Y].Length + 1;
                    submap[n.X, n.Y].Prev   = submap[cur.X, cur.Y];
                    if (submap[n.X, n.Y].Height == 'a')
                    {
                        r = n;
                        flag = true;
                    }
                }

                if (flag)
                {
                    break;
                }

                submap[cur.X, cur.Y].IsVisited = true;
            }

            var tmp = submap[r.X, r.Y];
            var result = 0;
            
            while (tmp != null)
            {
                tmp = tmp.Prev;
                result++;
            }
            
            return result - 1;
        }
    }
}
