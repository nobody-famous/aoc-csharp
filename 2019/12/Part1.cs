namespace aoc.y2019.day12
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        protected override int doWork() {
            var moons = Parser.parseInput(inputFile);

            System.Console.WriteLine($"moons {moons.Count}");

            throw new System.NotImplementedException();
        }
    }
}
