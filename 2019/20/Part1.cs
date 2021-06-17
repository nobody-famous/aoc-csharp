using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day20
{
    class Part1 : aoc.utils.ProblemSolver<int>
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

        private List<Point> findCandidates(Grid grid, Dictionary<Point, bool> seen, Point start, Point end) {
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

        private int findPath(Grid grid, Point start, Point end) {
            var seen = new Dictionary<Point, bool>();
            var candidates = new List<Point>() { start };
            var done = false;
            var dist = 0;

            seen[start] = true;
            while (!done) {
                var nextCandidates = new List<Point>();

                foreach (var pt in candidates) {
                    if (pt.Equals(end)) {
                        done = true;
                    } else {
                        var pts = findCandidates(grid, seen, pt, end);

                        nextCandidates.AddRange(pts);
                    }
                }

                if (!done) {
                    dist += 1;
                }

                foreach (var pt in nextCandidates) {
                    seen[pt] = true;
                }

                candidates = nextCandidates;
            }

            return dist;
        }

        protected override int doWork() {
            var parser = new Parser(inputFile);
            var grid = parser.parseInput();
            var startItem = grid.jumps["AA"];
            var endItem = grid.jumps["ZZ"];

            if (startItem is null || endItem is null) {
                throw new System.Exception("Could not find start or end point");
            }

            if (startItem.Count != 1 || endItem.Count != 1) {
                throw new System.Exception("End points have too many jumps");
            }

            return findPath(grid, startItem[0], endItem[0]);
        }
    }
}
