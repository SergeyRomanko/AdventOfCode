using System;
using System.Collections.Generic;
using System.Linq;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle16Task2
    {

        public static IEnumerable<int> Permute(IReadOnlyList<Puzzle16.Node> sequence, Puzzle16PermuteUtil util)
        {
            foreach (Puzzle16.Node currentNode in sequence)
            {
                var nextNodeUtil = new Puzzle16PermuteUtil
                {
                    Distances = util.Distances,
                    Pressure = util.Pressure,
                    Result = util.Result,
                    StepsLeft = util.StepsLeft,
                    PrevNode = currentNode
                };
                
                var distance = 1 + util.Distances.GetDistance(util.PrevNode, currentNode);

                nextNodeUtil.Result += util.Pressure * Math.Min(distance, util.StepsLeft);

                if (distance > util.StepsLeft)
                {
                    yield return nextNodeUtil.Result;

                    continue;
                }

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
