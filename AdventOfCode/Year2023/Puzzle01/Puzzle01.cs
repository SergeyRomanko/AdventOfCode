using System.Text;
using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle01 : Puzzle
    {
        private StringBuilder _sb      = new();
        private string[]      _numbers = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var inputList = input.ToList();
            
            return new[]
            {
                inputList.Select(GetNumbersFromString).Sum().ToString(),
                inputList.Select(PreprocessString).Select(GetNumbersFromString).Sum().ToString(),
            };
        }

        private int GetNumbersFromString(string input)
        {
            var first = input.Where(char.IsNumber).First();
            var last  = input.Where(char.IsNumber).Last();

            return int.Parse($"{first}{last}");
        }
        
        private string PreprocessString(string input)
        {
            _sb.Clear();
            
            for (int i = 0; i < input.Length; i++)
            {
                _sb.Append(StringToNumberOrFirstChar(input.AsSpan()[i..]));
            }
            
            return _sb.ToString();
        }

        private char StringToNumberOrFirstChar(ReadOnlySpan<char> span)
        {
            for (var i = 0; i < _numbers.Length; i++)
            {
                if (span.StartsWith(_numbers[i]))
                {
                    return (char)('1' + i);
                }
            }

            return span[0];
        }
    }
}