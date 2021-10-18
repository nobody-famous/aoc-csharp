using aoc.y2019.intcode;
using aoc.utils.geometry;

namespace aoc.y2019.day19
{
    class Part1 : Solver
    {
        private int size = 50;

        public Part1(string file, int exp) : base(file, exp) { }

        override protected bool isValid(Point pt) {
            return pt.x < size && pt.y < size;
        }

        protected override int doWork() {
            prog = Parser.parseInput(inputFile);

            var count = 1;
            var pt = findFirst();

            while (pt.x < size && pt.y < size) {
                var rowCount = countRow(pt);

                count += rowCount;

                pt.x += 1;
                pt.y += 1;
            }

            return count;
        }
    }
}
