using AdventOfCode.Common;

namespace AdventOfCode
{
    internal class Program
    {
        private static readonly Puzzle[] Year2022Puzzles = 
        {
            new Year2022.Puzzle01(),
            new Year2022.Puzzle02(),
            new Year2022.Puzzle03(),
            new Year2022.Puzzle04(),
            new Year2022.Puzzle05(),
            new Year2022.Puzzle06(),
            new Year2022.Puzzle07(),
            new Year2022.Puzzle08(),
            new Year2022.Puzzle09(),
            new Year2022.Puzzle10(),
            new Year2022.Puzzle11(),
            new Year2022.Puzzle12(),
            new Year2022.Puzzle13(),
            //new Year2022.Puzzle14(),
            //new Year2022.Puzzle15(),
            //new Year2022.Puzzle16(),
            new Year2022.Puzzle17(),
            new Year2022.Puzzle18(),
            //new Year2022.Puzzle19(),
            new Year2022.Puzzle20(),
            new Year2022.Puzzle21(),
            new Year2022.Puzzle22(),
            //new Year2022.Puzzle23(),
            //new Year2022.Puzzle24(),
            new Year2022.Puzzle25(),
        };
        
        private static readonly Puzzle[] Year2023Puzzles = 
        {
            new Year2023.Puzzle01(),
            new Year2023.Puzzle02(),
            new Year2023.Puzzle03(),
            new Year2023.Puzzle04(),
            new Year2023.Puzzle05(),
            new Year2023.Puzzle06(),
            new Year2023.Puzzle07(),
            new Year2023.Puzzle08(),
            new Year2023.Puzzle09(),
            //new Year2023.Puzzle10(),
            new Year2023.Puzzle11(),
            //new Year2023.Puzzle12(),
            new Year2023.Puzzle15(),
        };
        
        public static void Main(string[] args)
        {
            foreach (var puzzle in Year2023Puzzles)
            {
                var input = Inputs.ReadText(puzzle.Name).ToList();
                
                var results = puzzle.GetResults(input);
                
                Console.WriteLine($"{puzzle.Name}: {string.Join(", ", results)}");
            }

            Console.ReadLine();
        }
    }
}
