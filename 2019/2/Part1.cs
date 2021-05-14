using aoc.intcode;

namespace aoc.y2019.day2
{
    class Part1 : Solver
    {
        public Part1(string fileName, int exp) : base(fileName, exp) { }

        protected override int doWork()
        {
            var prog = Parser.parseInput(this.inputFile);

            return runMachine(prog, 12, 2);
        }
    }
}
