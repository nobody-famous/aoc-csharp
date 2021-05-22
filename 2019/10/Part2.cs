using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day10
{
    class Part2 : Solver
    {
        public Part2(string file, int exp) : base(file, exp) { }

        private List<List<Point>> sortQuad(Dictionary<float, List<Point>> grp) {
            var slopes = new List<float>(grp.Keys);
            var sorted = new List<List<Point>>();

            slopes.Sort();

            foreach (var slope in slopes) {
                sorted.Add(grp[slope]);
            }

            return sorted;
        }

        private List<List<Point>> sortPoints(Visible vis) {
            var sorted = new List<List<Point>>();

            sorted.AddRange(sortQuad(vis.grps.ne));
            sorted.AddRange(sortQuad(vis.grps.se));
            sorted.AddRange(sortQuad(vis.grps.sw));
            sorted.AddRange(sortQuad(vis.grps.nw));

            return sorted;
        }

        protected override int doWork() {
            var asteroids = Parser.parseInput(inputFile);
            var vis = mostVisible(asteroids);
            var sorted = sortPoints(vis);

            foreach (var grp in sorted) {
                System.Console.WriteLine($"{grp[0]}");
            }

            return 0;
        }
    }
}
