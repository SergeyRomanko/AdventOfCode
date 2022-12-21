using System;
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
                DoTask1(input).ToString(),
                DoTask2(input).ToString(),
            };
        }

        private List<Monkey> CreateMonkeys(IReadOnlyList<string> input)
        {
            var monkeys = CreateMonkeysNodes(input).ToList();

            foreach (var op in monkeys.OfType<Operator>())
            {
                op.A = monkeys.First(x => x.Name == op.NameA);
                op.B = monkeys.First(x => x.Name == op.NameB);
            }

            return monkeys;
        }

        private long DoTask2(IReadOnlyList<string> input)
        {
            var monkeys = CreateMonkeys(input);
            
            var root = (Operator) monkeys.First(x => x.Name == "root");
            var humn = (Value   ) monkeys.First(x => x.Name == "humn");

            var data = GetCriticalPath(monkeys, humn, root).ToList();

            data.Reverse();
            
            var equalsTo = data.Contains(root.A) ? root.B : root.A;
            
            var inverted = new List<string>();
            
            inverted.Add($"root: {equalsTo.Result}");
            
            for (var i = 1; i < data.Count - 1; i++)
            {
                var prv = data[i - 1];
                var cur = (Operator) data[i];
                var nxt = data[i + 1];
                var arg = cur.A == nxt ? cur.B : cur.A;
                
                inverted.Add($"{arg.Name}: {arg.Result}");

                switch (cur.Op)
                {
                    case "+":
                        
                        inverted.Add($"{cur.Name}: {prv.Name} - {arg.Name}");
                        
                        break;
                    case "-":
                        
                        if (cur.A == nxt)
                        {
                            inverted.Add($"{cur.Name}: {prv.Name} + {arg.Name}");
                        }
                        else
                        {
                            inverted.Add($"{cur.Name}: {arg.Name} - {prv.Name}");
                        }

                        break;
                    case "*":
                        
                        inverted.Add($"{cur.Name}: {prv.Name} / {arg.Name}");

                        break;
                    case "/":
                        
                        if (cur.A == nxt)
                        {
                            inverted.Add($"{cur.Name}: {prv.Name} * {arg.Name}");
                        }
                        else
                        {
                            inverted.Add($"{cur.Name}: {arg.Name} / {prv.Name}");
                        }
                        
                        break;
                }
            }

            var monkeys2 = CreateMonkeys(inverted);

            var name = data[^2].Name;
            var target = (Operator) monkeys2.First(x => x.Name == name);
            
            return target.Result;
        }

        private long DoTask1(IReadOnlyList<string> input)
        {
            var monkeys = CreateMonkeys(input);
            
            var root = monkeys.First(x => x.Name == "root");

            return root.Result;
        }

        private IEnumerable<Monkey> CreateMonkeysNodes(IReadOnlyList<string> input)
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
        
        private IEnumerable<Monkey> GetCriticalPath(List<Monkey> monkeys, Monkey humn, Monkey root)
        {
            var cur = humn;

            while (cur != root)
            {
                yield return cur;

                cur = monkeys.OfType<Operator>().First(x => x.A == cur || x.B == cur);
            }
            
            yield return cur;
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
