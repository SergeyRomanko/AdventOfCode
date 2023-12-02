using AdventOfCode.Common;

namespace AdventOfCode.Year2022
{
    public class Puzzle23Elf
    {
        private static readonly Vec2 N = new Vec2(0, -1);
        private static readonly Vec2 S = new Vec2(0, +1);
        private static readonly Vec2 W = new Vec2(-1, 0);
        private static readonly Vec2 E = new Vec2(+1, 0);
        
        private static readonly Vec2 NE = new Vec2(+1, -1);
        private static readonly Vec2 NW = new Vec2(-1, -1);
        private static readonly Vec2 SE = new Vec2(+1, +1);
        private static readonly Vec2 SW = new Vec2(-1, +1);
        
        private readonly List<Vec2[]> Dirs = new List<Vec2[]>
        {
            new Vec2[]{ NE, N, NW },
            new Vec2[]{ SE, S, SW },
            new Vec2[]{ NW, W, SW },
            new Vec2[]{ NE, E, SE },
        };

        public int Id { get; private set; }

        public Puzzle23Elf(int id)
        {
            Id = id;
        }

        public void Prepare(Vec2 pos, Dictionary<Vec2, Puzzle23Elf?> commands, Puzzle23Container container)
        {
            if (!TryGetNextPos(pos, container, out var next))
            {
                return;
            }

            if (commands.ContainsKey(next))
            {
                commands[next] = null;
            }
            else
            {
                commands[next] = this;
            }
        }

        private bool TryGetNextPos(Vec2 pos, Puzzle23Container container, out Vec2 next)
        {
            next = default;
            
            var isValid = true;
            var result = default(Vec2[]);
            
            foreach (var dir in Dirs)
            {
                var isFree = container.IsFree(pos + dir[0]) && 
                             container.IsFree(pos + dir[1]) && 
                             container.IsFree(pos + dir[2]);

                if (isFree)
                {
                    result ??= dir;
                }
                else
                {
                    isValid = false;
                }
            }

            var first = Dirs[0];
            Dirs.Remove(first);
            Dirs.Add(first);

            if (isValid)
            {
                return false;
            }
            
            next = result != null ? pos + result[1] : default;
            
            return result != null;
        }
    }
}
