using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day3
{
    class Part2 : Solver
    {
        public Part2(string file, int exp) : base(file, exp) { }

        private Dictionary<Point, int> getCrossDistances(List<Line> wire, List<Point> crosses) {
            var totalDist = 0;
            var dists = new Dictionary<Point, int>();

            foreach (var line in wire) {
                foreach (var cross in crosses) {
                    if (pointOnLine(cross, line)) {
                        var ptDist = totalDist + Funcs.manDist(line.start, cross);
                        dists[cross] = ptDist;
                    }
                }

                totalDist += Funcs.manDist(line.start, line.end);
            }

            return dists;
        }

        protected override int doWork() {
            var (wire1, wire2) = Parser.parseInput(inputFile);
            var crosses = getCrosses(wire1, wire2);

            var dists1 = getCrossDistances(wire1, crosses);
            var dists2 = getCrossDistances(wire2, crosses);

            var minDist = int.MaxValue;
            foreach (var cross in crosses) {
                var dist = dists1[cross] + dists2[cross];
                if (dist < minDist) {
                    minDist = dist;
                }
            }

            return minDist;
        }
    }
}
