using AdventOfCode.Common;

namespace AdventOfCode.Year2025;

public class Puzzle03 : Puzzle
{
    public override string[] GetResults(IReadOnlyList<string> input)
    {
        return new[]
        {
            DoThings(input,  2).Sum().ToString(),
            DoThings(input, 12).Sum().ToString(),
        };
    }

    private IEnumerable<long> DoThings(IReadOnlyList<string> input, int digits)
    {
        var result = new List<char>();
        
        foreach (var line in input)
        {
            var start = 0;
            
            for (var i = digits - 1; i >= 0; i--)
            {
                var span = line.AsSpan(start, line.Length - i - start);

                var maxChar = Max(span);
                
                result.Add(maxChar);

                start += span.IndexOf(maxChar) + 1;
            }
            
            yield return long.Parse(result.ToArray());
            
            result.Clear();
        }
    }

    private char Max(ReadOnlySpan<char> span)
    {
        if (span.IsEmpty)
            throw new InvalidOperationException("Span is empty");

        var max   = span[0];
        
        for (int i = 1; i < span.Length; i++)
        {
            if (span[i] > max)
                max = span[i];
        }
        
        return max;
    }
}