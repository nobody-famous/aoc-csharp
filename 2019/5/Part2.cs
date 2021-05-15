using aoc.intcode;

namespace aoc.y2019.day5
{
    class Part2 : Solver
    {
        public Part2(string file, int exp) : base(file, exp) { }

        protected override int doWork() {
            return base.doWork(new Input(5), new Output());
        }
    }
}
