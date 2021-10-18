using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day15
{
    class Part2 : Solver
    {
        public Part2(string file, int exp) : base(file, exp) { }

        private void buildGrid(long[] prog) {
            var droids = new List<Droid>() { new Droid(prog) };
            var nxtDroids = new List<Droid>();

            visited[droids[0].loc] = Status.Empty;

            while (droids.Count > 0) {
                foreach (var droid in droids) {
                    var next = visitNeighbors(droid);
                    nxtDroids.AddRange(next);
                }

                droids = nxtDroids;
                nxtDroids = new List<Droid>();
            }
        }

        private void visit(Dictionary<Point, bool> seen, List<Point> neighbors, Point point) {
            if (seen.ContainsKey(point)) {
                return;
            }

            if (visited.ContainsKey(point) && visited[point] == Status.Empty) {
                neighbors.Add(point);
            }
        }

        private List<Point> visitNeighbors(Dictionary<Point, bool> seen, Point point) {
            var neighbors = new List<Point>();

            seen[point] = true;

            visit(seen, neighbors, new Point(point.x, point.y + 1));
            visit(seen, neighbors, new Point(point.x, point.y - 1));
            visit(seen, neighbors, new Point(point.x + 1, point.y));
            visit(seen, neighbors, new Point(point.x - 1, point.y));

            return neighbors;
        }

        private List<Point> visitPoints(Dictionary<Point, bool> seen, List<Point> points) {
            var nextPoints = new List<Point>();

            foreach (var point in points) {
                var toVisit = visitNeighbors(seen, point);
                nextPoints.AddRange(toVisit);
            }

            return nextPoints;
        }

        private int fillGrid(Point start) {
            var count = 0;
            var seen = new Dictionary<Point, bool>();
            var points = visitPoints(seen, new List<Point>() { start });

            seen[start] = true;

            while (points.Count > 0) {
                count += 1;
                points = visitPoints(seen, points);
            }

            return count;
        }

        protected override int doWork() {
            var prog = aoc.y2019.intcode.Parser.parseInput(inputFile);

            buildGrid(prog);

            return fillGrid(oxygenSystem!);
        }

    }
}