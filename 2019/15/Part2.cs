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

        private (Point, Point) getGridRange() {
            var xMin = int.MaxValue;
            var yMin = int.MaxValue;
            var xMax = int.MinValue;
            var yMax = int.MinValue;

            foreach (var (point, status) in visited) {
                if (point.x < xMin) {
                    xMin = point.x;
                }

                if (point.x > xMax) {
                    xMax = point.x;
                }

                if (point.y < yMin) {
                    yMin = point.y;
                }

                if (point.y > yMax) {
                    yMax = point.y;
                }
            }

            return (new Point(xMin, yMin), new Point(xMax, yMax));
        }

        private void printGrid(Point low, Point high) {
            for (var y = low.y; y <= high.y; y += 1) {
                for (var x = low.x; x <= high.x; x += 1) {
                    var pt = new Point(x, y);
                    var ch = visited.ContainsKey(pt) ? visited[new Point(x, y)] switch
                    {
                        Status.Wall => '#',
                        Status.Empty => ' ',
                        Status.System => '@',
                        _ => '?',
                    } : '#';

                    System.Console.Write(ch);
                }

                System.Console.WriteLine();
            }
        }

        protected override int doWork() {
            var prog = aoc.intcode.Parser.parseInput(inputFile);

            buildGrid(prog);

            var (low, high) = getGridRange();

            printGrid(low, high);

            return 0;
        }

    }
}