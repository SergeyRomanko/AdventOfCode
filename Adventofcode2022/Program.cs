using System;
using System.Linq;
using Adventofcode2022.Common;
using Adventofcode2022.Puzzles;

namespace Adventofcode2022
{
    internal class Program
    {
        private static readonly Puzzle[] Puzzles = 
        {
            new Puzzle01(),
            new Puzzle02(),
            new Puzzle03(),
            new Puzzle04(),
            new Puzzle05(),
            new Puzzle06(),
            new Puzzle07(),
            new Puzzle08(),
            new Puzzle09(),
            new Puzzle10(),
            new Puzzle11(),
            new Puzzle12(),
            new Puzzle13(),
            //new Puzzle14(),
            //new Puzzle15(),
            new Puzzle16(),
            new Puzzle17(),
            new Puzzle18(),
            new Puzzle19(),
            new Puzzle20(),
            new Puzzle21(),
        };
        
        public static void Main(string[] args)
        {
            foreach (var puzzle in Puzzles)
            {
                var input = Inputs.ReadText(puzzle.Name).ToList();
                
                var results = puzzle.GetResults(input);
                
                Console.WriteLine($"{puzzle.Name}: {string.Join(", ", results)}");
            }
        }
    }
}
