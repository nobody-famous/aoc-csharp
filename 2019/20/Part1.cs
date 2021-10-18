using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day20
{
    class Part1 : Solver<Point>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        protected override List<Point> findCandidates(Grid grid, Dictionary<Point, bool> seen, Point start, Point end) {
            var candidates = new List<Point>();
            var toCheck = new List<Point>() {
                new Point(start.x, start.y - 1),
                new Point(start.x, start.y + 1),
                new Point(start.x + 1, start.y),
                new Point(start.x - 1, start.y)
            };

            if (grid.items[start.y, start.x] is OuterJump) {
                var jump = (OuterJump)grid.items[start.y, start.x];

                toCheck.Add(jump.pt);
            } else if (grid.items[start.y, start.x] is InnerJump) {
                var jump = (InnerJump)grid.items[start.y, start.x];

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
