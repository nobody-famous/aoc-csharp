namespace aoc.y2019.day14
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        protected override int doWork() {
            var reacts = Parser.parseInput(inputFile);

            System.Console.WriteLine($"reacts {reacts.Count}");
            throw new System.NotImplementedException();
        }
    }
}
