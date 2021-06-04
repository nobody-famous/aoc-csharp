using System.Collections.Generic;
using aoc.intcode;
using aoc.utils.geometry;

namespace aoc.y2019.day17
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        private (Point, Point) getBounds(Dictionary<Point, long> scaffold) {
            int xMin = int.MaxValue;
            int xMax = int.MinValue;
            int yMin = int.MaxValue;
            int yMax = int.MinValue;

            foreach (var entry in scaffold) {
                var pt = entry.Key;

                if (pt.x < xMin) { xMin = pt.x; }
                if (pt.x > xMax) { xMax = pt.x; }
                if (pt.y < yMin) { yMin = pt.y; }
                if (pt.y > yMax) { yMax = pt.y; }
            }

            return (new Point(xMin, yMin), new Point(xMax, yMax));
        }

        private void printScaffold(Robot robot) {
            var (low, high) = getBounds(robot.scaffold);
            for (var row = low.y; row <= high.y; row += 1) {
                for (var col = low.x; col <= high.x; col += 1) {
                    var pt = new Point(col, row);
                    var ch = robot.scaffold.ContainsKey(pt) ? robot.scaffold[pt] : Cell.SPACE;

                    if (pt.Equals(robot.loc)) {
                        ch = (char)robot.dir;
                    }

                    System.Console.Write((char)ch);
                }
                System.Console.WriteLine();
            }
        }

        private bool isIntersection(Dictionary<Point, long> scaffold, Point pt) {
            return scaffold.ContainsKey(pt)
            && scaffold.ContainsKey(new Point(pt.x, pt.y + 1))
            && scaffold.ContainsKey(new Point(pt.x, pt.y - 1))
            && scaffold.ContainsKey(new Point(pt.x + 1, pt.y))
            && scaffold.ContainsKey(new Point(pt.x - 1, pt.y));
        }

        private List<Point> findIntersections(Robot robot) {
            var intersections = new List<Point>();
            var bounds = getBounds(robot.scaffold);

            foreach (var entry in robot.scaffold) {
                var pt = entry.Key;

                if (isIntersection(robot.scaffold, pt)) {
                    intersections.Add(pt);
                }
            }

            return intersections;
        }

        protected override int doWork() {
            var prog = Parser.parseInput(inputFile);
            var robot = new Robot(prog);

            robot.run();

            var intersections = findIntersections(robot);
            var total = 0;

            foreach (var pt in intersections) {
                total += pt.x * pt.y;
            }

            return total;
        }
    }
}
