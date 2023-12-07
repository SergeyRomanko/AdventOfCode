using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle05 : Puzzle
    {
        private sealed class Ranges
        {
            public List<long> From   = new();
            public List<long> To     = new();
            public List<long> Length = new();

            public long Map(long value)
            {
                for (var i = 0; i < From.Count; i++)
                {
                    var delta = value - From[i];
                    if (delta >= 0 && delta < Length[i])
                    {
                        return To[i] + delta;
                    }
                }

                return value;
            }
        }
        
        private sealed class Data
        {
            public List<long> Seeds                 = new List<long>();
            public Ranges     SeedToSoil            = new Ranges();
            public Ranges     SoilToFertilizer      = new Ranges();
            public Ranges     FertilizerToWater     = new Ranges();
            public Ranges     WaterToLight          = new Ranges();
            public Ranges     LightToTemperature    = new Ranges();
            public Ranges     TemperatureToHumidity = new Ranges();
            public Ranges     HumidityToLocation    = new Ranges();
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var inputList = input.ToList();

            var data = InputToData(inputList);
            
            return new[]
            {
                data.Seeds.Select(x => Part1(x,           data)).Min().ToString(),
                //ToRanges(data.Seeds).Select(x => Part1(x, data)).Min().ToString(),
            };
        }
        
        private IEnumerable<long> ToRanges(List<long> dataSeeds)
        {
            for (var i = 0; i <= dataSeeds.Count / 2; i += 2)
            {
                var from = dataSeeds[i];
                var len  = dataSeeds[i + 1];

                for (var y = from; y < from + len; y++)
                {
                    yield return y;
                }
            }
        }

        private long Part1(long value1, Data data)
        {
            var value2 = data.SeedToSoil.Map(value1);
            var value3 = data.SoilToFertilizer.Map(value2);
            var value4 = data.FertilizerToWater.Map(value3);
            var value5 = data.WaterToLight.Map(value4);
            var value6 = data.LightToTemperature.Map(value5);
            var value7 = data.TemperatureToHumidity.Map(value6);
            var value8 = data.HumidityToLocation.Map(value7);
            
            return value8;
        }

        private Data InputToData(List<string> inputList)
        {
            var data = new Data();
            
            foreach (var group in GetGroup(inputList))
            {
                if (group[0].StartsWith("seeds:"))
                {
                    data.Seeds = group[0]
                                 .Replace("seeds:", "")
                                 .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                 .Select(long.Parse)
                                 .ToList();
                    
                    continue;
                }

                var range = group[0] switch
                {
                    "seed-to-soil map:"            => data.SeedToSoil,
                    "soil-to-fertilizer map:"      => data.SoilToFertilizer,
                    "fertilizer-to-water map:"     => data.FertilizerToWater,
                    "water-to-light map:"          => data.WaterToLight,
                    "light-to-temperature map:"    => data.LightToTemperature,
                    "temperature-to-humidity map:" => data.TemperatureToHumidity,
                    "humidity-to-location map:"    => data.HumidityToLocation,
                    _                              => throw new ArgumentOutOfRangeException()
                };
                
                for (var i = 1; i < group.Count; i++)
                {
                    var split = group[i].Split();

                    range.From.Add(long.Parse(split[1]));
                    range.To.Add(long.Parse(split[0]));
                    range.Length.Add(long.Parse(split[2]));
                }
            }

            return data;
        }
        
        private IEnumerable<List<string>> GetGroup(List<string> inputList)
        {
            var result = new List<string>();
            
            foreach (var line in inputList)
            {
                if (line == "")
                {
                    yield return result;
                    result = new List<string>();
                    continue;
                }
                
                result.Add(line);
            }
            
            yield return result;
        }
    }
}