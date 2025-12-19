using AdventOfCode.Common;

namespace AdventOfCode.Year2025;

public class Puzzle10 : Puzzle
{
    public class Data
    {
        public List<int>       A { get; } = new();
        public List<List<int>> B { get; } = new();
        public List<int>       C { get; } = new();
    }
    
    public override string[] GetResults(IReadOnlyList<string> input)
    {
        var listA = input.Select(x =>
        {
            var result = new Data();
            
            foreach (var entry in x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                var content = entry.Substring(1, entry.Length - 2);
                
                switch (entry[0])
                {
                    case '[' :
                        result.A.AddRange(content.Select(y => y == '.' ? 0 : 1));
                        break;
                    
                    case '(' :
                        result.B.Add(content.Split(',').Select(int.Parse).ToList());
                        break;
                    
                    case '{' :
                        result.C.AddRange(content.Split(',').Select(int.Parse));
                        break;
                }
            }

            return result;
        }).ToList();
            
        return new[]
        {
            Part1(listA).Sum().ToString(),
        };
    }

    private IEnumerable<int> Part1(List<Data> listA)
    {
        foreach (var data in listA)
        {
            yield return -1;
        }
    }
}