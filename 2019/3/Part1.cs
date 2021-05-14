using aoc.utils.geometry;

namespace aoc.y2019.day3
{
    class Part1 : Solver
    {
        public Part1(string file, int exp) : base(file, exp) { }

        protected override int doWork() {
            var (wire1, wire2) = Parser.parseInput(inputFile);
            var crosses = getCrosses(wire1, wire2);
            var minDist = int.MaxValue;
            var origin = new Point(0, 0);

            foreach (var cross in crosses) {
                var dist = Funcs.manDist(origin, cross);
                if (dist != 0 && dist < minDist) {
                    minDist = dist;
                }
            }

            return minDist;
        }
    }
}
