using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle24 : Puzzle
    {
        private int _task1 = int.MaxValue;
        private int cnt = 0;
        
        private readonly Vec2[] _offsets = new []
        {
            Vec2.Zero,
            Vec2.Down,
            Vec2.Left,
            Vec2.Up,
            Vec2.Right
        };
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            return new[]
            {
                DoTask1(new Puzzle24Container(input)).ToString(),
                ""
            };
        }

        private int DoTask1(Puzzle24Container container)
        {
            Task1(container.Start, 0, container);
            
            return _task1;
        }

        private void Task1(Vec2 me, int step, Puzzle24Container container)
        {
            if (cnt++ > 20)
            {
                return;
            }
            
            container.Print(me);
            
            var eur = (container.Finish.x - me.x) + (container.Finish.y - me.y);
            
            Console.WriteLine($" >>> {me} {step} {eur} {container.Finish} {_task1}");
            
            if (me == container.Finish)
            {
                _task1 = Math.Min(_task1, step);
                
                return;
            }

            if (step >= _task1)
            {
                return;
            }

            var candidates = _offsets
                .Select(x => me + x)
                .Where(container.IsValid)
                .OrderBy(x => (container.Finish.x - x.x) + (container.Finish.y - x.y))
                .ToArray();

            Console.WriteLine($" >>> {step} Candidates:{candidates.Length}");
            
            var nextContainer = container.NextStep();
            
            for (var i = 0; i < candidates.Length; i++)
            {
                Task1(candidates[i], step + 1, nextContainer);
            }

            Console.WriteLine($" <<< {step}");
        }
    }
}
