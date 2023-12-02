using AdventOfCode.Common;

namespace AdventOfCode.Year2022
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
            ["B01"] = "J02",
            ["J02"] = "B01",

            ["C01"] = "J23",
            ["J23"] = "C01",
            
            ["B02"] = "G20",
            ["G02"] = "B20",
            
            ["E02"] = "G01",
            ["G01"] = "E02",
            
            ["E02"] = "G01",
            ["G01"] = "E02",

            ["C13"] = "H31",
            ["H13"] = "C31",
            
            ["C23"] = "E13",
            ["E13"] = "C23",
            
            ["H23"] = "J13",
            ["J13"] = "H23",
        };

        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var field = ReadField(input.Take(input.Count - 2).ToList());
            var steps = ReadSteps(input[^1]).ToList();
            var startX = input[0].ToCharArray().TakeWhile(x => x == ' ').Count();
            
            return new[]
            {
                DoTask(field, steps, startX, 1).ToString(),   //75388
                DoTask(field, steps, startX, 2).ToString()
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
                    
                    if (field[nextPos.X, nextPos.Y] == '#')
                    {
                        break;
                    }

                    result = nextPos;
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

            result.X += pos.Dir.X;
            result.Y += pos.Dir.Y;

            var isInvalid = result.X == field.GetLength(0) ||
                            result.Y == field.GetLength(1) ||
                            result.X == -1 ||
                            result.Y == -1 ||
                            field[result.X, result.Y] == ' ';

            if (isInvalid)
            {
                result = DoThings(in pos, field);
            }

            return result;
        }

        private Pos DoThings(in Pos pos, char[,] field)
        {
            var sideSizeX = field.GetLength(0) / 3;
            var sideSizeY = field.GetLength(1) / 4;

            var minX = 0;
            var minY = 0;
            var maxX = sideSizeX - 1;
            var maxY = sideSizeY - 1;
            
            var delta = 0;
            var negDelta = 0;

            var code = "";
            if (pos.Dir == Dir.Up)
            {
                code = "01";
                delta = pos.X % sideSizeX;
                negDelta = sideSizeX - delta - 1;
            }

            if (pos.Dir == Dir.Left)
            {
                code = "02";
                delta = pos.Y % sideSizeY;
                negDelta = sideSizeY - delta - 1;
            }

            if (pos.Dir == Dir.Right)
            {
                code = "13";
                delta = pos.Y % sideSizeY;
                negDelta = sideSizeY - delta - 1;
            }

            if (pos.Dir == Dir.Down)
            {
                code = "23";
                delta = pos.X % sideSizeX;
                negDelta = sideSizeX - delta - 1;
            }

            var from = $"{PosToSide(pos, field)}{code}";
            var to = _map[from];
            
            var offset = to.Substring(1) switch
            {
                "01" => (delta, minY),
                "10" => (negDelta, minY),
                "02" => (minX, delta),
                "20" => (minX, negDelta),
                "13" => (maxX, delta),
                "31" => (maxX, negDelta),
                "23" => (delta, maxY),
                "32" => (negDelta, maxY),
            };
            
            var dir = to.Substring(1) switch
            {
                "01" => Dir.Down,
                "10" => Dir.Down,
                "02" => Dir.Right,
                "20" => Dir.Right,
                "13" => Dir.Left,
                "31" => Dir.Left,
                "23" => Dir.Up,
                "32" => Dir.Up,
            };

            var result = SideToPos(to, field);

            result.X += offset.Item1;
            result.Y += offset.Item2;
            result.Dir = dir;
            
            return result;
        }

        private string PosToSide(Pos pos, char[,] field)
        {
            var sideSizeX = field.GetLength(0) / 3;
            var sideSizeY = field.GetLength(1) / 4;
            var sideX = pos.X / sideSizeX;
            var sideY = pos.Y / sideSizeY;
            var result = 'A' + sideX + sideY * 3;
            return ((char) result).ToString();
        }
        
        private Pos SideToPos(string side, char[,] field)
        {
            var sideX = 3;
            var sideSizeX = field.GetLength(0) / 3;
            var sideSizeY = field.GetLength(1) / 4;
            
            var sideChar = side[0] - 'A';

            return new Pos
            {
                X = (sideChar % sideX) * sideSizeX,
                Y = (sideChar / sideX) * sideSizeY,
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
