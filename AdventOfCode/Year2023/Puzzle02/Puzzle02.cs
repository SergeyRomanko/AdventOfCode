using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle02 : Puzzle
    {
        public class GameData
        {
            public int                           Game;
            public List<Dictionary<string, int>> Rounds = new ();
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var inputList = input.ToList();
            
            return new[]
            {
                inputList.Select(x => GetGameData(x))
                         .Where(x => IsOk(x))
                         .Sum(x => x.Game)
                         .ToString(),
                
                inputList.Select(x => GetGameData(x))
                         .Select(x => GetPower(x))
                         .Sum()
                         .ToString(),
            };
        }

        private int GetPower(GameData data)
        {
            var result = new Dictionary<string, int>
            {
                ["red"]   = 0,
                ["green"] = 0,
                ["blue"]  = 0
            };
            
            foreach (var round in data.Rounds)
            {
                foreach (var (key, value) in round)
                {
                    result[key] = Math.Max(result[key], value);
                }
            }

            return result["red"] * result["green"] * result["blue"];
        }

        private bool IsOk(GameData data)
        {
            //only 12 red cubes, 13 green cubes, and 14 blue cubes?

            foreach (var round in data.Rounds)
            {
                foreach (var (key, value) in round)
                {
                    var isOk = key switch
                    {
                        "red"   => value <= 12,
                        "green" => value <= 13,
                        "blue"  => value <= 14,
                    };

                    if (!isOk)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private GameData GetGameData(string text)
        {
            //Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green

            var result = new GameData();
            
            var split1 = text.Split(":");
            
            result.Game = int.Parse(split1[0].Replace("Game ", ""));
            
            foreach (var data in split1[1].Split(";"))
            {
                var round = new Dictionary<string, int> { };
                
                foreach (var item in data.Split(","))
                {
                    var record = item.Trim().Split(" ");

                    round[record[1]] = int.Parse(record[0]);
                }
                result.Rounds.Add(round);
            }

            return result;
        }
    }
}