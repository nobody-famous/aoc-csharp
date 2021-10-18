using aoc.y2019.intcode;

namespace aoc.y2019.day5
{

    class Part1 : Solver
    {
        public Part1(string file, int exp) : base(file, exp) { }

        protected override long doWork() {
            return base.doWork(new Provider(1));
        }
    }
}