using System;
using System.Collections.Generic;
using System.Linq;

namespace Adventofcode2022.Puzzles
{
    public sealed class Puzzle16Task2
    {
        public static IEnumerable<int> Permute(IReadOnlyList<Puzzle16.Node> sequence, Puzzle16PermuteUtil playerA, Puzzle16PermuteUtil playerB)
        {
            foreach (var (nodeA, nodeB) in GetPairs(sequence))
            {
                var newPlayerA = DoThings(playerA, nodeA);
                var newPlayerB = DoThings(playerB, nodeB);

                var remainingItems = sequence
                    .Where(x => x != nodeA && x != nodeB)
                    .ToList();
                
                if (remainingItems.Count == 0)
                {
                    ???;
                    
                    yield return nextNodeUtil.Result + nextNodeUtil.Pressure * nextNodeUtil.StepsLeft;
                    
                    continue;
                }
    
                foreach (var result in Permute(remainingItems, newPlayerA, newPlayerB))
                {
                    yield return result;
                }
            }
        }

        private static Puzzle16PermuteUtil DoThings(Puzzle16PermuteUtil player, Puzzle16.Node? nodeA)
        {
            var result = player.Clone();
            
            if (nodeA == null)
            {
                return result;
            }
            
            var distance = 1 + player.Distances.GetDistance(player.PrevNode, nodeA);
            var realDistance = Math.Min(distance, player.StepsLeft);

            result.PrevNode = nodeA;
            result.Result += player.Pressure * realDistance;
            result.StepsLeft -= realDistance;

            if (distance <= player.StepsLeft)
            {
                result.Pressure += nodeA.Preassure;
            }

            return result;
        }

        private static IEnumerable<(Puzzle16.Node?, Puzzle16.Node?)> GetPairs(IReadOnlyList<Puzzle16.Node> sequence)
        {
            if (sequence.Count == 0)
            {
                yield break;
            }
            
            if (sequence.Count == 1)
            {
                yield return (sequence[0], null);
                yield return (null, sequence[0]);
                yield break;
            }
            
            foreach (var nodeA in sequence)
            {
                foreach (var nodeB in sequence)
                {
                    if (nodeA == nodeB)
                    {
                        continue;
                    }
                    
                    yield return (nodeA, nodeB);
                }
            }
        }
    }
}

/*

*/
