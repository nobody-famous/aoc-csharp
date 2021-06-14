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

        private int getUpperBound(int jump) {
            var bound = jump;

            while (!fits(bound, jump)) {
                bound *= 2;
            }

            return bound;
        }

        private bool fits(int y, int size) {
            var start = findBeamStart(y);
            var pt = new Point(start.x + size - 1, start.y - (size - 1));
            var ok = checkPoint(pt);

            return checkPoint(pt);
        }

        protected override int doWork() {
            prog = Parser.parseInput(inputFile);

            var size = 100;

            var high = size * size;
            var mid = high / 2;
            var low = 0;

            while (low < mid) {
                if (fits(mid, size)) {
                    high = mid;
                } else {
                    low = mid;
                }

                mid = low + ((high - low) / 2);
            }

            var start = findBeamStart(high);
            var pt = new Point(start.x, start.y - (size - 1));

            return (pt.x * 10000) + pt.y;
        }
    }
}
