using System;
using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle19 : Puzzle
    {
        private readonly IEnumerable<int[]> _options = new[]
        {
             new int[4] {0, 0, 0, 0},
             new int[4] {1, 0, 0, 0},
             new int[4] {0, 1, 0, 0},
             new int[4] {0, 0, 1, 0},
             new int[4] {0, 0, 0, 1},
        };

        private struct Blueprint
        {
            public int Id;
            public int OreRobotCost;
            public int ClayRobotCost;
            public int ObsidianRobotCostOre;
            public int ObsidianRobotCostClay;
            public int GeodeRobotCostOre;
            public int GeodeRobotCostObsidian;
        }
        
        private class State
        {
            public int Minute;
            public int[] Res;
            public int[] Bot;
            public List<State> Next;

            public int Heuristics => Bot[0] + Bot[1] * 10 + Bot[2] * 100 + Bot[3] * 1000;
        }

        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var blueprints = input
                .Select(x => x.Replace("Blueprint ", ""))
                .Select(x => x.Replace(": Each ore robot costs ", ";"))
                .Select(x => x.Replace(" ore. Each clay robot costs ", ";"))
                .Select(x => x.Replace(" ore. Each obsidian robot costs ", ";"))
                .Select(x => x.Replace(" ore and ", ";"))
                .Select(x => x.Replace(" clay. Each geode robot costs ", ";"))
                .Select(x => x.Replace(" obsidian.", ""))
                .Select(x => x.Split(';'))
                .Select(x => new Blueprint
                {
                    Id = int.Parse(x[0]),
                    OreRobotCost = int.Parse(x[1]),
                    ClayRobotCost = int.Parse(x[2]),
                    ObsidianRobotCostOre = int.Parse(x[3]),
                    ObsidianRobotCostClay = int.Parse(x[4]),
                    GeodeRobotCostOre = int.Parse(x[5]),
                    GeodeRobotCostObsidian = int.Parse(x[6])
                })
                .ToList();
            
            return new[]
            {
                blueprints.Select(x => DoTask(x, 24) * x.Id).Sum().ToString(),
                
                blueprints
                    .Take(3)
                    .Select(x => DoTask(x, 32))
                    .Aggregate(1, (x,y) => x * y)
                    .ToString()
            };
        }

        private int DoTask(Blueprint blueprint, int minutesMax)
        {
            var minute1 = new State
            {
                Minute = 1,
                Bot = new []{1,0,0,0},
                Res = new []{0,0,0,0},
            };
            
            var maxGeodes = Enumerable.Repeat(int.MaxValue, 1000).ToList();
            
            var max = 0;
            
            var candidates = new List<State> { minute1 };
            while (candidates.Count > 0)
            {
                var candidate = candidates.Aggregate((a, b) => a.Heuristics > b.Heuristics ? a : b);
                
                candidates.Remove(candidate);
                
                var bestMinute2 = maxGeodes[candidate.Bot[Mineral.Geode] + 1];
                if (candidate.Minute > bestMinute2)
                {
                    continue;
                }
                maxGeodes[candidate.Bot[Mineral.Geode]] = Math.Min(candidate.Minute, maxGeodes[candidate.Bot[Mineral.Geode]]);
                
                var next = GetNextStates(candidate, blueprint, minutesMax).ToList();

                candidate.Res[Mineral.Ore] += candidate.Bot[Mineral.Ore];
                candidate.Res[Mineral.Clay] += candidate.Bot[Mineral.Clay];
                candidate.Res[Mineral.Obsidian] += candidate.Bot[Mineral.Obsidian];
                candidate.Res[Mineral.Geode] += candidate.Bot[Mineral.Geode];
                
                foreach (var nxt in next)
                {
                    nxt.Res[Mineral.Ore] += candidate.Bot[Mineral.Ore];
                    nxt.Res[Mineral.Clay] += candidate.Bot[Mineral.Clay];
                    nxt.Res[Mineral.Obsidian] += candidate.Bot[Mineral.Obsidian];
                    nxt.Res[Mineral.Geode] += candidate.Bot[Mineral.Geode];
                }

                candidate.Next = next;
                
                candidates.AddRange(candidate.Next);

                max = Math.Max(max, candidate.Res[Mineral.Geode]);
            }

            //Console.WriteLine($"   {blueprint.Id, 2}: {max} [minutesMax:{minutesMax}]");

            return max;
        }

        private IEnumerable<State> GetNextStates(State candidate, Blueprint blueprint, int minutesMax)
        {
            if (candidate.Minute == minutesMax)
            {
                yield break;
            }

            foreach (var option in _options)
            {
                var oreTotal = 
                    blueprint.OreRobotCost         * option[Mineral.Ore] + 
                    blueprint.ClayRobotCost        * option[Mineral.Clay] + 
                    blueprint.ObsidianRobotCostOre * option[Mineral.Obsidian] +
                    blueprint.GeodeRobotCostOre    * option[Mineral.Geode];

                if (candidate.Res[Mineral.Ore] < oreTotal)
                {
                    continue;
                }

                var clayTotal = blueprint.ObsidianRobotCostClay * option[Mineral.Obsidian];
                if (candidate.Res[Mineral.Clay] < clayTotal)
                {
                    continue;
                }
                
                var obsidianTotal = blueprint.GeodeRobotCostObsidian * option[Mineral.Geode];
                if (candidate.Res[Mineral.Obsidian] < obsidianTotal)
                {
                    continue;
                }
                
                var next = new State
                {
                    Minute = candidate.Minute + 1,
                    Bot = candidate.Bot.ToArray(),
                    Res = candidate.Res.ToArray(),
                };

                next.Res[Mineral.Ore] -= oreTotal;
                next.Res[Mineral.Clay] -= clayTotal;
                next.Res[Mineral.Obsidian] -= obsidianTotal;

                if (option[Mineral.Ore] > 0)      next.Bot[Mineral.Ore]++;
                if (option[Mineral.Clay] > 0)     next.Bot[Mineral.Clay]++;
                if (option[Mineral.Obsidian] > 0) next.Bot[Mineral.Obsidian]++;
                if (option[Mineral.Geode] > 0)    next.Bot[Mineral.Geode]++;
                
                if (
                    next.Bot[Mineral.Clay] == 0 &&
                    next.Res[Mineral.Ore] >= blueprint.OreRobotCost &&
                    next.Res[Mineral.Ore] >= blueprint.ClayRobotCost
                )
                {
                    continue;
                }

                if (
                    next.Bot[Mineral.Obsidian] == 0 &&
                    next.Res[Mineral.Ore]  >= blueprint.ObsidianRobotCostOre &&
                    next.Res[Mineral.Clay] >= blueprint.ObsidianRobotCostClay
                    )
                {
                    continue;
                }

                if (
                    next.Res[Mineral.Ore] >= blueprint.GeodeRobotCostOre &&
                    next.Res[Mineral.Obsidian] >= blueprint.GeodeRobotCostObsidian)
                {
                    continue;
                }
                
                if (
                    next.Bot[Mineral.Ore] > blueprint.OreRobotCost &&
                    next.Bot[Mineral.Ore] > blueprint.ClayRobotCost &&
                    next.Bot[Mineral.Ore] > blueprint.ObsidianRobotCostOre &&
                    next.Bot[Mineral.Ore] > blueprint.GeodeRobotCostOre
                    )
                {
                    continue;
                }
                
                if (next.Bot[Mineral.Clay] > blueprint.ObsidianRobotCostClay)
                {
                    continue;
                }
                
                if (next.Bot[Mineral.Obsidian] > blueprint.GeodeRobotCostObsidian)
                {
                    continue;
                }

                yield return next;
            }
        }

        private static class Mineral
        {
            public static readonly int Ore      = 0;
            public static readonly int Clay     = 1;
            public static readonly int Obsidian = 2;
            public static readonly int Geode    = 3;
        }
    }
}

/*
 Task1:
    1: 1 [minutesMax:24]
    2: 3 [minutesMax:24]
    3: 3 [minutesMax:24]
    4: 1 [minutesMax:24]
    5: 10 [minutesMax:24]
    6: 3 [minutesMax:24]
    7: 0 [minutesMax:24]
    8: 3 [minutesMax:24]
    9: 3 [minutesMax:24]
   10: 2 [minutesMax:24]
   11: 3 [minutesMax:24]
   12: 9 [minutesMax:24]
   13: 1 [minutesMax:24]
   14: 15 [minutesMax:24]
   15: 0 [minutesMax:24]
   16: 1 [minutesMax:24]
   17: 5 [minutesMax:24]
   18: 0 [minutesMax:24]
   19: 1 [minutesMax:24]
   20: 0 [minutesMax:24]
   21: 2 [minutesMax:24]
   22: 0 [minutesMax:24]
   23: 9 [minutesMax:24]
   24: 1 [minutesMax:24]
   25: 2 [minutesMax:24]
   26: 3 [minutesMax:24]
   27: 2 [minutesMax:24]
   28: 0 [minutesMax:24]
   29: 1 [minutesMax:24]
   30: 0 [minutesMax:24]
    
Task:
    1: 21 [minutesMax:32]
    2: 27 [minutesMax:32]
    3: 38 [minutesMax:32]
 */
