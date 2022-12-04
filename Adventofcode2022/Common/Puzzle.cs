using System.Collections.Generic;

namespace Adventofcode2022.Common
{
    public abstract class Puzzle
    {
        public string Name => GetType().Name;

        public abstract string[] GetResults(IReadOnlyList<string> input);
    }
}
