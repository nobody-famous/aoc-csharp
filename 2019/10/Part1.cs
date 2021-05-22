using System.Collections.Generic;
using System.Threading.Tasks;
using aoc.utils.geometry;

namespace aoc.y2019.day10
{
    enum QUADRANT { NORTHEAST, SOUTHEAST, SOUTHWEST, NORTHWEST }

    class Groups
    {
        public Dictionary<float, List<Point>> ne { get; set; }
        public Dictionary<float, List<Point>> nw { get; set; }
        public Dictionary<float, List<Point>> se { get; set; }
        public Dictionary<float, List<Point>> sw { get; set; }

        public Groups() {
            ne = new Dictionary<float, List<Point>>();
            nw = new Dictionary<float, List<Point>>();
            se = new Dictionary<float, List<Point>>();
            sw = new Dictionary<float, List<Point>>();
        }

        private void addToList(Dictionary<float, List<Point>> dict, float slope, Point pt) {
            if (!dict.ContainsKey(slope)) {
                dict.Add(slope, new List<Point>());
            }

            var slopes = dict[slope];
            slopes.Add(pt);
        }

        public void Add(QUADRANT q, float slope, Point pt) {
            if (q == QUADRANT.NORTHEAST) addToList(ne, slope, pt);
            else if (q == QUADRANT.NORTHWEST) addToList(nw, slope, pt);
            else if (q == QUADRANT.SOUTHEAST) addToList(se, slope, pt);
            else if (q == QUADRANT.SOUTHWEST) addToList(sw, slope, pt);
        }
    }

    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        private float getSlope(Point p1, Point p2) {
            var rise = p2.y - p1.y;
            var run = p2.x - p1.x;

            return (float)rise / (float)run;
        }

        private QUADRANT getQuad(Point pt1, Point pt2) {
            if (pt2.x >= pt1.x && pt2.y < pt1.y) {
                return QUADRANT.NORTHEAST;
            } else if (pt2.x > pt1.x && pt2.y >= pt1.y) {
                return QUADRANT.SOUTHEAST;
            } else if (pt2.x <= pt1.x && pt2.y > pt1.y) {
                return QUADRANT.SOUTHWEST;
            } else if (pt2.x < pt1.x && pt2.y <= pt1.y) {
                return QUADRANT.NORTHWEST;
            }

            throw new System.Exception($"Could not determine quadrant {pt1} {pt2}");
        }

        private Groups getSlopes(Point orig, List<Point> all) {
            var groups = new Groups();

            foreach (var pt in all) {
                if (pt == orig) {
                    continue;
                }

                var slope = getSlope(orig, pt);
                var quad = getQuad(orig, pt);

                groups.Add(quad, slope, pt);
            }

            return groups;
        }

        private int countVisible(Point pt, List<Point> all) {
            var slopes = getSlopes(pt, all);
            var count = slopes.ne.Count + slopes.nw.Count + slopes.se.Count + slopes.sw.Count;

            return count;
        }

        private int mostVisible(List<Point> asteroids) {
            var tasks = new List<Task<int>>();

            foreach (var asteroid in asteroids) {
                tasks.Add(Task.Run<int>(() => countVisible(asteroid, asteroids)));
            }

            var most = 0;
            foreach (var task in tasks) {
                var res = task.Result;

                if (res > most) {
                    most = res;
                }
            }

            return most;
        }

        protected override int doWork() {
            var asteroids = Parser.parseInput(inputFile);

            return mostVisible(asteroids);
        }
    }
}
