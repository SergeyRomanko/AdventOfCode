using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle16 : Puzzle
    {
        private struct Beam
        {
            public Vec2      Pos;
            public Direction Dir;
            
            public Beam(Direction dir, Vec2 pos) 
            {
                Dir = dir;
                Pos = pos;
            }
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var data = ReadData(input);
            
            return new[]
            {
                Part1(data, new Beam(Direction.Right, Vec2.Zero)).ToString(),
                Part2(data).ToString()
            };
        }

        private int Part2(char[,] data)
        {
            var sizeX = data.GetLength(0);
            var sizeY = data.GetLength(1);

            var top    = Enumerable.Range(0, sizeX).Select(x => new Beam(Direction.Down,  new Vec2(x, 0)));
            var bottom = Enumerable.Range(0, sizeX).Select(x => new Beam(Direction.Up,    new Vec2(x, sizeY - 1)));
            var left   = Enumerable.Range(0, sizeY).Select(y => new Beam(Direction.Right, new Vec2(0, y)));
            var right  = Enumerable.Range(0, sizeY).Select(y => new Beam(Direction.Left,  new Vec2(sizeX - 1, y)));
            var all    = top.Concat(bottom).Concat(left).Concat(right);

            return all.Select(x => Part1(data, x)).Max();
        }

        private int Part1(char[,] data, Beam startBeam)
        {
            var sizeX = data.GetLength(0);
            var sizeY = data.GetLength(1);

            var visits = new char[sizeX, sizeY];
            var hash   = new HashSet<Beam>();
            var beams  = new List<Beam> { startBeam };

            while (beams.Count > 0)
            {
                beams = Step(data, visits, beams, hash).ToList();
            }
            
            return (
                from x in Enumerable.Range(0, sizeX)
                from y in Enumerable.Range(0, sizeY)
                select visits[x, y]
            ).Count(x => x != '\0');
        }

        private IEnumerable<Beam> Step(char[,] data, char[,] visits, List<Beam> beams, HashSet<Beam> hash)
        {
            foreach (var beam in beams)
            {
                foreach (var newBeam in BeamStep(data, visits, beam).Where(x => IsBeamValid(x, data, hash)))
                {
                    yield return newBeam;
                }
            }
        }

        private bool IsBeamValid(Beam beam, char[,] data, HashSet<Beam> hash)
        {
            if (!hash.Add(beam))
            {
                return false;
            }

            var sizeX = data.GetLength(0);
            var sizeY = data.GetLength(1);

            return 0 <= beam.Pos.x && beam.Pos.x < sizeX &&
                   0 <= beam.Pos.y && beam.Pos.y < sizeY;
        }

        private IEnumerable<Beam> BeamStep(char[,] data, char[,] visits, Beam beam)
        {
            visits[beam.Pos.x, beam.Pos.y] = '#';

            var sym = data[beam.Pos.x, beam.Pos.y];

            var dirs = beam.Dir switch
            {
                Direction.Up    when sym == '-'  => (Direction.Left, Direction.Right),
                Direction.Up    when sym == '/'  => (Direction.Right, Direction.None),
                Direction.Up    when sym == '\\' => (Direction.Left, Direction.None),
                
                Direction.Down  when sym == '-'  => (Direction.Left, Direction.Right),
                Direction.Down  when sym == '/'  => (Direction.Left, Direction.None),
                Direction.Down  when sym == '\\' => (Direction.Right, Direction.None),
                
                Direction.Left  when sym == '|'  => (Direction.Up, Direction.Down),
                Direction.Left  when sym == '/'  => (Direction.Down, Direction.None),
                Direction.Left  when sym == '\\' => (Direction.Up, Direction.None),
                    
                Direction.Right when sym == '|'  => (Direction.Up, Direction.Down),
                Direction.Right when sym == '/'  => (Direction.Up, Direction.None),
                Direction.Right when sym == '\\' => (Direction.Down, Direction.None),
                    
                _                                => (beam.Dir, Direction.None)
            };

            yield return new Beam(dirs.Item1, beam.Pos + dirs.Item1.ToVec());

            if (dirs.Item2 != Direction.None)
            {
                yield return new Beam(dirs.Item2, beam.Pos + dirs.Item2.ToVec());
            }
        }

        private char[,] ReadData(IReadOnlyList<string> input)
        {
            var inputList = input.ToList();
            var sizeX     = inputList[0].Length;
            var sizeY     = inputList.Count;

            var result = new char[sizeX, sizeY];
            
            for (var y = 0; y < sizeY; y++)
            {
                for (var x = 0; x < sizeX; x++)
                {
                    result[x, y] = inputList[y][x];
                }
            }
            
            return result;
        }
    }
}