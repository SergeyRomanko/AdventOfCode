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

            for (var i = 0; i < 30; i++)
            {
                result += preasure;
                var nxt = GetNext(all, cur);

                var openValve = nxt == cur;
                if (openValve)
                {
                    preasure += cur.Val;
                    cur.Val = 0;
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
                candidates.Add(cur);
            }

            return candidates
                .OrderByDescending(x => EvaluateMove(x, all))
                .First();
        }

        private int EvaluateMove(Node from, List<Node> all)
        {
            return all
                .Where(x => x.Val > 0)
                .Sum(x => (Distance(from, x, all) + 1) * x.Val);
        }
        
        private int EvaluateOpen(Node from, List<Node> all)
        {

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
                    weights[node] = Math.Min(cur.Value + 1, weights[node]);
                }

                weights.Remove(cur.Key);
            }

            return -1;
        }
    }
}