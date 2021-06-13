using aoc.intcode;
using aoc.utils.geometry;

namespace aoc.y2019.day19
{
    class Part2 : Solver
    {
        public Part2(string file, int exp) : base(file, exp) { }

        override protected bool isValid(Point pt) {
            return true;
        }

        protected override int doWork() {
            prog = Parser.parseInput(inputFile);

            var pt = findFirst();

            throw new System.NotImplementedException();
        }
    }
}
