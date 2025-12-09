using AdventOfCode.Common;

namespace AdventOfCode.Year2025;

public class Puzzle07 : Puzzle
{
    public override string[] GetResults(IReadOnlyList<string> input)
    {
        var (sizeX, sizeY) = (input[0].Length, input.Count);
        
        
        var input1 = Enumerable.Range(0, sizeX)
                               .SelectMany(x => Enumerable.Range(0, sizeY).Select(y => new Vec2(x, y)))
                               .ToDictionary(x => x, x => input[x.y][x.x]);
            
        return new[]
        {
            Part1(input1, (sizeX, sizeY)).Sum().ToString(),
        };
    }

    private IEnumerable<int> Part1(Dictionary<Vec2, char> input, (int x, int y) size)
    {
        var clone = new Dictionary<Vec2, char>(input);

        for (var y = 0; y < size.y; y++)
        {
            for (var x = 0; x < size.x; x++)
            {
                if (!clone.TryGetValue(new Vec2(x, y - 1), out var prevValue))
                    continue;
                
                if (prevValue is '.' or '^')
                    continue;

                var current = clone[new Vec2(x, y)];

                if (current == '.')
                {
                    clone[new Vec2(x, y)] = '|';
                }
                else if (current == '^')
                {
                    if (clone.ContainsKey(new Vec2(x - 1, y)))
                        clone[new Vec2(x - 1, y)] = '|';
                        
                    if (clone.ContainsKey(new Vec2(x + 1, y)))
                        clone[new Vec2(x + 1, y)] = '|';
                    
                    yield return 1;
                }
            }
        }
    }
}