using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle14 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {

            return new[]
            {
                Result(Part1(ReadData(input))).Sum().ToString(),
                Part2(ReadData(input)).ToString(),
            };
        }
        
        private int Part2(char[,] data)
        {
            var results = new List<int>();
            
            //Ниже много магии
            for (int i = 0; i < 18 * 9; i++)
            {
                Step(data, Direction.Up);
                Step(data, Direction.Left);
                Step(data, Direction.Down);
                Step(data, Direction.Right);
                
                results.Add(Result(data).Sum());
            }

            results.Reverse();

            var pattern = new List<int>();

            for (int i = 1; i < 100; i++)
            {
                var a = results.Take(i);
                var b = results.Skip(i).Take(i);

                if (a.SequenceEqual(b))
                {
                    pattern = a.ToList();
                    break;
                }
            }

            pattern.Reverse();
            
            return pattern[(1000000000 - 1) % pattern.Count]; //90795
        }
        
        private char[,] Part1(char[,] data)
        {
            Step(data, Direction.Up);
            
            return data;
        }

        private void Step(char[,] data, Direction dir)
        {
            var sizeX = data.GetLength(0);
            var sizeY = data.GetLength(1);
            
            var flag = true;
            while (flag)
            {
                flag = false;

                if (dir == Direction.Up)
                {
                    for (var y = 0; y < sizeY - 1; y++)
                    {
                        for (var x = 0; x < sizeX; x++)
                        {
                            if (data[x, y] == '.' && data[x, y + 1] == 'O')
                            {
                                data[x, y]     = 'O';
                                data[x, y + 1] = '.';
                                flag           = true;
                            }
                        }
                    }
                }
                else if (dir == Direction.Down)
                {
                    for (var y = 1; y < sizeY; y++)
                    {
                        for (var x = 0; x < sizeX; x++)
                        {
                            if (data[x, y] == '.' && data[x, y - 1] == 'O')
                            {
                                data[x, y]     = 'O';
                                data[x, y - 1] = '.';
                                flag           = true;
                            }
                        }
                    }
                }
                else if (dir == Direction.Left)
                {
                    for (var y = 0; y < sizeY; y++)
                    {
                        for (var x = 0; x < sizeX - 1; x++)
                        {
                            if (data[x, y] == '.' && data[x + 1, y] == 'O')
                            {
                                data[x, y]         = 'O';
                                data[x + 1, y] = '.';
                                flag               = true;
                            }
                        }
                    }
                }
                else if (dir == Direction.Right)
                {
                    for (var y = 0; y < sizeY; y++)
                    {
                        for (var x = 1; x < sizeX; x++)
                        {
                            if (data[x, y] == '.' && data[x - 1, y] == 'O')
                            {
                                data[x, y]     = 'O';
                                data[x - 1, y] = '.';
                                flag           = true;
                            }
                        }
                    }
                }
            }
        }

        private IEnumerable<int> Result(char[,] data)
        {
            var sizeX = data.GetLength(0);
            var sizeY = data.GetLength(1);
            
            for (var y = 0; y < sizeY; y++)
            {
                for (var x = 0; x < sizeX; x++)
                {
                    if (data[x, y] == 'O')
                    {
                        yield return sizeY - y;
                    }
                }
            }
        }

        private char[,] ReadData(IReadOnlyList<string> input)
        {
            var sizeX = input[0].Length;
            var sizeY = input.Count;
            var data  = new char[sizeX, sizeY];

            for (var y = 0; y < sizeY; y++)
            {
                for (var x = 0; x < sizeX; x++)
                {
                    data[x, y] = input[y][x];
                }
            }

            return data;
        }
    }
}