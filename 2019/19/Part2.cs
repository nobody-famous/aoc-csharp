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

            // drawBeam(100);

            var count = countRow(pt);
            while (count > 0 && count < 100) {
                pt.x += 1;
                pt.y += 1;
                count = countRow(pt);
                // System.Console.WriteLine($"{pt} {count}");
            }

            throw new System.NotImplementedException();
        }
    }
}
