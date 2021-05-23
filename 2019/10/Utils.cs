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
        public int Count
        {
            get => ne.Count + nw.Count + se.Count + sw.Count;
        }

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

    record Visible(Point pt, Groups grps, int count);

    abstract class Solver : aoc.utils.ProblemSolver<int>
    {
        protected Solver(string file, int exp) : base(file, exp) { }

        protected QUADRANT getQuad(Point pt1, Point pt2) {
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

        protected float getSlope(Point p1, Point p2) {
            var rise = p2.y - p1.y;
            var run = p2.x - p1.x;

            return (float)rise / (float)run;
        }

        protected Groups getSlopes(Point orig, List<Point> all) {
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

        protected int countVisible(Point pt, List<Point> all) {
            var slopes = getSlopes(pt, all);
            var count = slopes.ne.Count + slopes.nw.Count + slopes.se.Count + slopes.sw.Count;

            return count;
        }

        protected Visible getVisible(Point point, List<Point> all) {
            var slopes = getSlopes(point, all);
            return new Visible(point, slopes, slopes.Count);
        }

        protected Visible mostVisible(List<Point> asteroids) {
            var tasks = new List<Task<Visible>>();

            foreach (var asteroid in asteroids) {
                tasks.Add(Task.Run<Visible>(() => getVisible(asteroid, asteroids)));
            }

            var most = new Visible(new Point(0, 0), new Groups(), 0);
            foreach (var task in tasks) {
                var vis = task.Result;

                if (vis.count > most.count) {
                    most = vis;
                }
            }

            return most;
        }
    }
}
