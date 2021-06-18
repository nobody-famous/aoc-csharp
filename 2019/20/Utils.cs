using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day20
{
    abstract class Solver<T> : aoc.utils.ProblemSolver<int> where T : notnull
    {
        public Solver(string file, int exp) : base(file, exp) { }

        protected abstract List<T> findCandidates(Grid grid, Dictionary<T, bool> seen, T pt, T end);

        protected int findPath(Grid grid, T start, T end) {
            var seen = new Dictionary<T, bool>();
            var candidates = new List<T>() { start };
            var done = false;
            var dist = 0;

            seen[start] = true;
            while (!done) {
                var nextCandidates = new List<T>();

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
    }
}