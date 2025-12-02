using AdventOfCode.Common;

namespace AdventOfCode.Year2025;

public class Puzzle02 : Puzzle
{
    public override string[] GetResults(IReadOnlyList<string> input)
    {
        var input1 = input[0].Split(',')
                    .Select(x =>
                              {
                                  var split = x.Split('-');
                                  
                                  return (long.Parse(split.First()), long.Parse(split.Last()));
                              })
                    .ToList();
            
        return new[]
        {
            Part1(input1).Sum().ToString(),
            Part2(input1).Sum().ToString(),
        };
    }

    private IEnumerable<long> Part1(List<(long, long)> input)
    {
        foreach (var (from, to) in input)
        {
            for (var i = from; i <= to; i++)
            {
                if (Test(2, i.ToString()))
                    yield return i;
            }
        }
    }
    
    private IEnumerable<long> Part2(List<(long, long)> input)
    {
        foreach (var (from, to) in input)
        {
            for (var i = from; i <= to; i++)
            {
                var str = i.ToString();
                
                for (var z = 2; z <= str.Length; z++)
                {
                    if (Test(z, str))
                    {
                        yield return i;
                        
                        break;
                    }
                }
            }
        }
    }

    private bool Test(int subLength, string str)
    {
        if(str.Length % subLength != 0)
            return false;
                
        var partLength = str.Length / subLength;
        var parts = str.Length / partLength;
                
        var first  = str.AsSpan(0, partLength);
                
        for (var i = 1; i < parts; i++)
        {
            var cur = str.AsSpan(partLength * i, partLength);

            if (!first.SequenceEqual(cur))
                return false;
        }
        
        return true;
    }
}