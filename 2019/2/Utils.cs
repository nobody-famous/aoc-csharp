namespace aoc.y2019.day2
{
    abstract class Solver : aoc.utils.ProblemSolver<long>
    {
        protected Solver(string file, int exp) : base(file, exp) { }

        protected long runMachine(long[] prog, int noun, int verb) {
            var mach = new aoc.y2019.intcode.Machine(prog);

            mach[1] = noun;
            mach[2] = verb;

            while (!mach.isHalted()) {
                mach.step();
            }

            return mach[0];
        }
    }
}