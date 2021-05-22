using aoc.intcode;

namespace aoc.y2019.day9
{
    class Part1 : aoc.utils.ProblemSolver<long>
    {
        public Part1(string file, uint exp) : base(file, exp) { }

        private class Provider : Listener
        {
            public long? lastValue { get; set; }

            public long input() { return 1; }

            public void output(long value) { lastValue = value; }
        }

        private void runMachine(Machine mach) {
            while (!mach.isHalted()) {
                mach.step();
            }
        }

        protected override long doWork() {
            var prog = Parser.parseInput(inputFile);
            var provider = new Provider();
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