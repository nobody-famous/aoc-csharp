using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day10
{
    class Part2 : Solver
    {
        public Part2(string file, int exp) : base(file, exp) { }

        private List<List<Point>> sortQuad(Point origin, Dictionary<float, List<Point>> grp) {
            var slopes = new List<float>(grp.Keys);
            var sorted = new List<List<Point>>();

            slopes.Sort();

            foreach (var slope in slopes) {
                grp[slope].Sort((pt1, pt2) =>
                {
                    var dist1 = Funcs.manDist(origin, pt1);
                    var dist2 = Funcs.manDist(origin, pt2);

                    return dist1 < dist2 ? -1 : 1;
                });

                sorted.Add(grp[slope]);
            }

            return sorted;
        }

        private List<List<Point>> sortPoints(Visible vis) {
            var sorted = new List<List<Point>>();

            sorted.AddRange(sortQuad(vis.pt, vis.grps.ne));
            sorted.AddRange(sortQuad(vis.pt, vis.grps.se));
            sorted.AddRange(sortQuad(vis.pt, vis.grps.sw));
            sorted.AddRange(sortQuad(vis.pt, vis.grps.nw));

            return sorted;
        }

        protected override int doWork() {
            var asteroids = Parser.parseInput(inputFile);
            var vis = mostVisible(asteroids);
            var sorted = sortPoints(vis);

            var ndx = 0;
            Point? point = null;

            for (var count = 0; count < 200; count += 1) {
                while (sorted[ndx].Count == 0) {
                    ndx = (ndx + 1) % sorted.Count;
                }

                var slope = sorted[ndx];
                point = slope[0];
                slope.RemoveAt(0);
                ndx = (ndx + 1) % sorted.Count;
            }


            return (point is Point p) ? p.x * 100 + p.y : 0;
        }
    }
}
