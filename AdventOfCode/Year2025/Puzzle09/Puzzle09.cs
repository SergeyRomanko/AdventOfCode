using AdventOfCode.Common;

namespace AdventOfCode.Year2025;

public class Puzzle09 : Puzzle
{
    
    public override string[] GetResults(IReadOnlyList<string> input)
    {
        var input1 = input
                    .Select(x =>
                     {
                         var data = x.Split(',');
                         
                         return new Vec2Long(long.Parse(data[0]), long.Parse(data[^1]));
                     })
                    .ToList();
            
        return new[]
        {
            Part1(input1).ToString(),
        };
    }

    private decimal Part1(List<Vec2Long> input)
    {
        var pairs  = ListPairs(input);
        
        var first = pairs.Last();
        
        return Area(first);
    }

    private IEnumerable<(Vec2Long, Vec2Long)> ListPairs(List<Vec2Long> input)
    {
        return from a in input
               from b in input
               where (a.x, a.y).CompareTo((b.x, b.y)) < 0
               orderby Area2((a, b))
               select (a, b);
    }

    private decimal Area((Vec2Long, Vec2Long) first)
        => (first.Item2.x - first.Item1.x + 1) * (first.Item2.y - first.Item1.y + 1);
    
    private decimal Area2((Vec2Long, Vec2Long) first)
        //=> (first.Item2.x - first.Item1.x + 1) * (first.Item2.y - first.Item1.y + 1);
        => Vec2Long.DistanceSq(first.Item1, first.Item2);
}