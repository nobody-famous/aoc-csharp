using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day20
{
    class Part2 : Solver<Point3d>
    {
        public Part2(string file, int exp) : base(file, exp) { }

        protected override List<Point3d> findCandidates(Grid grid, Dictionary<Point3d, bool> seen, Point3d start, Point3d end) {
            var candidates = new List<Point3d>();
            var toCheck = new List<Point3d>()
            {
                new Point3d(start.x, start.y - 1, start.z),
                new Point3d(start.x, start.y + 1, start.z),
                new Point3d(start.x + 1, start.y, start.z),
                new Point3d(start.x - 1, start.y, start.z),
            };

            if (grid.items[start.y, start.x] is OuterJump && start.z != 0) {
                var jump = (OuterJump)grid.items[start.y, start.x];
                toCheck.Add(new Point3d(jump.pt.x, jump.pt.y, start.z - 1));
            } else if (grid.items[start.y, start.x] is InnerJump) {
                var jump = (InnerJump)grid.items[start.y, start.x];
                toCheck.Add(new Point3d(jump.pt.x, jump.pt.y, start.z + 1));
            }

            foreach (var pt in toCheck) {
                if (pt.Equals(end)) {
                    return new List<Point3d>() { pt };
                }

                if (seen.ContainsKey(pt)) {
                    continue;
                }

                if (isOpen(grid, new Point(pt.x, pt.y))) {
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

            return findPath(grid, new Point3d(startItem.x, startItem.y, 0), new Point3d(endItem.x, endItem.y, 0));
        }
    }
}
