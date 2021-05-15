using aoc.intcode;

namespace aoc.y2019.day5
{
    class Input : InputProvider
    {
        private int value;

        public Input(int value) { this.value = value; }

        public int next() {
            return value;
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

    abstract class Solver : aoc.utils.ProblemSolver<int>
    {
        public Solver(string file, int exp) : base(file, exp) { }

        protected void runMachine(Machine mach) {
            while (!mach.isHalted()) {
                mach.step();
            }
        }

        protected int doWork(InputProvider input, Output outHandler) {
            var prog = intcode.Parser.parseInput(inputFile);
            var mach = new Machine(prog, input, outHandler);

            runMachine(mach);

            return outHandler.getLastOutput();
        }
    }
}