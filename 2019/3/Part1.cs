using System.Collections.Generic;
using aoc.utils;

namespace aoc.y2019.day3
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        private bool inRange(int start, int end, int value) {
            return (value >= start && value <= end) || (value >= end && value <= start);
        }

        private Point? getCrossPoint(Line line1, Line line2) {
            Point? point = null;

            if (line1.start.x == line1.end.x) {
                if (inRange(line2.start.x, line2.end.x, line1.start.x) && inRange(line1.start.y, line1.end.y, line2.start.y)) {
                    point = new Point(line1.start.x, line2.start.y);
                }
            } else if (line1.start.y == line1.end.y) {
                if (inRange(line2.start.y, line2.end.y, line1.start.y) && inRange(line1.start.x, line1.end.x, line2.start.x)) {
                    point = new Point(line2.start.x, line1.start.y);
                }
            }

            return (point != null && point.x != 0 && point.y != 0) ?
                 point : null;
        }

        private List<Point> getCrosses(List<Line> wire1, List<Line> wire2) {
            var crosses = new List<Point>();

            foreach (var line1 in wire1) {
                foreach (var line2 in wire2) {
                    var pt = getCrossPoint(line1, line2);

                    if (pt is Point p) {
                        crosses.Add(p);
                    }
                }
            }

            return crosses;
        }

        protected override int doWork() {
            var (wire1, wire2) = Parser.parseInput(inputFile);
            var crosses = getCrosses(wire1, wire2);
            var minDist = int.MaxValue;
            var origin = new Point(0, 0);

            foreach (var cross in crosses) {
                var dist = GeoFuncs.manDist(origin, cross);
                if (dist < minDist) {
                    minDist = dist;
                }
            }

            return minDist;
        }
    }
}
