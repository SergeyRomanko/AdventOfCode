using AdventOfCode.Common;

namespace AdventOfCode.Year2022
{
    public class Puzzle04 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            return new[]
            {
                input
                    .Select(x => x.Split(','))
                    .Select(x => (A: ToRange(x[0]), B: ToRange(x[1])))
                    .Count(x => IsFullyContains(x.A, x.B))
                    .ToString(),
                
                input
                    .Select(x => x.Split(','))
                    .Select(x => (A: ToRange(x[0]), B: ToRange(x[1])))
                    .Count(x => IsOverlapping(x.A, x.B))
                    .ToString(),
            };
        }
        
        private bool IsFullyContains(Range a, Range b)
        {
            var c = new Range(
                Math.Min(a.Start.Value, b.Start.Value),
                Math.Max(a.End.Value, b.End.Value));

            var aSize = a.End.Value - a.Start.Value;
            var bSize = b.End.Value - b.Start.Value;
            var cSize = c.End.Value - c.Start.Value;

            return cSize == aSize || cSize == bSize;
        }
        
        private bool IsOverlapping(Range a, Range b) 
            => Math.Max(a.Start.Value, b.Start.Value) <= Math.Min(a.End.Value, b.End.Value);

        private Range ToRange(string input)
        {
            var split = input.Split('-');
            
            return new Range(int.Parse(split[0]), int.Parse(split[1]));
        }
    }
}
