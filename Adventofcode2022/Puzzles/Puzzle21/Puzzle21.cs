using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle21 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            return new[]
            {
                DoTask1(input).ToString(),  //1263608045- bad
                ""
            };
        }

        private long DoTask1(IReadOnlyList<string> input)
        {
            var monkeys = CreateMonkeys(input).ToList();

            foreach (var op in monkeys.OfType<Operator>())
            {
                op.A = monkeys.First(x => x.Name == op.NameA);
                op.B = monkeys.First(x => x.Name == op.NameB);
            }
            
            var res = monkeys.First(x => x.Name == "root");

            return res.Result;
        }

        private IEnumerable<Monkey> CreateMonkeys(IReadOnlyList<string> input)
        {
            foreach (var line in input)
            {
                var split1 = line.Split(": ");
                var name = split1[0];

                if (long.TryParse(split1[1], out var value))
                {
                    yield return new Value
                    {
                        Name = name,
                        Val = value
                    };
                }
                else
                {
                    var split2 = split1[1].Split(' ');
                    
                    yield return new Operator
                    {
                        Name = name,
                        NameA = split2[0],
                        Op    = split2[1],
                        NameB = split2[2]
                    };
                }
            }
        }

        private abstract class Monkey
        {
            public string Name;
            public abstract long Result { get; }
        }
        
        private class Value : Monkey
        {
            public long Val {  get; set; }
            public override long Result => Val;
        }
        
        private class Operator : Monkey
        {
            public string Op {  get; set; }
            
            public Monkey A {  get; set; }
            public Monkey B {  get; set; }
            
            public string NameA {  get; set; }
            public string NameB {  get; set; }

            public override long Result
            {
                get => Op switch
                {
                    "+" => A.Result + B.Result,
                    "-" => A.Result - B.Result,
                    "*" => A.Result * B.Result,
                    "/" => A.Result / B.Result,
                };
            }
        }
    }
}
