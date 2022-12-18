using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle17 : Puzzle
    {
        private Jets _jets;
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var jets = new Jets(input);
            var rocks = new Rocks();
            
            return new[]
            {
                DoPart1(jets, rocks).ToString(),
                ""
            };
        }

        private int DoPart1(Jets jets, Rocks rocks)
        {
            var data = new char[7, 2022*4];
            
            for (var x = 0; x < data.GetLength(0); x++)
            {
                for (var y = 0; y < data.GetLength(1); y++)
                {
                    data[x, y] = '.';
                }
            }

            var top = -1;
            
            for (var i = 0; i < 2022; i++)
            {
                var localTop = AddNewRock(data, rocks, top);

                //Print(data, localTop);

                while (true)
                {
                    TryApplyJet(data, jets, localTop);
                    
                    if (!TryApplyGravity(data, localTop))
                    {
                        StopRock(data, localTop);
                        
                        break;
                    }
                    
                    localTop -= 1;
                }

                top = Math.Max(top, localTop);
            }

            return top + 1;
        }

        private void Print(char[,] data, int top)
        {
            return;
            
            for (var y = 15; y >= 0; y--)
            {
                var tmp = new List<char>();
                
                for (var x = 0; x < 7; x++)
                {
                    tmp.Add(data[x, y]);
                }
                
                Console.WriteLine(string.Join("", tmp));
            }
            
            Console.WriteLine();
        }

        private void StopRock(char[,] data, int top)
        {
            for (var x = 0; x < 7; x++)
            {
                for (var y = 0; y < 4; y++)
                {
                    if (!IsActive(data, x, top - y))
                    {
                        continue;
                    }

                    data[x, top - y] = '#';
                }
            }
        }

        private bool TryApplyGravity(char[,] data, int top)
        {
            var tmp = new List<(int, int)>();
            
            for (var x = 0; x < 7; x++)
            {
                for (var y = 0; y < 4; y++)
                {
                    if (!IsActive(data, x, top - y))
                    {
                        continue;
                    }

                    if (top - y - 1 < 0)
                    {
                        return false;
                    }
                    
                    if (data[x, top - y - 1] == '#')
                    {
                        return false;
                    }
                    
                    tmp.Add((x, top - y));
                }
            }
            
            foreach (var (x, y) in tmp)
            {
                data[x, y] = '.';
            }
            
            foreach (var (x, y) in tmp)
            {
                data[x, y - 1] = '@';
            }

            return true;
        }

        private void TryApplyJet(char[,] data, Jets jets, int top)
        {
            var tmp = new List<(int, int)>();
            
            var jet = jets.GetNext();
            
            for (var x = 0; x < 7; x++)
            {
                for (var y = 0; y < 4; y++)
                {
                    if (!IsActive(data, x, top - y))
                    {
                        continue;
                    }

                    var newX = x + jet;
                    if (newX is < 0 or >= 7)
                    {
                        return;
                    }

                    var target = data[newX, top - y];
                    if (target is not '.' and not '@')
                    {
                        return;
                    }
                    
                    tmp.Add((x, top - y));
                }
            }
            
            foreach (var (x, y) in tmp)
            {
                data[x, y] = '.';
            }
            
            foreach (var (x, y) in tmp)
            {
                data[x + jet, y] = '@';
            }
        }

        private bool IsActive(char[,] data, int x, int y)
        {
            return y >= 0 && data[x, y] == '@';
        }
        
        private int AddNewRock(char[,] data, Rocks rocks, int top)
        {
            var rock = rocks.GetNext();

            top += 3 + rock.GetLength(1);
            
            for (var x = 0; x < rock.GetLength(0); x++)
            {
                for (var y = 0; y < rock.GetLength(1); y++)
                {
                    data[x + 2, top - y] = rock[x, y];
                }
            }

            return top;
        }

        private class Rocks
        {
            private readonly List<char[,]> _rocks = new List<char[,]>
            {
                new char[,]
                {
                    {'@'},
                    {'@'},
                    {'@'},
                    {'@'},
                },
                
                new char[,]
                {
                    {'.','@','.'},
                    {'@','@','@'},
                    {'.','@','.'},
                },
                new char[,]
                {
                    {'.','.','@'},
                    {'.','.','@'},
                    {'@','@','@'},
                },
                new char[,]
                {
                    {'@','@','@','@'}
                },
                new char[,]
                {
                    {'@','@'},
                    {'@','@'},
                }
            };

            private int _index = -1;
            
            public char[,] GetNext()
            {
                _index = (_index + 1) % _rocks.Count;

                return _rocks[_index];
            }
        }
        
        private class Jets
        {
            private readonly List<int> _jets;
            
            private int _index = -1;
            
            public Jets(IReadOnlyList<string> input)
            {
                var text = input[0];
                
                _jets = Enumerable.Range(0, text.Length)
                    .Select(x => text[x] == '<' ? -1 : +1)
                    .ToList();
            }

            public int GetNext()
            {
                _index = (_index + 1) % _jets.Count;

                return _jets[_index];
            }
        }
    }
}
