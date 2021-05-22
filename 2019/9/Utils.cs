using aoc.intcode;

namespace aoc.y2019.day9
{
    class Provider : Listener
    {
        private long inputValue;

        public Provider(long inputValue) { this.inputValue = inputValue; }

        public long? lastValue { get; set; }

        public long input() { return inputValue; }

        public void output(long value) { lastValue = value; }
    }

    abstract class Solver : aoc.utils.ProblemSolver<long>
    {
        public Solver(string file, uint exp) : base(file, exp) { }

        protected void runMachine(Machine mach) {
            while (!mach.isHalted()) {
                mach.step();
            }
        }
    }
}
