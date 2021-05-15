using aoc.intcode;

namespace aoc.y2019.day5
{
    class Input : InputProvider
    {
        public int next() {
            return 1;
        }
    }

    class Output : OutputHandler
    {
        private int last = 0;

        public void data(int value) {
            last = value;
        }

        public int getLastOutput() { return last; }
    }

    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        private void runMachine(Machine mach) {
            while (!mach.isHalted()) {
                mach.step();
            }
        }

        protected override int doWork() {
            var prog = intcode.Parser.parseInput(inputFile);
            var outHandler = new Output();
            var mach = new Machine(prog, new Input(), outHandler);

            runMachine(mach);

            return outHandler.getLastOutput();
        }
    }
}