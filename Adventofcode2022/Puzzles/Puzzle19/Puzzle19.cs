using System.Collections.Generic;
using System.Linq;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle19 : Puzzle
    {
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
                blueprints.Select(x => DoTask1(x) * x.Id).Sum().ToString(),
                ""
            };
        }

        private int DoTask1(Blueprint blueprint)
        {
            var ress = new int[4];
            var bots = new int[4];

            bots[0] = 1;
            
            for (var min = 0; min < 24; min++)
            {
                var delta = new int[4];

                for (var i = 0; i < bots.Length; i++)
                {
                    ress[i] += bots[i];
                }
                
                for (var i = 0; i < delta.Length; i++)
                {
                    bots[i] += delta[i];
                }
            }

            return ress[3];
        }
    }
}
