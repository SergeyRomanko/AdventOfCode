using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle06 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var inputList = input.ToList();

            var times = inputList[0].Replace("Time:", "")
                                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                    .Select(long.Parse)
                                    .ToList();
            
            var distances = inputList[1].Replace("Distance:", "")
                                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                    .Select(long.Parse)
                                    .ToList();
            
            var time2     = long.Parse(inputList[0].Replace("Time:", "").Replace(" ", ""));
            var distance2 = long.Parse(inputList[1].Replace("Distance:", "").Replace(" ", ""));
            
            return new[]
            { 
                Part1(times, distances).Aggregate((a,b) => a * b).ToString(),
                DoThings(time2, distance2).ToString()
            };
        }

        private IEnumerable<long> Part1(List<long> times, List<long> distances)
        {
            for (int i = 0; i < times.Count; i++)
            {
                var time     = times[i];
                var distance = distances[i];

                yield return DoThings(time, distance);
            }
        }
        
        private long DoThings(long time, long distance)
        {
            var (a, b) = Solve(time, distance);

            var min = Math.Min(a, b);
            var max = Math.Max(a, b);

            var from = (long) Math.Ceiling(min);
            var to   = (long) Math.Floor(max);

            return Math.Max(to - from + 1, 0);
        }

        private (double, double) Solve(long time, long distance)
        {
            double a = -1;
            double b = time;
            double c = -(distance + 0.001f);
    
            // Calculate the discriminant
            double discriminant = b * b - 4 * a * c;
    
            // Check the discriminant for roots
            if (discriminant > 0)
            {
                // Two real and distinct roots
                double root1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                double root2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                
                return (root1, root2);
    
            }
            else if (discriminant == 0)
            {
                // One real root (double root)
                double root = -b / (2 * a);
    
                throw new Exception($"Root is real and equal: {root}");
            }
            else
            {
                // Complex roots
                double realPart      = -b                       / (2 * a);
                double imaginaryPart = Math.Sqrt(-discriminant) / (2 * a);
    
                throw new Exception($"Roots are complex: {realPart} + {imaginaryPart}i and {realPart} - {imaginaryPart}i");
            }
        }
    }
}