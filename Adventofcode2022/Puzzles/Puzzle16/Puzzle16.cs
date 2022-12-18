using System;
using System.Collections;
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
            public int Hash;
            public int Preassure;
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
                    Preassure = int.Parse(x[1]),
                    Hash = x[0].GetHashCode()
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

            var start = allNodes.First(x => x.Name == "AA");
            
            return new[]
            {
                DoPart1(start, 30, allNodes).ToString(),
                ""
            };
        }

        private int DoPart1(Node start, int steps, List<Node> all)
        {
            var result = 0;

            var targets = all.Where(x => x.Preassure > 0).ToList();
            
            targets.Add(start);
            
            var distances = new DistanceCache();
            for (int i = 0; i < targets.Count; i++)
            {
                for (int j = 0; j < targets.Count; j++)
                {
                    var from = targets[i];
                    var to = targets[j];
                    
                    distances.AddDistance(from, to, Distance(from, to, all));
                }
            }
            
            targets.Remove(start);

            var util = new PermuteUtil
            {
                Distances = distances,
                Steps = steps,
                PrevNode = start,
            };
            
            /*
            var permute = Permute(targets, util);
            foreach (var val in permute)
            {
                Console.WriteLine($"   {string.Join(" ", val.Nodes.Select(x => x.Name))} => {val.Result}");
            }
            */
            
            var val = Permute(targets, util)
                .Aggregate((a, b) => a.Result > b.Result ? a : b)
                ;

            result = val.Result;
            
            Console.WriteLine($"   {string.Join(" ", val.Nodes.Select(x => x.Name))} => {val.Result}");

            return result;
        }

        private static IEnumerable<PermuteResult> Permute(IEnumerable<Node> sequence, PermuteUtil util)
        {
            if (sequence == null)
            {
                yield break;
            }
        
            var list = sequence.ToList();
        
            if (!list.Any())
            {
                util.Result += util.Preasure * util.Steps;
                
                yield return new PermuteResult
                {
                    Result = util.Result,
                    Nodes = Enumerable.Empty<Node>()
                };
            }
            else
            {
                foreach (Node currentNode in list)
                {
                    var nextNodeUtil = new PermuteUtil
                    {
                        Distances = util.Distances,
                        Preasure = util.Preasure,
                        Result = util.Result,
                        Steps = util.Steps,
                        PrevNode = currentNode
                    };
                    
                    //======================================
                    var distance = 1 + util.Distances.GetDistance(util.PrevNode, currentNode);

                    nextNodeUtil.Result += nextNodeUtil.Preasure * Math.Min(distance, util.Steps);

                    if (distance > nextNodeUtil.Steps)
                    {
                        yield return new PermuteResult
                        {
                            Result = nextNodeUtil.Result,
                            Nodes = Enumerable.Empty<Node>()
                        };

                        continue;
                    }

                    nextNodeUtil.Steps -= distance;
                    nextNodeUtil.Preasure += currentNode.Preassure;
                    
                    //======================================
                    
                    var remainingItems = list.Where(x => x != currentNode);
        
                    foreach (var result in Permute(remainingItems, nextNodeUtil))
                    {
                        result.Nodes = result.Nodes.Prepend(currentNode);
                        
                        yield return result;
                    }
                }
            }
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

        private struct PermuteUtil
        {
            public DistanceCache Distances;
            public Node PrevNode;
            public int Preasure;
            public int Result;
            public int Steps;
        }
        
        private class PermuteResult
        {
            public int Result;
            public IEnumerable<Node> Nodes;
        }

        private class DistanceCache
        {
            private readonly Dictionary<int, int> _cache = new();

            public void AddDistance(Node a, Node b, int distance)
            {
                _cache[GetPairHash(a, b)] = distance;
                _cache[GetPairHash(b, a)] = distance;
            }
            
            public int GetDistance(Node a, Node b)
            {
                return _cache[GetPairHash(a, b)];
            }

            private int GetPairHash(Node a, Node b)
            {
                return HashCode.Combine(a.Hash, b.Hash);
            }
        }
    }
}

/*
private static IEnumerable<IEnumerable<T>> Permute<T>(IEnumerable<T> sequence)
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
        
            foreach (var permutationOfRemainder in Permute(remainingItems))
            {
                yield return permutationOfRemainder.Prepend(startingElement);
            }
        
            startingElementIndex++;
        }
    }
}
*/
