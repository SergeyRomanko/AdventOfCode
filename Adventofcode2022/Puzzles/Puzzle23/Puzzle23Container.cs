using System.Collections.Generic;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public sealed class Puzzle23Container
    {
        private Dictionary<Puzzle23Elf, Vec2> _elfToPos = new Dictionary<Puzzle23Elf, Vec2>();
        private Dictionary<Vec2, Puzzle23Elf> _posToElf = new Dictionary<Vec2, Puzzle23Elf>();

        public IReadOnlyDictionary<Puzzle23Elf, Vec2> ElfToPos => _elfToPos;

        public void AddElf(Puzzle23Elf elf, Vec2 pos)
        {
            _elfToPos[elf] = pos;
            _posToElf[pos] = elf;
        }

        public void Move(Puzzle23Elf elf, Vec2 newPos)
        {
            var oldPos = _elfToPos[elf];

            _posToElf.Remove(oldPos);

            AddElf(elf, newPos);
        }

        public bool IsFree(Vec2 pos)
        {
            return !_posToElf.ContainsKey(pos);
        }
    }
}
