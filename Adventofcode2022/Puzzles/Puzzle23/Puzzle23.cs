using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public sealed class Puzzle23 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            return new[]
            {
                DoTask1(CreateContainer(input)).ToString(),
                DoTask2(CreateContainer(input)).ToString(),
            };
        }

        private int DoTask1(Puzzle23Container container)
        {
            for (int i = 0; i < 10; i++)
            {
                DoThings(container);
            }

            return FormatResult(container);
        }
        
        private int DoTask2(Puzzle23Container container)
        {
            var result = 0;

            while (DoThings(container))
            {
                result++;
            }
            
            return result + 1;
        }

        private bool DoThings(Puzzle23Container container)
        {
            var commands = new Dictionary<Vec2, Puzzle23Elf?>(1024);

            foreach (var (elf, pos) in container.ElfToPos)
            {
                elf.Prepare(pos, commands, container);
            }

            if (commands.Count == 0)
            {
                return false;
            }

            foreach (var (pos, elf) in commands.Where(x => x.Value != null))
            {
                container.Move(elf, pos);
            }

            return true;
        }

        private int FormatResult(Puzzle23Container container)
        {
            var minX = container.ElfToPos.Select(x => x.Value).Min(x => x.x);
            var minY = container.ElfToPos.Select(x => x.Value).Min(x => x.y);
            var maxX = container.ElfToPos.Select(x => x.Value).Max(x => x.x);
            var maxY = container.ElfToPos.Select(x => x.Value).Max(x => x.y);

            var dx = (maxX - minX) + 1;
            var dy = (maxY - minY) + 1;

            var area = dx * dy;

            return area - container.ElfToPos.Count;
        }

        private Puzzle23Container CreateContainer(IReadOnlyList<string> input)
        {
            var id = 0;
            var result = new Puzzle23Container();
            
            for (var y = 0; y < input.Count; y++)
            {
                for (var x = 0; x < input[0].Length; x++)
                {
                    if (input[y][x] != '#')
                    {
                        continue;
                    }
                    
                    result.AddElf(new Puzzle23Elf(id++), new Vec2(x,y));
                }
            }
            
            return result;
        }
    }
}
