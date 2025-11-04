using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Year2024;

public class Puzzle03 : Puzzle
{
    public override string[] GetResults(IReadOnlyList<string> input)
    {
        var lines = string.Join("", input);
            
        return new[]
        {
            Part1(lines).ToString(),
            Part2(lines).ToString(),
        };
    }

    private int Part1(string lines)
    {
        var result = 0;
        
        var pattern1 = @"mul\(\d{0,3},\d{0,3}\)";

        foreach (Match match in Regex.Matches(lines, pattern1))
        {
            result += ProcessMull(match);
        }
        
        return result;
    }
    
    private int Part2(string lines)
    {
        var result = 0;
        
        var pattern1 = @"mul\(\d{0,3},\d{0,3}\)";
        var pattern2 = @"do\(\)";
        var pattern3 = @"don\'t\(\)";
        
        var allMatches = Regex.Matches(lines, pattern1)
                              .Union(Regex.Matches(lines, pattern2))
                              .Union(Regex.Matches(lines, pattern3));

        var sortedMatches = allMatches.OrderBy(x => x.Index).ToList();

        var doFlag = true;
        
        foreach (var match in sortedMatches)
        {
            switch (match.Value)
            {
                case "do()":
                    doFlag = true;
                    break;
                
                case "don't()":
                    doFlag = false;
                    break;
                
                default:
                    
                    if (doFlag)
                        result += ProcessMull(match);
                    
                    break;
            }
        }
        
        return result;
    }

    private int ProcessMull(Match match)
    {
        var pattern2 = @"\d+";

        var matches  = Regex.Matches(match.Value, pattern2);
        var first    = matches.First();
        var last     = matches.Last();
        var firstInt = Convert.ToInt32(first.Value);
        var lastInt  = Convert.ToInt32(last.Value);
                
        return firstInt * lastInt;
    }
}