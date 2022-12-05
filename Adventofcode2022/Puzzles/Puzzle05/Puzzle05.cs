using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle05 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var stacks1 = BuildStacks(
                input.Where(x => x.Contains('[')),
                input.First(x => x.StartsWith(" 1")));
            
            var stacks2 = BuildStacks(
                input.Where(x => x.Contains('[')),
                input.First(x => x.StartsWith(" 1")));
            
            var moves = BuildMoves(input.Where(x => x.StartsWith("move")));

            return new[]
            {
                string.Join("", DoThings1(stacks1, moves).Select(x => x.Pop())),
                string.Join("", DoThings2(stacks2, moves).Select(x => x.Pop())),
            };
        }

        private IReadOnlyList<Stack<char>> DoThings1(IReadOnlyList<Stack<char>> stacks, IReadOnlyList<(int, int, int)> moves)
        {
            foreach (var (move, form, to) in moves)
            {
                for (var i = 0; i < move; i++)
                {
                    stacks[to].Push(stacks[form].Pop());
                }
            }

            return stacks;
        }
        
        private IReadOnlyList<Stack<char>> DoThings2(IReadOnlyList<Stack<char>> stacks, IReadOnlyList<(int, int, int)> moves)
        {
            var util = new Stack<char>();
            
            foreach (var (move, form, to) in moves)
            {
                for (var i = 0; i < move; i++)
                {
                    util.Push(stacks[form].Pop());
                }
                
                for (var i = 0; i < move; i++)
                {
                    stacks[to].Push(util.Pop());
                }
            }

            return stacks;
        }

        private IReadOnlyList<Stack<char>> BuildStacks(IEnumerable<string> crates, string indexes)
        {
            var stacksCount = indexes
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .Max(x => x);

            var result = Enumerable
                .Range(0, stacksCount)
                .Select(x => new Stack<char>())
                .ToList();
            
            foreach (var line in crates.Reverse())
            {
                var items = Enumerable
                    .Range(0, stacksCount)
                    .Select(x => line.Substring(x * 4, 3)[1])
                    .ToList();

                for (var i = 0; i < items.Count; i++)
                {
                    if (items[i] != ' ')
                    {
                        result[i].Push(items[i]);
                    }
                }
            }

            return result;
        }
        
        private IReadOnlyList<(int, int, int)> BuildMoves(IEnumerable<string> moves)
        {
            return moves
                .Select(x => x.Split(' '))
                .Select(x => 
                (
                    int.Parse(x[1]),
                    int.Parse(x[3]) - 1,
                    int.Parse(x[5]) - 1
                ))
                .ToList();
        }
    }
}
