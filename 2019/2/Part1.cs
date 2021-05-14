using aoc.intcode;

namespace aoc.y2019.day2
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string fileName, int exp) : base(fileName, exp) { }

        protected override int doWork()
        {
            var prog = Parser.parseInput(this.inputFile);
            var mach = new Machine(prog);

            mach[1] = 12;
            mach[2] = 2;

            while (!mach.isHalted())
            {
                mach.step();
            }

            return mach[0];
        }
    }
}
