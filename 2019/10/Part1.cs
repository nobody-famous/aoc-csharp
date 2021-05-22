using System.Collections.Generic;
using System.Threading.Tasks;
using aoc.utils.geometry;

namespace aoc.y2019.day10
{
    class Part1 : Solver
    {
        public Part1(string file, int exp) : base(file, exp) { }

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
