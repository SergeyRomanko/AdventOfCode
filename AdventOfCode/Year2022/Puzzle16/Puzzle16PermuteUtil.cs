namespace AdventOfCode.Year2022
{
    public struct Puzzle16PermuteUtil
    {
        public Puzzle16.DistanceCache Distances;
        public Puzzle16.Node PrevNode;
            
        public int Pressure;
        public int Result;
        public int StepsLeft;

        public Puzzle16PermuteUtil Clone()
        {
            return new Puzzle16PermuteUtil
            {
                Distances = Distances,
                PrevNode = PrevNode,
                Pressure = Pressure,
                Result = Result,
                StepsLeft = StepsLeft
            };
        }
    }
}
