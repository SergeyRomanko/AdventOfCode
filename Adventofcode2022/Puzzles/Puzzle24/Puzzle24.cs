using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle24 : Puzzle
    {
        private class State
        {
            public int Heuristic;
            public Vec2 Pos;
            public int Step;
            public Puzzle24Container Container;

            public override int GetHashCode()
            {
                return HashCode.Combine(Pos.x, Pos.y, Step);
            }
        }
        
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
            var initialState = new Puzzle24Container(input, Debug);
            
            return new[]
            {
                DoTask1(initialState, initialState.Start, initialState.Finish).Step.ToString(),
                DoTask2(initialState).ToString()
            };
        }

        private int DoTask2(Puzzle24Container initialState)
        {
            var phase1 = DoTask1(initialState, initialState.Start, initialState.Finish);
            var phase2 = DoTask1(phase1.Container, initialState.Finish, initialState.Start);
            var phase3 = DoTask1(phase2.Container, initialState.Start, initialState.Finish);
            
            return phase1.Step + (phase2.Step + 1) + (phase3.Step + 1);
        }

        private State DoTask1(Puzzle24Container startContainer, Vec2 from, Vec2 to)
        {
            var result = new State
            {
                Step = int.MaxValue
            };
            
            var candidates = new List<State>
            {
                new State
                {
                    Heuristic = Vec2.ManhattanDistance(from, to),
                    Pos = from,
                    Step = 0,
                    Container = startContainer.NextStep()
                }
            };
            
            var visitedHashes = new HashSet<int>();
            
            while (candidates.Count > 0)
            {
                var state = candidates.Aggregate((a, b) => a.Heuristic < b.Heuristic ? a : b);

                candidates.Remove(state);

                if (state.Pos == to)
                {
                    result = result.Step <= state.Step ? result : state;
                
                    continue;
                }

                if (state.Step >= result.Step)
                {
                    continue;
                }

                var distance = Vec2.ManhattanDistance(state.Pos, to);
                if (state.Step + distance >= result.Step)
                {
                    continue;
                }
                
                var next = state.Container.NextStep();

                var neighb = _offsets
                    .Select(x => state.Pos + x)
                    .Where(state.Container.IsValid)
                    .Select(x => new State()
                    {
                        Heuristic = Vec2.ManhattanDistance(x, to),
                        Pos = x,
                        Step = state.Step + 1,
                        Container = next,
                    });
                
                foreach (var neighbState in neighb)
                {
                    if (visitedHashes.Contains(neighbState.GetHashCode()))
                    {
                        continue;
                    }

                    visitedHashes.Add(neighbState.GetHashCode());
                    
                    candidates.Add(neighbState);
                }
            }
            
            return result;
        }
    }
}
