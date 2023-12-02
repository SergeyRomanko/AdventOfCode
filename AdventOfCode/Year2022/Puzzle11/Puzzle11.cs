using AdventOfCode.Common;

namespace AdventOfCode.Year2022
{
    public class Puzzle11 : Puzzle
    {
        private class Monkey
        {
            public List<ulong> Items;
            public string[] Data;
            public ulong Test;
            public int IfTrue;
            public int IfFalse;
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var monkeys = GetMonkeys(input);
            var results = new ulong[monkeys.Count];
            
            var magic = monkeys.Select(x => x.Test).Aggregate((a, b) => a * b);

            for (int i = 0; i < 20; i++)
            {
                Round(monkeys, 3, magic, results);
            }

            var r1 = results
                .OrderByDescending(x => x)
                .Take(2)
                .Aggregate((a, b) => a * b)
                .ToString();
            
            monkeys = GetMonkeys(input);
            results = new ulong[monkeys.Count];
            
            for (int i = 0; i < 10000; i++)
            {
                Round(monkeys, 1, magic, results);
            }
            
            var r2 = results
                .OrderByDescending(x => x)
                .Take(2)
                .Aggregate((a, b) => a * b)
                .ToString();
            
            return new[]
            {
                r1,    //98280
                r2     //17673687232
            };
        }

        private void Round(List<Monkey> monkeys, ulong divider, ulong magic, ulong[] results)
        {
            for (var m = 0; m < monkeys.Count; m++)
            {
                var monkey = monkeys[m];
                
                for (var i = 0; i < monkey.Items.Count; i++)
                {
                    var argA = monkey.Data[0] == "old" ? monkey.Items[i] : ulong.Parse(monkey.Data[0]);
                    var argB = monkey.Data[2] == "old" ? monkey.Items[i] : ulong.Parse(monkey.Data[2]);

                    var level = monkey.Data[1] switch
                    {
                        "*" => argA * argB,
                        "+" => argA + argB,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    level /= divider;

                    var index = level % monkey.Test == 0
                        ? monkey.IfTrue
                        : monkey.IfFalse;
                    
                    monkeys[index].Items.Add(level % magic);
                    
                    results[m]++;
                }
                
                monkey.Items.Clear();
            }
        }

        private List<Monkey> GetMonkeys(IReadOnlyList<string> input)
        {
            var monkeys = new List<Monkey>();
            string tmp;
            
            foreach (var line in input)
            {
                switch (line)
                {
                    case string s when s.StartsWith("Monkey "):
                        monkeys.Add(new Monkey());
                        break;
                    case string s when s.StartsWith("  Starting items: "):
                        tmp = s.Replace("  Starting items: ", "");
                        monkeys.Last().Items = tmp.Split(", ").Select(ulong.Parse).ToList();
                        break;
                    case string s when s.StartsWith("  Operation: new = "):
                        tmp = s.Replace("  Operation: new = ", "");
                        monkeys.Last().Data = tmp.Split(" ");
                        break;
                    case string s when s.StartsWith("  Test: divisible by "):
                        tmp = s.Replace("  Test: divisible by ", "");
                        monkeys.Last().Test = ulong.Parse(tmp);
                        break;
                    case string s when s.StartsWith("    If true: throw to monkey "):
                        tmp = s.Replace("    If true: throw to monkey ", "");
                        monkeys.Last().IfTrue = int.Parse(tmp);
                        break;
                    case string s when s.StartsWith("    If false: throw to monkey "):
                        tmp = s.Replace("    If false: throw to monkey ", "");
                        monkeys.Last().IfFalse = int.Parse(tmp);
                        break;
                }
            }

            return monkeys;
        }
    }
}
