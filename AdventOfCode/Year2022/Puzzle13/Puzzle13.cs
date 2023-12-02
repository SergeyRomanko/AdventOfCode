using AdventOfCode.Common;

namespace AdventOfCode.Year2022
{

    public class Puzzle13 : Puzzle
    {
        private enum Result
        {
            True,
            Equal,
            False
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var lines = input
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

             var data1 = Enumerable
                .Range(0, lines.Count / 2)
                .Where(x => TestPair(lines[x * 2], lines[x * 2 + 1]) == Result.True)
                .Select(x => x + 1)
                .Sum();

             var special1 = GetNode(GetItems("[[2]]").ToArray());
             var special2 = GetNode(GetItems("[[6]]").ToArray());

             special1.IsSpecial = true;
             special2.IsSpecial = true;

             var ordered = lines
                 .Select(x => GetNode(GetItems(x).ToArray()))
                 .Concat(new[] { special1, special2 })
                 .ToList();
             
             ordered.Sort(CompareNodes);
             
             var data2 = ordered
                 .Select((x,i) => x.IsSpecial ? i + 1 : -1)
                 .Where(x => x > 0)
                 .Aggregate((a,b) => a * b);
             
            return new[]
            {
                data1.ToString(),
                data2.ToString()
            };
        }

        private int CompareNodes(Node x, Node y)
        {
            return Compare(x, y) switch
            {
                Result.True  => -1,
                Result.Equal =>  0,
                Result.False => +1
            };
        }

        private Result TestPair(string line1, string line2)
        {
            var data1 = GetNode(GetItems(line1).ToArray());
            var data2 = GetNode(GetItems(line2).ToArray());

            var result = Compare(data1, data2);
            
            //Console.WriteLine($">>> line1:{line1} line2:{line2} result:{result}");
            
            return result;
        }

        private Result Compare(Node l, Node r)
        {
            if (l is Val l1 && r is Val r1)
            {
                if (l1.Value < r1.Value)
                {
                    return Result.True;
                }
                
                if (l1.Value > r1.Value)
                {
                    return Result.False;
                }
                
                return Result.Equal;
            }
            
            if (l is Lst l2 && r is Lst r2)
            {
                var cnt = Math.Max(l2.Children.Count, r2.Children.Count);
                for (var i = 0; i < cnt; i++)
                {
                    if (l2.Children.Count == i)
                    {
                        return Result.True;
                    }
                    
                    if (r2.Children.Count == i)
                    {
                        return Result.False;
                    }
                    
                    var result = Compare(l2.Children[i], r2.Children[i]);
                    if (result != Result.Equal)
                    {
                        return result;
                    }
                }
                
                return Result.Equal;
            }
            
            if (l is Lst l3 && r is Val r3)
            {
                return Compare(l3, new Lst { Children = new List<Node>{ r3 } });
            }
            
            if (l is Val l4 && r is Lst r4)
            {
                return Compare(new Lst { Children = new List<Node>{ l4 } }, r4);
            }

            throw new Exception();
        }

        private Node GetNode(string[] signals)
        {
            if (signals.Length == 1)
            {
                return new Val { Value = int.Parse(signals[0]) };
            }

            var content = Enumerable
                .Range(1, signals.Length - 2)
                .Select(x => signals[x])
                .ToArray();

            return new Lst { Children = GetElements(content).Select(GetNode).ToList() };
        }

        private IEnumerable<string[]> GetElements(string[] items)
        {
            var depth = 0;
            var tmp = new List<string>();
            
            foreach (var item in items)
            {
                tmp.Add(item);

                if (item == "[")
                {
                    depth++;
                }
                
                if (item == "]")
                {
                    depth--;
                }

                if (depth == 0)
                {
                    yield return tmp.ToArray();
                    
                    tmp.Clear();
                }
            }
        }

        private IEnumerable<string> GetItems(string text)
        {
            var tmp = new List<char>();
            
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                
                switch (c)
                {
                    case '[':
                        
                        yield return c.ToString();
                        
                        break;
                    
                    case ']':

                        if (tmp.Count > 0)
                        {
                            yield return string.Join("", tmp);
                            tmp.Clear();
                        }
                        
                        yield return c.ToString();
                        
                        break;
                    
                    case ',':
                        
                        if (tmp.Count > 0)
                        {
                            yield return string.Join("", tmp);
                        
                            tmp.Clear();
                        }

                        break;
                    
                    default: 
                        tmp.Add(c);
                        break;
                }
            }
        }
    }

    public class Lst : Node
    {
        public List<Node> Children = new List<Node>();
    }
    
    public class Val : Node
    {
        public int Value;
    }
    
    public class Node
    {
        public bool IsSpecial;
    }
}
