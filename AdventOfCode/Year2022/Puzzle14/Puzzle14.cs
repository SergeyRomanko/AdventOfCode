using AdventOfCode.Common;

namespace AdventOfCode.Year2022
{
    public class Puzzle14 : Puzzle
    {
        private struct Pos
        {
            public int X;
            public int Y;
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            return new[]
            {
                GetResultA(input).ToString(),
                GetResultB(input).ToString()
            };
        }
        
        private int GetResultB(IReadOnlyList<string> input)
        {
            var lines = input.Select(
                x => x
                    .Split(" -> ")
                    .Select(x => x.Split(","))
                    .Select(x => new Pos { X = int.Parse(x[0]), Y = int.Parse(x[1])})
                    .ToList()
            ).ToList();

            var sizeY = lines
                .SelectMany(x => x)
                .Max(x => x.Y) + 1 + 2;

            var size = new Pos { X = 1000, Y = sizeY };

            var data = new char[size.X, size.Y];
            
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    data[i, j] = j == size.Y - 1 ? '#' : '.';
                }
            }
            
            foreach (var line in lines)
            {
                AddLine(data, line);
            }

            var result = 0;

            while (data[500, 0] != '0')
            {
                Sim(data, size);
                
                result++;
            }
            
            Print(data, size);

            return result;
        }

        private int GetResultA(IReadOnlyList<string> input)
        {
            var lines = input.Select(
                x => x
                    .Split(" -> ")
                    .Select(x => x.Split(","))
                    .Select(x => new Pos { X = int.Parse(x[0]), Y = int.Parse(x[1])})
                    .ToList()
            ).ToList();

            var sizeY = lines
                .SelectMany(x => x)
                .Max(x => x.Y) + 1;

            var size = new Pos { X = 1000, Y = sizeY };

            var data = new char[size.X, size.Y];
            
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    data[i, j] = '.';
                }
            }
            
            foreach (var line in lines)
            {
                AddLine(data, line);
            }

            var result = 0;

            while (Sim(data, size))
            {
                result++;
            }
            
            return result;
        }

        private bool Sim(char[,] data, Pos size)
        {
            data[500, 0] = '0';
            
            for (int j = 0; j < size.Y; j++)
            {
                for (int i = 0; i < size.X; i++)
                {
                    if (data[i, j] != '0')
                    {
                        continue;
                    }

                    if (j == size.Y - 1)
                    {
                        return false;
                    }

                    if (data[i, j+1] == '.')
                    {
                        data[i, j] = '.';
                        data[i, j + 1] = '0';
                        break;
                    }
                    
                    if (data[i - 1, j+1] == '.')
                    {
                        data[i, j] = '.';
                        data[i - 1, j + 1] = '0';
                        break;
                    }
                    
                    if (data[i + 1, j+1] == '.')
                    {
                        data[i, j] = '.';
                        data[i + 1, j + 1] = '0';
                        break;
                    }
                }
            }

            return true;
        }

        private void Print(char[,] data, Pos size)
        {
            var text = "\n" + string.Join("\n", Enumerable.Range(0, size.Y).Select(
                y => string.Join("", Enumerable.Range(0, size.X).Select(x => data[x, y]))
            ));
            
            File.WriteAllText("TestTestesdfgdfg.txt", text);
            
            //Console.WriteLine(text);
        }

        private void AddLine(char[,] data, List<Pos> line)
        {
            for (var i = 1; i < line.Count; i++)
            {
                var from = line[i - 1];
                var to = line[i];
                
                var delta = new Pos
                {
                    X = to.X - from.X,
                    Y = to.Y - from.Y
                };

                var offset = new Pos
                {
                    X = delta.X / Math.Max(Math.Abs(delta.X), 1),
                    Y = delta.Y / Math.Max(Math.Abs(delta.Y), 1)
                };
                
                data[from.X, from.Y] = '#';

                while (from.X != to.X || from.Y != to.Y)
                {
                    from.X += offset.X;
                    from.Y += offset.Y;
                    
                    data[from.X, from.Y] = '#';
                }
            }
        }
    }
}
