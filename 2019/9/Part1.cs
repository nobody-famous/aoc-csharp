using aoc.intcode;

namespace aoc.y2019.day9
{
    class Part1 : aoc.utils.ProblemSolver<uint>
    {
        public Part1(string file, uint exp) : base(file, exp) { }

        private class Provider : Listener
        {
            public long input() { System.Console.WriteLine("INPUT"); return 1; }

            public void output(long value) { System.Console.WriteLine($"OUTPUT {value}"); }
        }

        private void runMachine(Machine mach) {
            while (!mach.isHalted()) {
                mach.step();
            }
        }

        protected override uint doWork() {
            var prog = Parser.parseInput(inputFile);
            var mach = new Machine(prog, new Provider());

            mach.setDebug(true);
            runMachine(mach);

            throw new System.NotImplementedException();
        }
    }
}