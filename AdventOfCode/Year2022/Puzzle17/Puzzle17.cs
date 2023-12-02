﻿using AdventOfCode.Common;

namespace AdventOfCode.Year2022
{
    public class Puzzle17 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var jets = new Jets(input);
            var rocks = new Rocks();
            
            return new[]
            {
                DoPart1(jets.Clone(), rocks.Clone()).ToString(),
                DoPart2(jets.Clone(), rocks.Clone()).ToString()
            };
        }

        private ulong DoPart2(Jets jets, Rocks rocks)
        {
            //PrintMagicNumbers(jets.Clone(), rocks.Clone());
            
            var data = new char[7, 10000*4];
            
            for (var x = 0; x < data.GetLength(0); x++)
            {
                for (var y = 0; y < data.GetLength(1); y++)
                {
                    data[x, y] = '.';
                }
            }

            //Magic numbers
            var initSteps = 3471;
            var patternSize = 1735;
            var patternHeight = 2781;
            
            var top0 = SimRocks(data, initSteps, -1, jets, rocks);

            var stepsLeft = 1000000000000 - (ulong)initSteps;
            var patternCount = stepsLeft / (ulong)patternSize;
            var stepsRequired = stepsLeft % (ulong)patternSize;
            
            var top1 = SimRocks(data, (int)stepsRequired, top0, jets, rocks);

            
            return 1 + (ulong)top1 + patternCount * (ulong)patternHeight;
        }

        /*
        Если добавлять камни достаточно долго, то кучи камней начинают образовывать повторяющиеся формы.
        Этот метод выводит характеристики повторяющихся фрагментов
        !!!! 15617 J:4 R:1 25013 dSize:2781 dSteps:1735
        !!!! 17352 J:4 R:1 27794 dSize:2781 dSteps:1735
        !!!! 19087 J:4 R:1 30575 dSize:2781 dSteps:1735
        */
        private void PrintMagicNumbers(Jets jets, Rocks rocks)
        {
            var data = new char[7, 100000*4];
            
            for (var x = 0; x < data.GetLength(0); x++)
            {
                for (var y = 0; y < data.GetLength(1); y++)
                {
                    data[x, y] = '.';
                }
            }

            var top = 0;
            var lastTop = 0;
            var lastI = 0;
            for (var i = 0; i < 10000; i++)
            {
                if (jets.Index <= 4)
                {
                    Console.WriteLine($"!!!! {i} J:{jets.Index} R:{rocks.Index} {top} dSize:{top - lastTop} dSteps:{i - lastI}");
                    lastTop = top;
                    lastI = i;
                } 

                var localTop = AddNewRock(data, rocks, top);

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
            
            return SimRocks(data, 2022, -1, jets, rocks) + 1;
        }

        private int SimRocks(char[,] data, int steps, int top, Jets jets, Rocks rocks)
        {
            for (var i = 0; i < steps; i++)
            {
                var localTop = AddNewRock(data, rocks, top);

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

            return top;
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
            
            public int Index => _index;
            
            public char[,] GetNext()
            {
                _index = (_index + 1) % _rocks.Count;

                return _rocks[_index];
            }

            public Rocks Clone()
            {
                return new Rocks
                {
                    _index = _index
                };
            }
        }
        
        private class Jets
        {
            private readonly List<int> _jets;
            
            private int _index = -1;
            
            public int Index => _index;
            
            public Jets(IReadOnlyList<string> input)
            {
                var text = input[0];
                
                _jets = Enumerable.Range(0, text.Length)
                    .Select(x => text[x] == '<' ? -1 : +1)
                    .ToList();
            }
            
            public Jets(List<int> jets)
            {
                _jets = jets;
            }

            public int GetNext()
            {
                _index = (_index + 1) % _jets.Count;

                return _jets[_index];
            }
            
            public Jets Clone()
            {
                return new Jets(_jets)
                {
                    _index = _index
                };
            }
        }
    }
}
