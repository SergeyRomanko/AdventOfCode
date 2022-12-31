using System;
using System.Collections.Generic;
using System.Linq;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle16Task1
    {
        public static IEnumerable<int> Permute(IReadOnlyList<Puzzle16.Node> sequence, Puzzle16PermuteUtil util)
        {
            foreach (Puzzle16.Node currentNode in sequence)
            {
                var distance = 1 + util.Distances.GetDistance(util.PrevNode, currentNode);
                var currentResult = util.Result + util.Pressure * Math.Min(distance, util.StepsLeft);

                if (distance > util.StepsLeft)
                {
                    yield return currentResult;

                    continue;
                }

                var nextNodeUtil = util.Clone();
                nextNodeUtil.PrevNode = currentNode;
                nextNodeUtil.Result = currentResult;
                nextNodeUtil.StepsLeft -= distance;
                nextNodeUtil.Pressure += currentNode.Preassure;
                
                if (sequence.Count == 1)
                {
                    yield return nextNodeUtil.Result + nextNodeUtil.Pressure * nextNodeUtil.StepsLeft;
                    
                    continue;
                }
                
                var remainingItems = sequence
                    .Where(x => x != currentNode)
                    .ToList();
    
                foreach (var result in Permute(remainingItems, nextNodeUtil))
                {
                    yield return result;
                }
            }
        }
    }
}
