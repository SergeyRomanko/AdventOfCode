using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle16 : Puzzle
    {
        public class Node
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
            
            var nodesWithPressure = allNodes.Where(x => x.Preassure > 0).ToList();
            
            nodesWithPressure.Add(start);
            
            var distances = new DistanceCache();
            for (int i = 0; i < nodesWithPressure.Count; i++)
            {
                for (int j = 0; j < nodesWithPressure.Count; j++)
                {
                    var from = nodesWithPressure[i];
                    var to = nodesWithPressure[j];
                    
                    distances.AddDistance(from, to, Distance(from, to, allNodes));
                }
            }
            
            //В моем конфиге в кране start давление 0, поэтому кран start не рассматриваем
            nodesWithPressure.Remove(start);

            var util = new Puzzle16PermuteUtil
            {
                Distances = distances,
                StepsLeft = 30,
                PrevNode = start,
            };
            
            return new[]
            {
                Puzzle16Task1.Permute(nodesWithPressure, util).Max().ToString(),
                ""
            };
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

        public class DistanceCache
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
