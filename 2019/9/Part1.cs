using aoc.intcode;

namespace aoc.y2019.day9
{
    class Part1 : Solver
    {
        public Part1(string file, uint exp) : base(file, exp) { }

        protected override long doWork() {
            var prog = Parser.parseInput(inputFile);
            var provider = new Provider(1);
            var mach = new Machine(prog, provider);

            runMachine(mach);

            if (provider.lastValue is long v) {
                return v;
            } else {
                return 0;
            }
        }
    }
}