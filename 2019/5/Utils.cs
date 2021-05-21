using aoc.intcode;

namespace aoc.y2019.day5
{
    class Provider : Listener
    {
        int value;

        int lastOutput = 0;

        public Provider(int inputValue) {
            this.value = inputValue;
        }

        public int input() { return value; }
        public void output(int value) { lastOutput = value; }

        public int getLastOutput() { return lastOutput; }
    }

    abstract class Solver : aoc.utils.ProblemSolver<int>
    {
        public Solver(string file, int exp) : base(file, exp) { }

        protected void runMachine(Machine mach) {
            while (!mach.isHalted()) {
                mach.step();
            }
        }

        protected int doWork(Provider provider) {
            var prog = intcode.Parser.parseInput(inputFile);
            var mach = new Machine(prog, provider);

            runMachine(mach);

            return provider.getLastOutput();
        }
    }
}