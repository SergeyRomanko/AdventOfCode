using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle07 : Puzzle
    {
        private class Node
        {
            public string Name;
            public bool IsFolder;
            public int Size;
            public Node Parent;
            public List<Node> Children = new();
        }

        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var root = new Node
            {
                Name = "/",
                IsFolder = true
            };
            
            GetTree(input, root);

            root.Size = UpdateSizes(root);

            var freeSpace = 70000000 - root.Size;

            return new[]
            {
                GetTask1Result(root).ToString(),
                
                Linear(root)
                    .Where(x => freeSpace + x.Size >= 30000000)
                    .Aggregate((c1, c2) => c1.Size < c2.Size ? c1 : c2)
                    .Size
                    .ToString(),
            };
        }
        
        private IEnumerable<Node> Linear(Node root)
        {
            if (root.IsFolder)
            {
                yield return root;
            }

            foreach (var node in root.Children.SelectMany(Linear))
            {
                yield return node;
            }
        }

        private int GetTask1Result(Node root)
        {
            return 
                root.Children
                    .Where(x => x.IsFolder)
                    .Where(x => x.Size <= 100000)
                    .Sum(x => x.Size) +
                root.Children.Sum(GetTask1Result);
        }

        private int UpdateSizes(Node root)
        {
            root.Size += root.Children.Sum(UpdateSizes);
            return root.Size;
        }

        private void GetTree(IReadOnlyList<string> input, Node root)
        {
            var node = root;
            
            foreach (var text in input)
            {
                var split = text.Split(' ');
                
                switch (split[1])
                {
                    case "cd":
                        node = GetNode(split[2], node, root);
                        break;
                    case "ls":
                        break;
                    default:
                        var child = new Node
                        {
                            Parent = node,
                            Name = split[1],
                            IsFolder = split[0] == "dir",
                            Size = split[0] == "dir" ? 0 : int.Parse(split[0])
                        };

                        node.Children.Add(child);
                        break;
                }
            }
        }

        private Node GetNode(string dir, Node node, Node root)
        {
            return dir switch
            {
                "/" => root,
                ".." => node.Parent,
                _ => node.Children.Where(x => x.IsFolder).First(x => x.Name == dir)
            };
        }
    }
}
