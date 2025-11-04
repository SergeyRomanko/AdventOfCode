using AdventOfCode.Common;

namespace AdventOfCode.Year2024;

public class Puzzle04 : Puzzle
{
    private struct Helper
    {
        public Vec2 Pos;
        public Vec2 Dir;
    }
    
    private readonly List<Vec2> _dirs = new()
    {
        new Vec2(-1, -1),
        new Vec2(-1,  0),
        new Vec2(-1, +1),
        new Vec2( 0, -1),
        new Vec2( 0, +1),
        new Vec2(+1, -1),
        new Vec2(+1,  0),
        new Vec2(+1, +1),
    };
    
    public override string[] GetResults(IReadOnlyList<string> input)
    {
        var data = new Dictionary<Vec2, char>();
        
        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                var pos = new Vec2(x, y);

                data[pos] = input[y][x];
            }
        }

        return new[]
        {
            Part1(data).Sum().ToString(),
            Part2(data).Sum().ToString(),
        };
    }
    
    private IEnumerable<int> Part1(Dictionary<Vec2, char> data)
    {
        var tasks = new Queue<Helper>();

        //Создаем начальные таски 
        foreach (var (pos, value) in data)
        {
            if (value != 'X')
                continue;
            
            foreach (var vec2 in _dirs)
            {
                tasks.Enqueue(new Helper
                {
                    Pos = pos,
                    Dir = vec2
                });
            }
        }

        var phrase = "XMAS";

        while (tasks.TryDequeue(out var it))
        {
            var value = data[it.Pos];
            var index = phrase.IndexOf(value);

            //Нашли полную фразу
            if (index == phrase.Length - 1)
            {
                yield return 1;
                continue;
            }

            value = phrase[index + 1];
            
            var newPosition = it.Pos + it.Dir;

            if (!data.TryGetValue(newPosition, out var newValue))
                continue;

            if (value == newValue)
            {
                tasks.Enqueue(it with
                {
                    Pos = newPosition
                });
            }
        }
    }

    private IEnumerable<int> Part2(Dictionary<Vec2, char> data)
    {
        var tasks = new Queue<Vec2>();

        //Создаем начальные таски 
        foreach (var (pos, value) in data)
        {
            if (value == 'X')
                tasks.Enqueue(pos);
        }

        var phrase = "XMAS";

        while (tasks.TryDequeue(out var it))
        {
            var value = data[it];
            var index = phrase.IndexOf(value);

            //Нашли полную фразу
            if (index == phrase.Length - 1)
            {
                yield return 1;
                continue;
            }

            value = phrase[index + 1];
            
            foreach (var vec2 in _dirs)
            {
                var newPosition = vec2 + it;

                if (!data.TryGetValue(newPosition, out var newValue))
                    continue;
                
                if (value == newValue)
                    tasks.Enqueue(newPosition);
            }
        }
    }
}