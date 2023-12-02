namespace AdventOfCode.Common
{
    public abstract class Puzzle
    {
        public string Name  => Path.Combine(GetType().FullName.Split(".").TakeLast(2).ToArray());
        public Debug  Debug { get; }

        protected Puzzle()
        {
            Debug = new Debug(Name);
        }
        
        public abstract string[] GetResults(IReadOnlyList<string> input);
    }
}
