using AdventOfCode.Common;

namespace AdventOfCode.Year2022
{
    public class Puzzle02 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            return new[]
            {
                input
                    .Select(x => (A: x[0] - 'A', B: x[2] - 'X'))
                    .Select(x => GetRoundResult(x.A, x.B))
                    .Sum()
                    .ToString(),
                
                input
                    .Select(x => (A: x[0] - 'A', B: x[2] - 'X'))
                    .Select(x => GetRoundResult(x.A, GetMyTurn(x.A, x.B)))
                    .Sum()
                    .ToString(),
            };
        }

        private int GetRoundResult(int a, int b)
        {
            var result = b + 1;

            //есть ощущение что тут можно лучше
            if (a == b)
            {
                result += 3;
            }
            else
            {
                var isWin = a == (b + 1) % 3;
                
                result += isWin ? 0 : 6;
            }

            return result;
        }
        
        private int GetMyTurn(int a, int b)
        {
            return (a + b + 2) % 3;
        }
    }
}
