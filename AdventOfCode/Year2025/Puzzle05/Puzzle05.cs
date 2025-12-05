using AdventOfCode.Common;

namespace AdventOfCode.Year2025;

public class Puzzle05 : Puzzle
{
    public override string[] GetResults(IReadOnlyList<string> input)
    {
        var ranges = input
                    .Where(x => x.Contains('-'))
                    .Select(x =>
                     {
                         var split = x.Split('-');
                         return (long.Parse(split[0]), long.Parse(split[1]));
                     })
                    .ToList();
        
        var values = input
                    .Where(x => !x.Contains('-') && !string.IsNullOrEmpty(x))
                    .Select(long.Parse)
                    .ToList();
            
        return new[]
        {
            Part1(ranges, values).Count()
                                 .ToString(),
            
            Part2(ranges).Select(x => x.Item2 - x.Item1 + 1)
                         .Sum()
                         .ToString(),
        };
    }


    private IEnumerable<long> Part1(List<(long,long)> ranges, List<long> values)
    {
        return values.Where(value =>
        {
            return ranges.Any(range => value >= range.Item1 && value <= range.Item2);
        });
    }
    
    private IEnumerable<(long, long)> Part2(List<(long, long)> ranges)
    {
        var ordered = ranges
                     .OrderBy(range => range.Item1)
                     .ToList();

        (long, long)? current = null;
        
        foreach (var value in ordered)
        {
            if (current == null)
            {
                current = value;
                continue;
            }

            if (current.Value.Item2 >= value.Item1)
            {
                current = (current.Value.Item1, Math.Max(current.Value.Item2, value.Item2));
            }
            else
            {
                yield return current.Value;
                current = value;
            }
        }

        if (current != null)
            yield return current.Value;
    }
}