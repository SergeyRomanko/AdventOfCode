using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle10 : Puzzle
    {
        public class Cell
        {
            public Vec2        Pos;
            public int         Dist;
            public bool        IsPath;
            public Direction[] Dirs;
            public List<Cell>  Neighb = new ();
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var inputList = input.ToList();

            var grid = BuildGrid(inputList);
            var start = GetStart(inputList, grid);
            
            return new[]
            {
                Part1(start).ToString(),
                Part2(grid, start).ToString(),
            };
        }

        private int Part2(Cell[,] grid, Cell start)
        {
            //Fix start dirs
            var startNeigh = start.Neighb.Select(x => x.Pos).ToList();
            start.Dirs = start.Dirs
                .Where(x => startNeigh.Contains(start.Pos + x.ToVec()))
                .ToArray();
            
            var sizeX = grid.GetLength(0);
            var sizeY = grid.GetLength(1);
            
            var sizeX2 = sizeX * 2;
            var sizeY2 = sizeY * 2;

            var newGrid = new int[sizeX2, sizeY2];
            
            for (var y = 0; y < sizeY; y++)
            {
                for (var x = 0; x < sizeX; x++)
                {
                    if (!grid[x, y].IsPath)
                    {
                        continue;
                    }
                    
                    newGrid[x * 2, y * 2] = 1;
                    
                    if (grid[x, y].Dirs.Contains(Direction.Right))
                    {
                        newGrid[(x * 2) + 1, y * 2] = 1;
                    }
                    
                    if (grid[x, y].Dirs.Contains(Direction.Down))
                    {
                        newGrid[x * 2, (y * 2) + 1] = 1;
                    }
                }
            }

            var dirs = new List<Vec2>
            {
                Vec2.Up,
                Vec2.Down,
                Vec2.Left,
                Vec2.Right
            };
            
            var candidates = new Queue<Vec2>();

            for (var y = 0; y < sizeY2; y++)
            {
                for (var x = 0; x < sizeX2; x++)
                {
                    if (x == 0 || x == sizeX2 - 1 || y == 0 || y == sizeY2 - 1)
                    {
                        if (newGrid[x, y] == 0)
                        {
                            candidates.Enqueue(new Vec2(x, y));
                        }
                    }
                }
            }

            while (candidates.TryDequeue(out var cell))
            {
                newGrid[cell.x, cell.y] = 1;

                foreach (var vec2 in dirs)
                {
                    var neighPos = cell + vec2;

                    if (neighPos.x < 0 || neighPos.x >= sizeX2 || neighPos.y < 0 || neighPos.y >= sizeY2)
                    {
                        continue;
                    }

                    if (newGrid[neighPos.x, neighPos.y] == 0)
                    {
                        candidates.Enqueue(neighPos);
                    }
                }
            }

            var isInsideCounter = 0;
                
            for (var y = 0; y < sizeY; y++)
            {
                for (var x = 0; x < sizeX; x++)
                {
                    if (newGrid[x * 2, y * 2] == 0)
                    {
                        isInsideCounter++;
                    }
                }
            }
            
            return isInsideCounter;
        }

        private int Part1(Cell start)
        {
            var processed  = new List<Cell>();
            var candidates = new Queue<Cell>(new List<Cell>{start});

            while (candidates.TryDequeue(out var cell))
            {
                foreach (var neighb in cell.Neighb)
                {
                    if (processed.Contains(neighb))
                    {
                        continue;
                    }

                    neighb.Dist  = cell.Dist + 1;
                    candidates.Enqueue(neighb);
                }
                
                cell.IsPath = true;
                
                processed.Add(cell);
            }

            return processed.Max(x => x.Dist);
        }

        private Cell[,] BuildGrid(List<string> inputList)
        {
            var sizeX = inputList[0].Length;
            var sizeY = inputList.Count;

            var result = new Cell[sizeX, sizeY];
            
            for (var y = 0; y < sizeY; y++)
            {
                for (var x = 0; x < sizeX; x++)
                {
                    result[x, y] = new Cell
                    {
                        Dirs = GetDirs(inputList[y][x]),
                        Pos  = new Vec2(x, y)
                    };
                }
            }
            
            for (var y = 0; y < sizeY; y++)
            {
                for (var x = 0; x < sizeX; x++)
                {
                    var cur = result[x, y];

                    foreach (var dir in cur.Dirs)
                    {
                        var neighPos = dir.ToVec() + new Vec2(x, y);

                        if (neighPos.x < 0 || neighPos.x >= sizeX || neighPos.y < 0 || neighPos.y >= sizeY)
                        {
                            continue;
                        }

                        var neigh = result[neighPos.x, neighPos.y];
                        if (neigh.Dirs.Contains(dir.ToOpposite()))
                        {
                            cur.Neighb.Add(neigh);
                        }
                    }
                }
            }

            return result;
        }

        private Direction[] GetDirs(char symbol)
        {
            return symbol switch
            {
                '|' => new[]{Direction.Up,    Direction.Down},
                '-' => new[]{Direction.Left,  Direction.Right},
                'L' => new[]{Direction.Up,    Direction.Right},
                'J' => new[]{Direction.Up,    Direction.Left},
                '7' => new[]{Direction.Left,  Direction.Down},
                'F' => new[]{Direction.Right, Direction.Down},
                'S' => new[]{Direction.Up,    Direction.Down, Direction.Left, Direction.Right},
                _   => Array.Empty<Direction>()
            };
        }
        
        private Cell GetStart(List<string> inputList, Cell[,] grid)
        {
            var sizeX = inputList[0].Length;
            var sizeY = inputList.Count;
            
            for (var y = 0; y < sizeY; y++)
            {
                for (var x = 0; x < sizeX; x++)
                {
                    var symbol = inputList[y][x];

                    if (symbol == 'S')
                    {
                        return grid[x, y];
                    }
                }
            }

            throw new Exception("Start not found");
        }
    }
}
