using AdventOfCode.Common;

namespace AdventOfCode.Year2023
{
    public class Puzzle07 : Puzzle
    {
        [Flags]
        private enum Type
        {
            None,
            
            FiveOfAKind,
            FourOfAKind,
            FullHouse,
            ThreeOfAKind,
            TwoPair,
            OnePair,
            HighCard,
        }
        
        private class Hand : IComparable<Hand>
        {
            public List<Char> Chars;
            public List<int>  Value;
            public long       Bid;
            public Type       Type;

            public int CompareTo(Hand? other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                
                var type = Type.CompareTo(other.Type);
                if (type != 0)
                {
                    return type;
                }

                for (int i = 0; i < Chars.Count; i++)
                {
                    var chr = Value[i].CompareTo(other.Value[i]);
                    if (chr != 0)
                    {
                        return chr;
                    }
                }

                return 0;
            }
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var inputList1 = input.Select(ReadHand1).ToList();
            var inputList2 = input.Select(ReadHand2).ToList();

            var part1 = inputList1.OrderByDescending(x => x).ToList();
            var part2 = inputList2.OrderByDescending(x => x).ToList();
            
            return new[]
            {
                part1.Select((x, i) => x.Bid * ((long)(i + 1))).Sum().ToString(),
                part2.Select((x, i) => x.Bid * ((long)(i + 1))).Sum().ToString(),
            };
        }
        
        private Hand ReadHand1(string arg)
        {
            var value = new [] { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2'};
            var split = arg.Split(" ");

            return new Hand
            {
                Bid   = int.Parse(split[1]),
                Chars = split[0].ToList(),
                Type  = GetHandType(split[0].ToList()),
                Value = split[0].Select(x => Array.IndexOf(value,  x)).ToList(),
            };
        }
        
        private Hand ReadHand2(string arg)
        {
            var value = new [] { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'};
            var split = arg.Split(" ");

            return new Hand
            {
                Bid   = int.Parse(split[1]),
                Chars = split[0].ToList(),
                Type  = GetHandType(PreprocessPart2(split[0].ToList())),
                Value = split[0].Select(x => Array.IndexOf(value, x)).ToList(),
            };
        }

        private List<char> PreprocessPart2(List<char> toList)
        {
            var data = toList
                       .Where(x => x != 'J')
                       .GroupBy(x => x)
                       .OrderByDescending(x => x.Count())
                       .SelectMany(x => x)
                       .ToList();

            if (data.Count == 5 || data.Count == 0)
            {
                return toList;
            }

            return toList.Select(x => x == 'J' ? data[0] : x).ToList();
        }

        private Type GetHandType(List<char> toList)
        {
            var groups = toList
                         .GroupBy(x => x)
                         .Select(x => x.Count())
                         .OrderByDescending(x => x)
                         .ToList();

            //Five of a kind
            if (groups.Count == 1)
            {
                return Type.FiveOfAKind;
            }
            
            //Four of a kind
            if (groups.Count == 2 && groups[0] == 4)
            {
                return Type.FourOfAKind;
            }
            
            //Full house
            if (groups.Count == 2 && groups[0] == 3)
            {
                return Type.FullHouse;
            }
            
            //Three of a kind
            if (groups.Count == 3 && groups[0] == 3)
            {
                return Type.ThreeOfAKind;
            }
            
            //Two pair
            if (groups.Count == 3 && groups[0] == 2)
            {
                return Type.TwoPair;
            }
            
            //One pair
            if (groups.Count == 4 && groups[0] == 2)
            {
                return Type.OnePair;
            }
            
            //High card
            if (groups.Count == 5)
            {
                return Type.HighCard;
            }

            throw new Exception(string.Join("", toList));
        }
    }
}