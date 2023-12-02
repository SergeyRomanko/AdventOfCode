namespace AdventOfCode.Year2022
{
    public sealed class Puzzle16Task2
    {
        private static int _maxResult = int.MinValue;
        
        public static int DoTask(
            List<Puzzle16.Node> nodesWithPressure,
            Puzzle16.DistanceCache distances,
            Puzzle16.Node start)
        {
            
            var playerA = new Puzzle16PermuteUtil
            {
                Distances = distances,
                StepsLeft = 26,
                PrevNode = start
            };
            
            var playerB = new Puzzle16PermuteUtil
            {
                Distances = distances,
                StepsLeft = 26,
                PrevNode = start
            };
            
            foreach (var value in Permute(nodesWithPressure, playerA, playerB))
            {
                if (value > _maxResult)
                {
                    _maxResult = value;
                }
            }

            return _maxResult;
        }
        
        public static IEnumerable<int> Permute(IReadOnlyList<Puzzle16.Node> sequence, Puzzle16PermuteUtil playerA, Puzzle16PermuteUtil playerB)
        {
            foreach (var (nodeA, nodeB) in GetPairs(sequence))
            {
                if (sequence.Count == 15)
                {
                    Console.WriteLine($"   nodeA:{nodeA.Name} nodeB:{nodeB.Name} MaxResult:{_maxResult}");
                }
                
                var newPlayerA = DoThings(playerA, nodeA);
                var newPlayerB = DoThings(playerB, nodeB);

                var noStepsLeft = newPlayerA.StepsLeft <= 0 && newPlayerB.StepsLeft <= 0;
                if (noStepsLeft)
                {
                    yield return GetResult(newPlayerA, newPlayerB);
                    
                    continue;
                }
                
                var remainingItems = sequence
                    .Where(x => x != nodeA && x != nodeB)
                    .ToList();
                
                if (remainingItems.Count == 0)
                {
                    yield return GetResult(newPlayerA, newPlayerB);
                    
                    continue;
                }

                //Если максимальный возможный результат для этой комбинации меньше текущего лучшего результата, то не рассматриваем эту комбинацию
                var superOptimisticResult = GetMaxAvailableResult(remainingItems, newPlayerA, newPlayerB);
                if (superOptimisticResult <= _maxResult)
                {
                    yield return -1;
                    
                    continue;
                }
    
                foreach (var result in Permute(remainingItems, newPlayerA, newPlayerB))
                {
                    yield return result;
                }
            }
        }

        private static Puzzle16PermuteUtil DoThings(Puzzle16PermuteUtil player, Puzzle16.Node? node)
        {
            var result = player.Clone();
            
            if (node == null)
            {
                return result;
            }
            
            var distance = 1 + player.Distances.GetDistance(player.PrevNode, node);
            var realDistance = Math.Min(distance, player.StepsLeft);

            result.PrevNode = node;
            result.Result += player.Pressure * realDistance;
            result.StepsLeft -= realDistance;

            if (distance <= player.StepsLeft)
            {
                result.Pressure += node.Preassure;
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
        
        private static int GetResult(Puzzle16PermuteUtil playerA, Puzzle16PermuteUtil playerB)
        {
            var resultA = playerA.Result + playerA.Pressure * playerA.StepsLeft;
            var resultB = playerB.Result + playerB.Pressure * playerB.StepsLeft;
                    
            return resultA + resultB;
        }
        
        //Какой результат у нас получится если открыть все оставшиеся краны прямо сейчас
        private static int GetMaxAvailableResult(IReadOnlyList<Puzzle16.Node> sequence, Puzzle16PermuteUtil playerA, Puzzle16PermuteUtil playerB)
        {
            var curResult = GetResult(playerA, playerB);

            var maxSteps = Math.Max(playerA.StepsLeft, playerB.StepsLeft);

            var maxAvailableSteam = 0;
            
            for (var i = 0; i < sequence.Count; i++)
            {
                maxAvailableSteam += sequence[i].Preassure * maxSteps;
            }
            
            return curResult + maxAvailableSteam;
        }
    }
}
