using aoc.intcode;

namespace aoc.y2019.day5
{
    class Provider : Listener
    {
        long value;

        long lastOutput = 0;

        public Provider(int inputValue) {
            this.value = inputValue;
        }

        public long input() { return value; }
        public void output(long value) { lastOutput = value; }

        public long getLastOutput() { return lastOutput; }
    }

    abstract class Solver : aoc.utils.ProblemSolver<long>
    {
        public Solver(string file, long exp) : base(file, exp) { }

        protected void runMachine(Machine mach) {
            while (!mach.isHalted()) {
                mach.step();
            }
        }

        protected long doWork(Provider provider) {
            var prog = intcode.Parser.parseInput(inputFile);
            var mach = new Machine(prog, provider);

            runMachine(mach);

            return provider.getLastOutput();
        }
    }
}