namespace aoc.y2019.day12
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        protected override int doWork() {
            var points = Parser.parseInput(inputFile);

            System.Console.WriteLine($"points {points.Count}");
            throw new System.NotImplementedException();
        }
    }
}
