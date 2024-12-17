using System.Text;
using AdventOfCode.Common;

namespace AdventOfCode.Year2024;

public class Puzzle01 : Puzzle
{
    private StringBuilder _sb      = new();
    private string[]      _numbers = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        
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
        var dataA = listA.OrderBy(x => x);
        var dataB = listB.OrderBy(x => x);
        
        foreach (var (a, b) in dataA.Zip(dataB))
        {
            yield return Math.Abs(a - b);
        }
    }
    
    private IEnumerable<int> Part2(List<int> listA, List<int> listB)
    {
        var dataA = listA.OrderBy(x => x);
        
        var dataB = listA.Distinct().ToDictionary(
            x => x, 
            x => listB.Count(y => y == x)
        );
        
        foreach (var a in dataA)
        {
            yield return a * dataB[a];
        }
    }

    private int GetNumbersFromString(string input)
    {
        var first = input.Where(char.IsNumber).First();
        var last  = input.Where(char.IsNumber).Last();

        return int.Parse($"{first}{last}");
    }
        
    private string PreprocessString(string input)
    {
        _sb.Clear();
            
        for (int i = 0; i < input.Length; i++)
        {
            _sb.Append(StringToNumberOrFirstChar(input.AsSpan()[i..]));
        }
            
        return _sb.ToString();
    }

    private char StringToNumberOrFirstChar(ReadOnlySpan<char> span)
    {
        for (var i = 0; i < _numbers.Length; i++)
        {
            if (span.StartsWith(_numbers[i]))
            {
                return (char)('1' + i);
            }
        }

        return span[0];
    }
}