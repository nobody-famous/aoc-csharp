using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day20
{
    class Part1 : Solver<Point>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        private bool isMaze(Grid grid, Point pt) {
            var item = grid.items[pt.y, pt.x];

            return (item is Maze);
        }

        private bool isJump(Grid grid, Point pt) {
            var item = grid.items[pt.y, pt.x];

            return (item is Jump);
        }

        private bool isOpen(Grid grid, Point pt) {
            return isMaze(grid, pt) || isJump(grid, pt);
        }

        protected override List<Point> findCandidates(Grid grid, Dictionary<Point, bool> seen, Point start, Point end) {
            var candidates = new List<Point>();
            var toCheck = new List<Point>() {
                new Point(start.x, start.y - 1),
                new Point(start.x, start.y + 1),
                new Point(start.x + 1, start.y),
                new Point(start.x - 1, start.y)
            };

            if (isJump(grid, start)) {
                var jump = (Jump)grid.items[start.y, start.x];

                toCheck.Add(jump.pt);
            }

            foreach (var pt in toCheck) {
                if (pt.Equals(end)) {
                    return new List<Point>() { pt };
                }

                if (seen.ContainsKey(pt)) {
                    continue;
                }

                if (isOpen(grid, pt)) {
                    candidates.Add(pt);
                }
            }

            return candidates;
        }

        protected override int doWork() {
            var parser = new Parser(inputFile);
            var grid = parser.parseInput();
            var startItem = grid.outerJumps["AA"];
            var endItem = grid.outerJumps["ZZ"];

            if (startItem is null || endItem is null) {
                throw new System.Exception("Could not find start or end point");
            }

            return findPath(grid, startItem, endItem);
        }
    }
}
