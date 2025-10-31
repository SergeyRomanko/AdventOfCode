using AdventOfCode.Common;

namespace AdventOfCode.Year2024;

public class Puzzle01 : Puzzle
{
        
    public override string[] GetResults(IReadOnlyList<string> input)
    {
        var listA = input.Select(x => int.Parse(x.Split(' ').First())).ToList();
        var listB = input.Select(x => int.Parse(x.Split(' ').Last())).ToList();
            
        return new[]
        {
            Part1(listA, listB).Sum().ToString(),
            Part2(listA, listB).Sum().ToString(),
        };
    }

    private IEnumerable<int> Part1(List<int> listA, List<int> listB)
    {
        listA.Sort();
        listB.Sort();

        return listA.Zip(listB, (x, y) => Math.Abs(x - y));
    }
    
    private IEnumerable<int> Part2(List<int> listA, List<int> listB)
    {
        var data = listB.ToLookup(x => x);
        
        return listA.Select(x => data[x].Count() * x);
    }
}