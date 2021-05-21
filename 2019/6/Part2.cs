using System.Collections.Generic;
using System.Threading.Tasks;

namespace aoc.y2019.day6
{
    class Part2 : aoc.utils.ProblemSolver<int>
    {
        public Part2(string file, int exp) : base(file, exp) { }

        private List<string>? findPath(Dictionary<string, List<string>> map, string start, string end) {
            if (start == end) {
                return new List<string>() { end };
            } else if (!map.ContainsKey(start)) {
                return null;
            }

            var entry = map[start];

            foreach (var kid in entry) {
                var sub = findPath(map, kid, end);

                if (sub is List<string> p) {
                    var path = new List<string>() { start };

                    path.AddRange(p);

                    return path;
                }
            }

            return null;
        }

        private int getCommonIndex(List<string> path1, List<string> path2) {
            var ndx = 0;

            while (path1[ndx] == path2[ndx]) {
                ndx += 1;
            }

            return ndx + 1;
        }

        protected override int doWork() {
            var map = Parser.parseInput(inputFile);
            var youTask = Task.Run<List<string>?>(() => findPath(map, "COM", "YOU"));
            var santaTask = Task.Run<List<string>?>(() => findPath(map, "COM", "SAN"));

            var youPath = youTask.Result;
            var santaPath = santaTask.Result;

            if (youPath == null || santaPath == null) { return 0; }

            var ndx = getCommonIndex(youPath, santaPath);
            var youLength = youPath.Count - ndx;
            var santaLength = santaPath.Count - ndx;

            return youLength + santaLength;
        }
    }
}