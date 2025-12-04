using AdventOfCode.Common;

namespace AdventOfCode.Year2025;

public class Puzzle04 : Puzzle
{
    public override string[] GetResults(IReadOnlyList<string> input)
    {
        var size = new Vec2(input[0].Length, input.Count);
        var grid = new Dictionary<Vec2, char>();
        
        for (var y = 0; y < size.y; y++)
        {
            for (var x = 0; x < size.x; x++)
            {
                grid[new Vec2(x, y)] = input[y][x];
            }
        }
        
        return new[]
        {
            Part1(size, grid).ToString(),
            Part2(size, grid).ToString(),
        };
    }

    private int Part2(Vec2 size, Dictionary<Vec2, char> grid)
    {
        var result = 0;
        
        while (true)
        {
            var list = DoThings(size, grid).ToList();

            if (list.Count == 0)
                return result;

            result += list.Count;
            
            foreach (var vec2 in list)
                grid[vec2] = '.';
        }
    }

    private object Part1(Vec2 size, Dictionary<Vec2, char> grid)
    {
        return DoThings(size, grid).Count();
    }

    private IEnumerable<Vec2> DoThings(Vec2 size, Dictionary<Vec2, char> grid)
    {
        var adjacent = Vec2.Adjacent;
        
        for (var y = 0; y < size.y; y++)
        {
            for (var x = 0; x < size.x; x++)
            {
                var pos = new Vec2(x, y);
                
                if (grid[pos] != '@')
                    continue;
                    
                var result = adjacent
                            .Select(offset =>
                             {
                                 return (grid.TryGetValue(pos + offset, out var cur) && cur == '@')
                                     ? 1
                                     : 0;
                             })
                            .Sum();

                if (result >= 4)
                    continue;
                
                yield return pos;
            }
        }
    }
}