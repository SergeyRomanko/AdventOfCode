using AdventOfCode.Common;

namespace AdventOfCode.Year2022
{
    public class Puzzle20 : Puzzle
    {
        private class Node
        {
            public Node Prv;
            public Node Nxt;
            public long Value;
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var data = input
                .Select(int.Parse)
                .ToList();
            
            return new[]
            {
                DoTask1(data).ToString(),
                DoTask2(data).ToString()
            };
        }

        private long DoTask2(List<int> data)
        {
            var nodes = data
                .Select(x => new Node { Value = x * 811589153L })
                .ToList();
            
            for (var i = 0; i < nodes.Count; i++)
            {
                nodes[i].Nxt = i == nodes.Count - 1 ? nodes[0] : nodes[i + 1];
                nodes[i].Prv = i == 0 ? nodes[^1] : nodes[i - 1];
            }
            
            for (var times = 0; times < 10; times++)
            {
                for (var i = 0; i < data.Count; i++)
                {
                    Mix(nodes[i], data.Count);
                }
            }

            var results = new List<long>();
            
            var node = GetNodeByValue(nodes[0], 0L);
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

        private long DoTask1(List<int> data)
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
                Mix(nodes[i], data.Count);
                
                //Print(nodes[0]);
            }
            
            var results = new List<long>();
            
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

        private void Mix(Node target, long count)
        {
            var offset = Math.Abs(target.Value) % (count - 1L);
            
            if (offset == 0)
            {
                return;
            }

            var moveLeft = target.Value < 0L;

            var prv1 = target.Prv;
            var nxt1 = target.Nxt;

            var cur = target;
            for (var i = 0; i < offset; i++)
            {
                cur = moveLeft ? cur.Prv : cur.Nxt;
            }
            
            var prv2 = moveLeft ? cur.Prv : cur;
            var nxt2 = moveLeft ? cur : cur.Nxt;

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

        private Node GetNodeByValue(Node start, long value)
        {
            var result = start;
            
            while (result.Value != value)
            {
                result = result.Nxt;
            }

            return result;
        }

        private void Print(Node start)
        {
            var cur = start;
            var lst = new List<long>();
            
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
