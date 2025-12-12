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
        
        /*
        var ordered =
            from a in points
            from b in points
            where (a.x, a.y, a.z).CompareTo((b.x, b.y, b.z)) < 0
            orderby Vec3.Distance(a, b)
            select (a, b);

        foreach (var (a, b) in ordered.Take(1000))
        {
            if (workingSet[a] == workingSet[b])
                continue;

            workingSet[a].UnionWith(workingSet[b]);

            foreach (var bb in workingSet[b])
            {
                workingSet[bb] = workingSet[a];
            }
        }
        */
        
        foreach (var (a, b) in GetOrderedPairs(points).Take(1000))
        {
            /*if (setOf[a] != setOf[b])
            {
                Connect(a, b, setOf);
            }*/
            
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

    void Connect(Vec3 a, Vec3 b, Dictionary<Vec3, HashSet<Vec3>> setOf)
    {
        setOf[a].UnionWith(setOf[b]);
        
        foreach (var p in setOf[b])
        {
            setOf[p] = setOf[a];
        }
    }

    IEnumerable<(Vec3 a, Vec3 b)> GetOrderedPairs(List<Vec3> points) =>
        from a in points
        from b in points
        where (a.x, a.y, a.z).CompareTo((b.x, b.y, b.z)) < 0
        orderby Vec3.DistanceSq(a,b)
        select (a, b);
}