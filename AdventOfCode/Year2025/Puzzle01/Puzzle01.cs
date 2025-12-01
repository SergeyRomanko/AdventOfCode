using AdventOfCode.Common;

namespace AdventOfCode.Year2025;

public class Puzzle01 : Puzzle
{
    public override string[] GetResults(IReadOnlyList<string> input)
    {
        var input1 = input
                    .Select(x => (x.First(), int.Parse(x[1..])))
                    .ToList();
            
        return new[]
        {
            Part1(input1).ToString(),
            Part2(input1).ToString(),
        };
    }

    private int Part1(List<(char, int)> input)
    {
        var number = 50;
        var result = 0;
        
        foreach (var (dir, value) in input)
        {
            if (dir == 'R')
                number += value;
            else
                number -= value;

            if (number % 100 == 0)
                result++;
        }
        
        return result;
    }
    
    private int Part2(List<(char, int)> input)
    {
        var number  = 50;
        var counter = 0;
        var result  = 0;
        
        foreach (var (dir, value) in input)
        {
            var old = number;

            if (dir == 'R')
                number += value;
            else
                number -= value;

            foreach (var i in BetweenOrdered(old, number).Skip(1))
            {
                if (i % 100 == 0)
                    result++;
            }
        }
        
        return result;
    }
    
    public static IEnumerable<int> BetweenOrdered(int a, int b)
    {
        if (a <= b)
        {
            for (int i = a; i <= b; i++)
                yield return i;
        }
        else
        {
            for (int i = a; i >= b; i--)
                yield return i;
        }
    }
}