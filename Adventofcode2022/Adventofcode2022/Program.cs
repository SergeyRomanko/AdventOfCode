using System;
using System.Collections.Generic;
using System.IO;
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
        };
        
        public static void Main(string[] args)
        {
            var text = ReadText();

            foreach (var puzzle in Puzzles)
            {
                Console.WriteLine($"{puzzle.Name}: {string.Join(", ", puzzle.GetResults(text))}");
            }
        }

        private static IEnumerable<string> ReadText()
        {
            DirectoryInfo d = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            FileInfo[] Files = d.GetFiles("*.txt", SearchOption.AllDirectories);
            var file = Files.FirstOrDefault(x => x.Name.Contains("Puzzle01.txt"));

            return File.ReadLines(file.FullName);
        }
    }
}
