using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle25 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            return new[]
            {
                DoTask1(input),
                ""
            };
        }

        private string DoTask1(IReadOnlyList<string> snafus)
        {
            var resultBase5 = snafus
                 .Select(SnafuToBase5)
                 .Aggregate(AddBase5);
            
             return Base5ToSnafu(resultBase5);
        }

        private List<int> AddBase5(IEnumerable<int> a, IEnumerable<int> b)
        {
            var ra = new List<int>(a);
            var rb = new List<int>(b);

            ra.Reverse();
            rb.Reverse();
            
            var index = 0;
            var tmp = 0;
            var result = new List<int>();
            while (index < ra.Count || index < rb.Count || tmp > 0)
            {
                var aval = index < ra.Count ? ra[index] : 0;
                var bval = index < rb.Count ? rb[index] : 0;

                var val = aval + bval + tmp;
                
                result.Add(val % 5);

                tmp = val >= 5 ? 1 : 0;

                index++;
            }
            
            result.Reverse();

            return result;
        }
        
        private List<int> SnafuToBase5(string snafu)
        {
            if (string.IsNullOrEmpty(snafu) || snafu == "0")
            {
                return new List<int> { 0 };
            }
            
            var data = snafu
                .ToCharArray()
                .Reverse()
                .Select(x => x switch
                {
                    '0' => 0,
                    '1' => 1,
                    '2' => 2,
                    '-' => -1,
                    '=' => -2
                }).ToList();

            var result = new List<int>();
            var tmp = 0;
            for (var i = 0; i < data.Count; i++)
            {
                tmp += data[i];
                var val = tmp % 5;
                
                tmp = tmp < 0 ? -1 : 0;
                
                result.Add(val >= 0 ? val : 5 + val);
            }

            result.Reverse();

            return result;
        }

        private string Base5ToSnafu(List<int> base5)
        {
            var result = new List<int>(base5);
            
            for (int i = result.Count - 1; i >= 0; i--)
            {
                var value = result[i];

                if (result[i] == 3) value = -2;
                if (result[i] == 4) value = -1;

                if (result[i] == value)
                {
                    continue;
                }

                result[i] = value;
                
                for (var j = i - 1; j >= 0; j--)
                {
                    var val = result[j] + 1;
                    var isValid = (val < 5);
                    result[j] = isValid ? val : 0;
                    
                    if (isValid)
                    {
                        break;
                    }
                }
            }

            return SnafuToText(result);
        }
        
        private string SnafuToText(List<int> snafu)
        {
            var digits = snafu
                .SkipWhile(x => x == 0)
                .Select(x => x switch
                {
                    0  => '0',
                    1  => '1',
                    2  => '2',
                    -1 => '-',
                    -2 => '=',
                });

            return string.Join("", digits);
        }
        
        private long Base5ToDecimal(IEnumerable<int> base5)
        {
            return base5
                .Reverse()
                .Select((a, digit) => a * (long)Math.Pow(5, digit))
                .Sum();
        }
    }
}
