using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day3
{
    abstract class Solver : aoc.utils.ProblemSolver<int>
    {
        protected Solver(string file, int exp) : base(file, exp) { }

        protected abstract override int doWork();
        protected bool inRange(int start, int end, int value) {
            return (value >= start && value <= end) || (value >= end && value <= start);
        }

        protected bool pointOnLine(Point pt, Line line) {
            if (line.start.x == line.end.x) {
                return pt.x == line.start.x && inRange(line.start.y, line.end.y, pt.y);
            } else if (line.start.y == line.end.y) {
                return pt.y == line.start.y && inRange(line.start.x, line.end.x, pt.x);
            } else {
                throw new System.NotImplementedException();
            }
        }

        protected Point? getCrossPoint(Line line1, Line line2) {
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

            return point;
        }

        protected List<Point> getCrosses(List<Line> wire1, List<Line> wire2) {
            var crosses = new List<Point>();

            foreach (var line1 in wire1) {
                foreach (var line2 in wire2) {
                    var pt = getCrossPoint(line1, line2);

                    if (pt is Point p && p.x != 0 && p.y != 0) {
                        crosses.Add(p);
                    }
                }
            }

            return crosses;
        }
    }
}