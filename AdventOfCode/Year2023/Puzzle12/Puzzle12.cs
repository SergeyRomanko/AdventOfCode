using System.Collections.Concurrent;
using System.Diagnostics;
using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle12 : Puzzle
    {
        private ConcurrentBag<long> _part2Results;
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var inputList = input.ToList();

            
            _part2Results = new ConcurrentBag<long>();

            Parallel.ForEach(inputList, x => _part2Results.Add(Part2(x)) );
            
            return new[]
            {
                //inputList.Select(Part1).Sum().ToString(),
                //inputList.Select(Part2).Sum().ToString(),
                _part2Results.Sum().ToString(),
            };
        }

        private int Part1(string line)
        {
            var split   = line.Split(" ");
            var chars   = split[0];
            var numbers = split[1].Split(",").Select(int.Parse).ToList();
            
            return ListVariants(chars.ToArray(), numbers)
                .Count(variant => Validate(variant, numbers));
        }
        
        private long Part2(string line)
        {
            var split   = line.Split(" ");
            var chars   = string.Join("?", Enumerable.Range(0, 5).Select(x => split[0]));
            var numbers = string.Join(",", Enumerable.Range(0, 5).Select(x => split[1]))
                .Split(",")
                .Select(int.Parse)
                .ToList();
            
            var sw = new Stopwatch();

            sw.Start();
            
            var result = ListVariants(chars.ToArray(), numbers)
                .Count(variant => Validate(variant, numbers));

            Console.WriteLine($"[{_part2Results.Count:D3}] {line} Elapsed={sw.Elapsed}");

            sw.Stop();
            
            return result;
        }

        private IEnumerable<char[]> ListVariants(char[] data, List<int> numbers)
        {
            var pos = -1;
            
            for (var i = 0; i < data.Length; i++)
            {
                if (data[i] == '?')
                {
                    pos = i;
                    break;
                }
            }
            
            if (pos < 0)
            {
                yield return data;
                yield break;
            }

            if (pos > 0)
            {
                if (!Validate(data, numbers))
                {
                    yield break;
                }
            }
            
            data[pos] = '.';
            
            foreach (var variant in ListVariants(data, numbers))
            {
                yield return variant;
            }

            data[pos] = '#';
            
            foreach (var variant in ListVariants(data, numbers))
            {
                yield return variant;
            }
            
            data[pos] = '?';
        }

        private bool Validate(char[] variant, List<int> numbers)
        {
            var cursor = 0;
            var result = true;
            
            for (int i = 0; i < numbers.Count; i++)
            {
                var length = 0;

                while (cursor < variant.Length && variant[cursor] == '.')
                {
                    cursor++;
                }
                
                while (cursor < variant.Length && variant[cursor] == '#')
                {
                    cursor++;
                    length++;
                }

                if (length == numbers[i])
                {
                    continue;
                }
                
                result = false;

                if (cursor < variant.Length && variant[cursor] == '?')
                {
                    if (length < numbers[i])
                    {
                        return true;
                    }
                }
                
                break;
            }
            
            while (cursor < variant.Length)
            {
                if (variant[cursor] == '#')
                {
                    result = false;
                    break;
                }

                cursor++;
            }
            
            return result;
        }
    }
}