using AdventOfCode.Common;

namespace AdventOfCode.Year2025;

public class Puzzle08 : Puzzle
{
    public override string[] GetResults(IReadOnlyList<string> input)
    {
        var points = input
                    .Select(x =>
                     {
                         var data = x.Split(',')
                                     .Select(int.Parse)
                                     .ToArray();
                         
                         return new Vec3(data[0], data[1], data[2]);
                     })
                    .ToList();
        
        return new[]
        {
            Part1(points).ToString(),
        };
    }

    private long Part1(List<Vec3> points)
    {
        var setOf = points.ToDictionary(
            x => x,
            x => new HashSet<Vec3> { x }
        );
        
        foreach (var (a, b) in GetOrderedPairs(points).Take(1000))
        {
            if (setOf[a] == setOf[b])
                continue;

            setOf[a].UnionWith(setOf[b]);

            foreach (var bb in setOf[b])
            {
                setOf[bb] = setOf[a];
            }
        }
        
        return setOf.Values.Distinct()
                    .OrderByDescending(set => set.Count)
                    .Take(3)
                    .Aggregate(1, (a, b) => a * b.Count);
    }
    
    IEnumerable<(Vec3 a, Vec3 b)> GetOrderedPairs(List<Vec3> points) =>
        from a in points
        from b in points
        where (a.x, a.y, a.z).CompareTo((b.x, b.y, b.z)) < 0
        orderby Vec3.DistanceSq(a,b)
        select (a, b);
}