using aoc.intcode;

namespace aoc.y2019.day5
{

    class Part1 : Solver
    {
        public Part1(string file, int exp) : base(file, exp) { }

        protected override int doWork() {
            return base.doWork(new Input(1), new Output());
        }
    }
}