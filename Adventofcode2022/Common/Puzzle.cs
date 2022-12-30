using System.Collections.Generic;

namespace Adventofcode2022.Common
{
    public abstract class Puzzle
    {
        public string Name => GetType().Name;
        public Debug Debug { get; }

        protected Puzzle()
        {
            Debug = new Debug(Name);
        }
        
        public abstract string[] GetResults(IReadOnlyList<string> input);
    }
}
