using AdventOfCode.Common;

namespace AdventOfCode.Year2024;

public class Puzzle02 : Puzzle
{
    public override string[] GetResults(IReadOnlyList<string> input)
    {
        var list = input.Select(x =>
        {
            return x.Split(' ').Select(int.Parse).ToList();
        }).ToList();
            
        return new[]
        {
            list.Count(IsSafe1).ToString(),
            list.Count(IsSafe2).ToString(),
        };
    }
    
    private bool IsSafe2(List<int> input)
    {
        for (var i = 0; i < input.Count; i++)
        {
            var tmpList = input.Where((_, index) => i != index).ToList();

            if (IsSafe1(tmpList)) return true;
        }
        
        return false;
    }

    private bool IsSafe1(List<int> input)
    {
        var pairs = input.Zip(input.Skip(1)).ToList();
        
        if(!pairs.Select(x => Math.Abs(x.First - x.Second)).All(x => x is >= 1 and <= 3))
            return false;
        
        var signSum = pairs.Select(x => Math.Sign(x.First - x.Second)).Sum();
        
        return Math.Abs(signSum) == input.Count - 1;
    }
}

//https://github.com/encse/adventofcode/tree/master/2024