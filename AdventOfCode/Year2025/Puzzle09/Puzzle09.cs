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
            Part2(input1).ToString(),
        };
    }

    private decimal Part1(List<Vec2Long> input)
    {
        var pairs = ListPairs(input);
        
        var first = pairs.First();
        
        return Area(first);
    }
    
    private decimal Part2(List<Vec2Long> input)
    {
        var pairs = ListPairs(input);
        
        var segments = ListSegments(input);
        
        var first = pairs
           .First(x => segments.All(y => !AabbCollision(x, y)));
        
        return Area(first);
    }

    private bool AabbCollision((Vec2Long, Vec2Long) a, (Vec2Long, Vec2Long) b)
    {
        var (aLeft, aRight, aBottom, aTop) = (a.Item1.x, a.Item2.x, a.Item1.y, a.Item2.y);
        var (bLeft, bRight, bBottom, bTop) = (b.Item1.x, b.Item2.x, b.Item1.y, b.Item2.y);

        // Using <= and >= means if they only touch at the edges, 
        // they are considered "separated" and thus NOT colliding.
        var separatedX = aRight <= bLeft   || aLeft   >= bRight;
        var separatedY = aTop   <= bBottom || aBottom >= bTop;
        
        return !(separatedX || separatedY);
    }

    private List<(Vec2Long, Vec2Long)> ListSegments(List<Vec2Long> input)
    {
        return input.Zip(
            input.Prepend(input.Last()), 
            CreateValidRect
        ).ToList();
    }

    private IEnumerable<(Vec2Long, Vec2Long)> ListPairs(List<Vec2Long> input)
    {
        return from a in input
               from b in input
               orderby Area(CreateValidRect(a, b)) descending 
               select CreateValidRect(a, b);
    }

    private decimal Area((Vec2Long, Vec2Long) first)
        => (first.Item2.x - first.Item1.x + 1) * (first.Item2.y - first.Item1.y + 1);


    private (Vec2Long, Vec2Long) CreateValidRect(Vec2Long a, Vec2Long b)
    {
        var minX = Math.Min(a.x, b.x);
        var maxX = Math.Max(a.x, b.x);
        var minY = Math.Min(a.y, b.y);
        var maxY = Math.Max(a.y, b.y);
        
        return (new Vec2Long(minX, minY), new Vec2Long(maxX, maxY));
    }
}