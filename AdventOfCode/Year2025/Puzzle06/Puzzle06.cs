using AdventOfCode.Common;

namespace AdventOfCode.Year2025;

public class Puzzle06 : Puzzle
{
    public override string[] GetResults(IReadOnlyList<string> input)
    {
        return new[]
        {
            Part1(input).Sum().ToString(),
            Part2(input).Sum().ToString(),
        };
    }

    private IEnumerable<long> Part1(IReadOnlyList<string> input)
    {
        var rawData = input
                     .SkipLast(1)
                     .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList())
                     .ToList();
        
        var (sizeX, sizeY) =  (rawData[0].Count, rawData.Count);

        var data = Enumerable.Range(0, sizeX)
                             .SelectMany(x => Enumerable.Range(0, sizeY).Select(y => new Vec2(x, y)))
                             .ToDictionary(
                                  x => x, 
                                  x => rawData[x.y][x.x]
                              );
        
        var math = input
                  .Last()
                  .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                  .ToList();
        
        for (var i = 0; i < math.Count; i++)
        {
            var index = i;
            var sign = math[i];
            
            yield return data.Where(x => x.Key.x == index)
                             .Select(x => x.Value)
                             .Aggregate((x, y) =>
                              {
                                  return sign switch
                                  {
                                      "+" => x + y,
                                      "*" => x * y,
                                  };
                              });
        }
    }
    
    private IEnumerable<long> Part2(IReadOnlyList<string> input)
    {
        return GetData(input)
              .Select(data => data.Item2.Aggregate((x, y) =>
                   {
                       return data.Item1 switch
                       {
                           '+' => x + y,
                           '*' => x * y,
                       };
                   })
               );
    }

    private IEnumerable<(char, List<long>)> GetData(IReadOnlyList<string> input)
    {
        var (sizeX, sizeY) = (input[0].Length, input.Count);

        var lines = Enumerable.Range(0, sizeX)
                              .Select(x =>
                               {
                                   return new string(Enumerable.Range(0, sizeY)
                                                               .Select(y => input[y][x])
                                                               .ToArray()
                                                                );
                               })
                              .ToList();

        var data = (' ', new List<long>());

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                yield return data;
                data = (' ', new List<long>());
                continue;
            }

            var last = line.Last();
            if (last != ' ')
                data.Item1 = last;

            var numberString = new string(line.SkipLast(1).ToArray()).Trim();
            
            data.Item2.Add(
                long.Parse(numberString)
            );
        }
        
        yield return data;
    }
}