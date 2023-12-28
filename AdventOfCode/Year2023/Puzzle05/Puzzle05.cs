using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle05 : Puzzle
    {
        private struct Range
        {
            public long Start;
            public long Length;

            public bool IsInRange(long value)
            {
                var x = value - Start;
                return 0 <= x && x < Length;
            }
        }
        
        private sealed class Map
        {
            public Range From;
            public Range To;
            
            public Range MapTo(Range range)
            {
                if (!(From.Start <= range.Start && range.Start < (From.Start + From.Length)))
                {
                    throw new Exception();
                }
                
                if (!(0 < range.Length && range.Length <= From.Length))
                {
                    throw new Exception();
                }
                
                return new Range
                {
                    Start  = To.Start + (range.Start - From.Start),
                    Length = range.Length
                };
            }
        }
        
        private sealed class Ranges
        {
            public List<Map> Maps = new();

            public Range Map(Range value)
            {
                for (var i = 0; i < Maps.Count; i++)
                {
                    if (Maps[i].From.IsInRange(value.Start))
                    {
                        return Maps[i].MapTo(value);
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
                ToRanges(data.Seeds).Select(x => Part2(x, data)).Min().ToString(),
            };
        }

        private long Part2(Range range, Data data)
        {
            var list1 = new List<Range> {range};

            var sum1 = list1.Select(x => x.Length).Sum();

            var list2 = DoThings(list1, data.SeedToSoil).ToList();
            var list3 = DoThings(list2, data.SoilToFertilizer).ToList();
            var list4 = DoThings(list3, data.FertilizerToWater).ToList();
            var list5 = DoThings(list4, data.WaterToLight).ToList();
            var list6 = DoThings(list5, data.LightToTemperature).ToList();
            var list7 = DoThings(list6, data.TemperatureToHumidity).ToList();
            var list8 = DoThings(list7, data.HumidityToLocation).ToList();
            
            var sum2 = list8.Select(x => x.Length).Sum();

            if (sum1 != sum2)
            {
                throw new Exception();
            }

            return list8.Select(x => x.Start).Min();
        }

        private IEnumerable<Range> DoThings(List<Range> input, Ranges ranges)
        {
            for (var i = 0; i < input.Count; i++)
            {
                var range = input[i];
                
                while (range.Length > 0)
                {
                    var subRange = GetNextRange(range, ranges);

                    range = new Range
                    {
                        Start  = range.Start  + subRange.Length,
                        Length = range.Length - subRange.Length
                    };

                    yield return subRange;
                }
            }
        }

        private Range GetNextRange(Range range, Ranges ranges)
        {
            foreach (var map in ranges.Maps)
            {
                if (map.From.IsInRange(range.Start))
                {
                    var length1 = (map.From.Start + map.From.Length) - range.Start;
                    var length2 = range.Length;
                    
                    var subRange = range with
                    {
                        Length = Math.Min(length1, length2)
                    };

                    return map.MapTo(subRange);
                }
            }

            throw new Exception();
        }

        private IEnumerable<Range> ToRanges(List<long> dataSeeds)
        {
            for (var i = 0; i < dataSeeds.Count; i += 2)
            {
                yield return new Range
                {
                    Start  = dataSeeds[i],
                    Length = dataSeeds[i + 1]
                };
            }
        }

        private long Part1(long value0, Data data)
        {
            var value1 = new Range
            {
                Start  = value0,
                Length = 1
            };
            
            var value2 = data.SeedToSoil.Map(value1);
            var value3 = data.SoilToFertilizer.Map(value2);
            var value4 = data.FertilizerToWater.Map(value3);
            var value5 = data.WaterToLight.Map(value4);
            var value6 = data.LightToTemperature.Map(value5);
            var value7 = data.TemperatureToHumidity.Map(value6);
            var value8 = data.HumidityToLocation.Map(value7);
            
            return value8.Start;
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

                var originalMaps = new List<Map>();
                
                for (var i = 1; i < group.Count; i++)
                {
                    var split = group[i].Split();
                    
                    originalMaps.Add(new Map
                    {
                        From = new Range
                        {
                            Start  = long.Parse(split[1]),
                            Length = long.Parse(split[2])
                        },
                        To = new Range
                        {
                            Start  = long.Parse(split[0]),
                            Length = long.Parse(split[2])
                        }
                    });
                }
                
                range.Maps = CreateMaps(originalMaps);
            }

            return data;
        }

        private List<Map> CreateMaps(List<Map> originalMaps)
        {
            var gaps = new List<Map>();
            var maps = originalMaps.OrderBy(x => x.From.Start).ToList();

            var cursor = 0L;
            
            foreach (var map in maps)
            {
                var gap = CreateGap(cursor, map.From.Start - cursor);

                if (gap.From.Length > 0)
                {
                    gaps.Add(gap);
                }
                
                cursor += map.From.Length + gap.From.Length;
            }
            
            gaps.Add(CreateGap(cursor, long.MaxValue - cursor));
            
            return originalMaps.Concat(gaps).OrderBy(x => x.From.Start).ToList();
        }

        private Map CreateGap(long start, long length)
        {
            var range = new Range
            {
                Start  = start,
                Length = length
            };

            return new Map
            {
                From = range,
                To   = range
            };
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