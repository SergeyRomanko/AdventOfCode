using AdventOfCode.Common;

namespace AdventOfCode.Year2022
{
    public class Puzzle09 : Puzzle
    {
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            (int x, int y)[] offsets = input.SelectMany(GetOffsets).ToArray();
            
            return new[]
            {
                Sim(02, offsets).ToString(),
                Sim(10, offsets).ToString()
            };
        }

        private int Sim(int len, (int x, int y)[] offsets)
        {
            var knots = Enumerable.Range(0, len).Select(x => (x:0, y:0)).ToList();
            
            var history = new HashSet<(int x, int y)> { knots.Last() };
            
            foreach (var (offsetX, offsetY) in offsets)
            {
                knots[0] = (knots[0].x + offsetX, knots[0].y + offsetY);

                for (int i = 1; i < len; i++)
                {
                    (int x, int y) delta = (knots[i - 1].x - knots[i].x, knots[i - 1].y - knots[i].y);

                    if (Math.Abs(delta.x) > 1 || Math.Abs(delta.y) > 1)
                    {
                        (int x, int y) tmp = (
                            delta.x / Math.Max(Math.Abs(delta.x), 1),
                            delta.y / Math.Max(Math.Abs(delta.y), 1));
                        
                        knots[i] = (knots[i].x + tmp.x, knots[i].y + tmp.y);
                    }
                    else
                    {
                        break;
                    }
                }
                    
                history.Add(knots.Last());
            }

            return history.Count;
        }

        private IEnumerable<(int x, int y)> GetOffsets(string s)
        {
            var words = s.Split(' ');
            var offset = words[0] switch
            {
                "U" => (0, +1),
                "D" => (0, -1),
                "L" => (-1, 0),
                "R" => (+1, 0),
            };
            
            for (var i = 0; i < int.Parse(words[1]); i++)
            {
                yield return offset;
            }
        }
    }
}
