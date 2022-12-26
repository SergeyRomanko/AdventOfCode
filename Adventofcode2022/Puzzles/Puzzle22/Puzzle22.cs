using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle22 : Puzzle
    {
        private struct Pos
        {
            public Dir Dir;
            public int X;
            public int Y;
        }

        private Dictionary<string, string> _map = new Dictionary<string, string>
        {
            ["C01"] = "E10",
            ["C02"] = "F01",
            ["C13"] = "G31",
            ["E01"] = "C10",
            ["F01"] = "C02",
            ["E02"] = "L32",
            ["G13"] = "L10",
            ["E23"] = "K32",
            ["F23"] = "K20",
            ["L01"] = "G13",
            ["K02"] = "F32",
            ["L13"] = "C31",
            ["K23"] = "E32",
            ["L23"] = "E20",
        };

        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var field1 = ReadField(input.Take(input.Count - 2).ToList());
            var steps = ReadSteps(input[^1]).ToList();
            var startX = input[0].ToCharArray().TakeWhile(x => x == ' ').Count();
            
            var field2 = PreprocessField(field1);
            
            Util.PrintMap(field2);

            return new[]
            {
                DoTask(field1, steps, startX, 1).ToString(),   //75388
                DoTask(field2, steps, startX, 2).ToString()
            };
        }

        private int DoTask(char[,] field, List<string> steps, int startX, int task)
        {
            var pos = new Pos
            {
                Dir = new Dir(1, 0),
                X = startX,
                Y = 0
            };

            foreach (var step in steps)
            {
                pos = Move(pos, step, field, task);
            }

            return GetResult(pos);
        }

        private Pos Move(in Pos pos, string step, char[,] field, int task)
        {
            var result = pos;
            
            if (int.TryParse(step, out var moves))
            {
                for (var i = 0; i < moves; i++)
                {
                    var nextPos = (task == 1) 
                        ? GetNextPos1(result, field) 
                        : GetNextPos2(result, field);
                    
                    //Console.WriteLine($"{result.X,2}:{result.Y,2} {nextPos.X,2}:{nextPos.Y,2} {field.GetLength(0)}:{field.GetLength(1)}");

                    if (field[nextPos.X, nextPos.Y] == '#')
                    {
                        break;
                    }

                    result = nextPos;

                    var tmp = field[result.X, result.Y];
                        
                    field[result.X, result.Y] = '@';
                    
                    Util.PrintMap(field);

                    field[result.X, result.Y] = tmp;
                }
            }
            else
            {
                switch (step)
                {
                    case "L":
                        result.Dir = new (+pos.Dir.Y, -pos.Dir.X);
                        break;
                    case "R":
                        result.Dir = new (-pos.Dir.Y, +pos.Dir.X);
                        break;
                }
            }
            
            return result;
        }

        
        private Pos GetNextPos2(in Pos pos, char[,] field)
        {
            var result = pos;
            
            var sideSizeX = field.GetLength(0) / 4;
            var sideSizeY = field.GetLength(1) / 3;

            while (true)
            {
                result.X += pos.Dir.X;
                result.Y += pos.Dir.Y;

                var deltaX = result.X % sideSizeX;
                var deltaY = result.Y % sideSizeX;
                var negDeltaX = sideSizeX - deltaX - 1;
                var negDeltaY = sideSizeY - deltaY - 1;
                
                
                
            }

            return result;
        }


        private string PosToSide(Pos pos, char[,] field)
        {
            var result = 'A' + pos.X + pos.Y * field.GetLength(0);
            return result.ToString();
        }
        
        private Pos SideToPos(string side, char[,] field)
        {
            var sizeX = field.GetLength(0);
            var sideChar = side[0];

            return new Pos
            {
                X = sideChar % sizeX,
                Y = sideChar / sizeX,
            };
        }
        
        private Pos GetNextPos1(in Pos pos, char[,] field)
        {
            var result = pos;
            
            while (true)
            {
                result.X += pos.Dir.X;
                result.Y += pos.Dir.Y;

                if (result.X == field.GetLength(0)) result.X = 0;
                if (result.Y == field.GetLength(1)) result.Y = 0;
                if (result.X == -1) result.X = field.GetLength(0) - 1;
                if (result.Y == -1) result.Y = field.GetLength(1) - 1;

                if (field[result.X, result.Y] == ' ')
                {
                    continue;
                }

                return result;
            }
        }

        private int GetResult(in Pos pos)
        {
            var dir = pos.Dir switch
            {
                (1,  0) => 0,
                (0,  1) => 1,
                (-1, 0) => 2,
                (0, -1) => 3
            };

            return 1000 * (pos.Y + 1) + 4 * (pos.X + 1) + dir;
        }
        
        private char[,] PreprocessField(char[,] field)
        {
            var length0 = field.GetLength(0);
            var length1 = field.GetLength(1);
            
            var result = new char[length0, length1];
            
            for (var y = 0; y < length1; y++)
            {
                for (var x = 0; x < length0; x++)
                {
                    result[x, y] = field[x, y];
                }
            }

            var sideSizeX = length0 / 4;
            var sideSizeY = length1 / 3;

            for (var y = 0; y < sideSizeY; y++)
            {
                for (var x = 0; x < sideSizeX; x++)
                {
                    result[x + (sideSizeX * 0), y + (sideSizeY * 0)] = '1';
                    result[x + (sideSizeX * 1), y + (sideSizeY * 0)] = '2';
                    result[x + (sideSizeX * 3), y + (sideSizeY * 0)] = '3';
                    result[x + (sideSizeX * 3), y + (sideSizeY * 1)] = '4';
                    result[x + (sideSizeX * 0), y + (sideSizeY * 2)] = '5';
                    result[x + (sideSizeX * 1), y + (sideSizeY * 2)] = '6';
                }
            }

            return result;
        }

        private char[,] ReadField(IReadOnlyList<string> input)
        {
            var result = new char[input.Max(x => x.Length), input.Count];
            
            for (var y = 0; y < result.GetLength(1); y++)
            {
                for (var x = 0; x < result.GetLength(0); x++)
                {
                    result[x, y] = x >= input[y].Length
                        ? ' '
                        : input[y][x];
                }
            }
            
            return result;
        }

        private IEnumerable<string> ReadSteps(string line)
        {
            var util = new List<char>();

            for (var i = 0; i < line.Length; i++)
            {
                var cur = line[i];

                if (cur is 'L' or 'R')
                {
                    if (util.Count > 0)
                    {
                        yield return string.Join("", util);
                        
                        util.Clear();
                    }

                    yield return cur.ToString();
                }
                else
                {
                    util.Add(cur);
                }
            }

            if (util.Count > 0)
            {
                yield return string.Join("", util);
            }
        }
        
        private struct Dir : IEquatable<Dir>
        {
            public static readonly Dir Right = new() { X = +1, Y =  0 };
            public static readonly Dir Left  = new() { X = -1, Y =  0 };
            public static readonly Dir Up    = new() { X =  0, Y = -1 };
            public static readonly Dir Down  = new() { X =  0, Y = +1 };
            
            public int X;
            public int Y;

            public Dir(int x, int y)
            {
                X = x;
                Y = y;
            }

            public void Deconstruct(out int x, out int y)
            {
                x = X;
                y = Y;
            }

            public bool Equals(Dir other)
            {
                return X == other.X && Y == other.Y;
            }

            public override bool Equals(object? obj)
            {
                return obj is Dir other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }
            
            public static bool operator ==(Dir left, Dir right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Dir left, Dir right)
            {
                return !left.Equals(right);
            }
        }
    }
}
