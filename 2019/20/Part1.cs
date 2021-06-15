namespace aoc.y2019.day20
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        protected override int doWork() {
            var parser = new Parser(inputFile);

            parser.parseInput();

            throw new System.NotImplementedException();
        }
    }
}
