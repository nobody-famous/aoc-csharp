using System.Collections.Generic;

namespace aoc.y2019.day6
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        private void setCount(Dictionary<string, int> counts, string name, int value) {
            var count = counts.ContainsKey(name) ? counts[name] : 0;

            counts[name] = value;
        }

        private void addKidCounts(Dictionary<string, List<string>> map, Dictionary<string, int> counts, string name) {
            var kids = map.ContainsKey(name) ? map[name] : new List<string>();
            var count = counts.ContainsKey(name) ? counts[name] : 0;

            foreach (var kid in kids) {
                setCount(counts, kid, count + 1);
                addKidCounts(map, counts, kid);
            }
        }

        protected override int doWork() {
            var map = Parser.parseInput(inputFile);
            var counts = new Dictionary<string, int>();

            addKidCounts(map, counts, "COM");

            var total = 0;
            foreach (var entry in counts) {
                total += entry.Value;
            }

            return total;
        }
    }
}
