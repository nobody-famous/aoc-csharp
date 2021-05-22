using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day10
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        private float getSlope(Point p1, Point p2) {
            var rise = p2.y - p1.y;
            var run = p2.x - p1.x;

            return (float)rise / (float)run;
        }

        private Dictionary<Point, float> getSlopes(Point orig, List<Point> all) {
            var slopes = new Dictionary<Point, float>();

            foreach (var pt in all) {
                if (pt != orig) {
                    slopes.Add(pt, getSlope(orig, pt));
                }
            }

            return slopes;
        }

        private int countVisible(Point pt, List<Point> all) {
            var slopes = getSlopes(pt, all);

            foreach (var entry in slopes) {
                System.Console.WriteLine($"{entry.Key} {entry.Value}");
            }

            return 0;
        }

        protected override int doWork() {
            var asteroids = Parser.parseInput(inputFile);

            countVisible(asteroids[0], asteroids);

            throw new System.NotImplementedException();
        }
    }
}
