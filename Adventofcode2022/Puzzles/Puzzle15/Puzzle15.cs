using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle15 : Puzzle
    {
        public class Vec : IComparable<Vec>, IEquatable<Val>
        {
            public int X;
            public int Y;

            public int CompareTo(Vec? other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                var xComparison = X.CompareTo(other.X);
                if (xComparison != 0) return xComparison;
                return Y.CompareTo(other.Y);
            }

            protected bool Equals(Vec other)
            {
                return X == other.X && Y == other.Y;
            }

            public bool Equals(Val? other)
            {
                throw new NotImplementedException();
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Vec)obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var data =
                input.Select(x => x.Replace("Sensor at ", "").Replace(" closest beacon is at ", ""))
                    .Select(x => x.Split(":"))
                    .Select(x => (ReadVec(x[0]), ReadVec(x[1])))
                    .ToList();

            var ss = data.Select(x => x.Item1).ToList();
            var bb = data.Select(x => x.Item2).ToList();

            var data1 = data
                .SelectMany(x => Brutforce(2000000, x.Item1, x.Item2))
                .Where(x => !ss.Contains(x))
                .Where(x => !bb.Contains(x))
                .Distinct()
                .ToList();

            return new[]
            {
                data1.Count.ToString(),
                Puzzle15_2.GetResult(data, 4000000).ToString()
            };
        }

        private IEnumerable<Vec> Brutforce(int lineY, Vec s, Vec b)
        {
            var bDist = Math.Abs(b.X - s.X) + Math.Abs(b.Y - s.Y);
            var lDist = Math.Abs(lineY - s.Y);
            var xDist = bDist - lDist;

            for (var x = -xDist; x <= xDist; x++)
            {
                yield return new Vec { X = s.X + x, Y = lineY };
            }
        }

        private Vec ReadVec(string input)
        {
            var coords = input.Split(", ");
            var x = coords[0].Replace("x=", "");
            var y = coords[1].Replace("y=", "");

            return new Vec()
            {
                X = int.Parse(x),
                Y = int.Parse(y),
            };
        }
    }
}
