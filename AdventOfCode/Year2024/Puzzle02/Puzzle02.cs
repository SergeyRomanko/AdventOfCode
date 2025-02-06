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
        var test = Part1Process(input).ToList();

        return test.All(x => x == 1) || test.All(x => x == -1);
    }
    
    private IEnumerable<int> Part1Process(List<int> input)
    {
        for (var i = 0; i < input.Count - 1; i++)
        {
            var delta = input[i + 1] - input[i];

            if (delta == 0 || Math.Abs(delta) > 3)
            {
                yield return 0;
            }
            else
            {
                yield return delta > 0 ? 1 : -1;
            }
        }
    }
}

//https://github.com/encse/adventofcode/tree/master/2024