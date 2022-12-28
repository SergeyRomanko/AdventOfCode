using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle24 : Puzzle
    {
        private struct State
        {
            public Vec2 Pos;
            public int Step;
            public Puzzle24Container Container;
            public int Heuristic => ((Container.Finish.x - Pos.x) + (Container.Finish.y - Pos.y));
        }
        
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

        private int DoTask1(Puzzle24Container startContainer)
        {
            var result = int.MaxValue;
            var candidates = new List<State>
            {
                new State
                {
                    Pos = startContainer.Start,
                    Step = 0,
                    Container = startContainer.NextStep()
                }
            };

            while (candidates.Count > 0)
            {
                var state = candidates.Aggregate((a, b) => a.Heuristic < b.Heuristic ? a : b);

                candidates.Remove(state);
                
                Console.WriteLine($" >>> {state.Pos} {state.Step} {state.Container.Finish} {result}");
                
                state.Container.Print(state.Pos);
                
                if (state.Pos == state.Container.Finish)
                {
                    result = Math.Min(result, state.Step);
                
                    continue;
                }

                if (state.Step >= result)
                {
                    continue;
                }

                var eur = (state.Container.Finish.x - state.Pos.x) + (state.Container.Finish.y - state.Pos.y);
                if (state.Step + eur >= result)
                {
                    continue;
                }
                
                var neighb = _offsets
                    .Select(x => state.Pos + x)
                    .Where(state.Container.IsValid)
                    .OrderBy(x => (state.Container.Finish.x - x.x) + (state.Container.Finish.y - x.y))
                    .ToArray();

                var nextContainer = state.Container.NextStep();
                for (var i = 0; i < neighb.Length; i++)
                {
                    candidates.Add(new State()
                    {
                        Pos = neighb[i],
                        Step = state.Step + 1,
                        Container = nextContainer
                    });
                }
            }
            
            return result;
        }
    }
}
