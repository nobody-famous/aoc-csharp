using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day15
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        private Dictionary<Point, bool> visited = new Dictionary<Point, bool>();
        private Point? oxygenSystem;

        public Part1(string file, int exp) : base(file, exp) { }

        private void visit(List<Droid> neighbors, Droid droid, Point point, Direction dir) {
            if (visited.ContainsKey(point)) {
                return;
            }

            var copy = new Droid(droid);
            var result = copy.move(dir);

            visited[point] = true;

            if (result == Status.System) {
                oxygenSystem = point;
            }

            if (result == Status.Moved) {
                neighbors.Add(copy);
            }
        }

        private List<Droid> visitNeighbors(Droid droid) {
            var neighbors = new List<Droid>();

            var north = new Point(droid.loc.x, droid.loc.y + 1);
            var south = new Point(droid.loc.x, droid.loc.y - 1);
            var east = new Point(droid.loc.x + 1, droid.loc.y);
            var west = new Point(droid.loc.x - 1, droid.loc.y);

            visit(neighbors, droid, north, Direction.North);
            visit(neighbors, droid, south, Direction.South);
            visit(neighbors, droid, east, Direction.East);
            visit(neighbors, droid, west, Direction.West);

            return neighbors;
        }

        protected override int doWork() {
            var prog = aoc.intcode.Parser.parseInput(inputFile);
            var droids = new List<Droid>() { new Droid(prog) };
            var nxtDroids = new List<Droid>();
            var dist = 0;

            visited[droids[0].loc] = true;

            while (oxygenSystem is null) {
                foreach (var droid in droids) {
                    var next = visitNeighbors(droid);
                    nxtDroids.AddRange(next);
                }

                dist += 1;

                droids = nxtDroids;
                nxtDroids = new List<Droid>();
            }

            return dist;
        }
    }
}
