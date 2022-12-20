using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle20 : Puzzle
    {
        private class Node
        {
            public Node Prv;
            public Node Nxt;
            public int Value;
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var data = input
                .Select(int.Parse)
                .ToList();
            
            return new[]
            {
                DoTask1(data).ToString(),
                ""
            };
        }

        private int DoTask1(List<int> data)
        {
            var nodes = data
                .Select(x => new Node { Value = x })
                .ToList();
            
            for (var i = 0; i < nodes.Count; i++)
            {
                nodes[i].Nxt = i == nodes.Count - 1 ? nodes[0] : nodes[i + 1];
                nodes[i].Prv = i == 0 ? nodes[^1] : nodes[i - 1];
            }

            //Print(nodes[0]);
            
            for (var i = 0; i < data.Count; i++)
            {
                Mix(nodes[i], data[i], data.Count);
                
                //Print(nodes[0]);
            }
            
            var results = new List<int>();
            
            var node = GetNodeByValue(nodes[0], 0);
            for (int i = 1; i <= 3000; i++)
            {
                node = node.Nxt;

                if (i % 1000 == 0)
                {
                    results.Add(node.Value);
                }
            }
            
            return results.Sum();
        }

        private void Mix(Node target, int value, int count)
        {
            var offset = Math.Abs(value) % (count - 1);
            
            if (offset == 0)
            {
                return;
            }

            var prv1 = target.Prv;
            var nxt1 = target.Nxt;

            var cur = target;
            for (var i = 0; i < offset; i++)
            {
                cur = value < 0 ? cur.Prv : cur.Nxt;
            }
            
            var prv2 = value < 0 ? cur.Prv : cur;
            var nxt2 = value < 0 ? cur : cur.Nxt;

            if (prv2 == target || nxt2 == target)
            {
                return;
            }
            
            prv1.Nxt = nxt1;
            nxt1.Prv = prv1;

            prv2.Nxt = target;
            nxt2.Prv = target;

            target.Prv = prv2;
            target.Nxt = nxt2;
        }

        private Node GetNodeByValue(Node start, int value)
        {
            var result = start;
            
            while (result.Value != value)
            {
                result = result.Nxt;

                if (result == start)
                {
                    throw new Exception($"Value not found {value}");
                }
            }

            return result;
        }

        private void Print(Node start)
        {
            var cur = start;
            var lst = new List<int>();
            
            while (true)
            {
                lst.Add(cur.Value);

                cur = cur.Nxt;

                if (cur == start)
                {
                    break;
                }
            }
            
            Console.WriteLine(string.Join(", ", lst));
        }
    }
}
