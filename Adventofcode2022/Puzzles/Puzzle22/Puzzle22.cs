using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle22 : Puzzle
    {
        private class Pos
        {
            public (int X, int Y) Dir;
            public int X;
            public int Y;
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var field = ReadField(input.Take(input.Count - 2).ToList());
            var steps = ReadSteps(input[^1]).ToList();
            var startX = input[0].ToCharArray().TakeWhile(x => x == ' ').Count();
            
            return new[]
            {
                DoTask1(field, steps, startX).ToString(),
                ""
            };
        }

        private int DoTask1(char[,] field, List<string> steps, int startX)
        {
            var pos = new Pos
            {
                Dir = (1, 0),
                X = startX,
                Y = 0
            };

            foreach (var step in steps)
            {
                Move(pos, step, field);
            }

            var dir = pos.Dir switch
            {
                (1,  0) => 0,
                (0,  1) => 1,
                (-1, 0) => 2,
                (0, -1) => 3
            };

            return 1000 * (pos.Y + 1)  + 4 * (pos.X + 1) + dir;
        }

        private void Move(Pos pos, string step, char[,] field)
        {
            if (int.TryParse(step, out var moves))
            {
                for (var i = 0; i < moves; i++)
                {
                    var nextPos = GetNextPos(pos, field);

                    if (field[nextPos.Item1, nextPos.Item2] == '#')
                    {
                        break;
                    }

                    pos.X = nextPos.Item1;
                    pos.Y = nextPos.Item2;
                }
            }
            else
            {
                switch (step)
                {
                    case "L":
                        pos.Dir = (+pos.Dir.Y, -pos.Dir.X);
                        break;
                    case "R":
                        pos.Dir = (-pos.Dir.Y, +pos.Dir.X);
                        break;
                }
            }
        }

        private (int, int) GetNextPos(Pos pos, char[,] field)
        {
            var result = (pos.X, pos.Y);
            while (true)
            {
                result.X += pos.Dir.Item1;
                result.Y += pos.Dir.Item2;

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

        private char[,] ReadField(IReadOnlyList<string> input)
        {
            var result = new char[input[0].Length, input.Count];
            
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
    }
}
