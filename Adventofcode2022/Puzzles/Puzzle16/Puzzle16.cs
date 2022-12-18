using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle16 : Puzzle
    {
        private class Node
        {
            public string Name;
            public int Val;
            public List<Node> Tunnels;
        }

        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var data = input
                .Select(x => x
                    .Replace("Valve ", "")
                    .Replace(" has flow rate=", "|")
                    .Replace("; tunnels lead to valves ", "|")
                    .Replace("; tunnel leads to valve ", "|")
                )
                .Select(x => x.Split("|"))
                .ToList();

            var allNodes = data
                .Select(x => new Node
                {
                    Name = x[0],
                    Val = int.Parse(x[1])
                })
                .ToList();

            foreach (var node in allNodes)
            {
                node.Tunnels = data
                    .First(x => x[0] == node.Name)[2]
                    .Split(", ")
                    .Select(x => allNodes.First(n => n.Name == x))
                    .ToList();
            }

            return new[]
            {
                DoThings(allNodes, allNodes.First(x => x.Name == "AA")).ToString(),
                ""
            };
        }

        private int DoThings(List<Node> all, Node cur)
        {
            var result = 0;
            var preasure = 0;
            var open = new List<Node>();

            for (var i = 0; i < 2; i++)
            {
                Console.WriteLine($"==== Minute {i} ====");
                Console.WriteLine($"Open valves {string.Join(", ", open.Select(x => x.Name))} Preasure: {preasure}");
                
                result += preasure;
                var nxt = GetNext(all, cur);

                var openValve = nxt == cur;
                if (openValve)
                {
                    preasure += cur.Val;
                    cur.Val = 0;
                    open.Add(cur);
                    
                    Console.WriteLine($"You open valve {cur.Name}");
                }
                else
                {
                    Console.WriteLine($"You move to valve  {nxt.Name}");
                }

                cur = nxt;
            }

            return result;
        }

        private Node GetNext(List<Node> all, Node cur)
        {
            var candidates = cur.Tunnels
                .ToDictionary(x => x, x => EvaluateMove(x, all));

            if (cur.Val > 0)
            {
                candidates.Add(cur, EvaluateOpen(cur, all));
            }
            
            return candidates
                .OrderBy(x => x.Value)
                .First()
                .Key;
        }

        private int EvaluateMove(Node from, List<Node> all)
        {
            var tmp = all
                .Where(x => x.Val > 0)
                .Select(x => (x.Name, Distance(from, x, all), 1, x.Val, 0))
                .ToList();
                 
            for (var i = 0; i < tmp.Count; i++)
            {
                var value = tmp[i];
                value.Item5 = (value.Item2 + value.Item3) * value.Item4;
                tmp[i] = value;
            }
             
            var result = tmp.Sum(x => (x.Item2 + x.Item3) * x.Item4);
             
            Console.WriteLine($" + EvaluateMove {from.Name} {result}");
             
            foreach (var v in tmp)
            {
                Console.WriteLine($"    - {v.Item1} {v.Item2, 2} {v.Item3, 2} {v.Item4, 2} {v.Item5, 3}");
            }

            return result;
        }
        
        private int EvaluateOpen(Node from, List<Node> all)
        {
             var tmp = all
                .Where(x => x.Val > 0)
                .Select(x => (x.Name, Distance(from, x, all), (from == x ? 1 : 2), x.Val * (from == x ? -1 : +1), 0))
                .ToList();
                 
             for (var i = 0; i < tmp.Count; i++)
             {
                 var value = tmp[i];
                 value.Item5 = (value.Item2 + value.Item3) * value.Item4;
                 tmp[i] = value;
             }
             
             var result = tmp.Sum(x => (x.Item2 + x.Item3) * x.Item4);
             
             Console.WriteLine($" + EvaluateOpen {from.Name} {result}");
             
             foreach (var v in tmp)
             {
                 Console.WriteLine($"    - {v.Item1} {v.Item2, 2} {v.Item3, 2} {v.Item4, 2} {v.Item5, 3}");
             }

             return result;
        }

        private int Distance(Node from, Node to, List<Node> all)
        {
            var weights = all.ToDictionary(x => x, x => int.MaxValue);

            weights[from] = 0;

            while (weights.Count > 0)
            {
                var cur = weights
                    .Aggregate((a, b) => a.Value < b.Value ? a : b);

                if (cur.Key == to)
                {
                    return cur.Value;
                }

                foreach (var node in cur.Key.Tunnels)
                {
                    if (weights.ContainsKey(node))
                    {
                        weights[node] = Math.Min(cur.Value + 1, weights[node]);
                    }
                }

                weights.Remove(cur.Key);
            }

            return -1;
        }
        
        public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                yield break;
            }
        
            var list = sequence.ToList();
        
            if (!list.Any())
            {
                yield return Enumerable.Empty<T>();
            }
            else
            {
                var startingElementIndex = 0;
        
                foreach (var startingElement in list)
                {
                    var index = startingElementIndex;
                    var remainingItems = list.Where((e, i) => i != index);
        
                    foreach (var permutationOfRemainder in remainingItems.Permute())
                    {
                        yield return permutationOfRemainder.Prepend(startingElement);
                    }
        
                    startingElementIndex++;
                }
            }
        }
    }
}
