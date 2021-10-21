using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day20
{
    class DistWalker
    {
        private Grid grid;
        private string startItem;
        private int dist;
        private List<Point> toVisit;
        private HashSet<Point> seen;
        private Dictionary<string, int> dists;

        public DistWalker(Grid grid, string startItem) {
            this.grid = grid;
            this.startItem = startItem;
            this.seen = new HashSet<Point>();
            this.dists = new Dictionary<string, int>();

            this.toVisit = new List<Point>();
            this.toVisit.Add(grid.jumps[startItem]);
        }

        private void visitItem(List<Point> next, int x, int y) {
            var item = grid.items[y, x];
            var pt = new Point(x, y);

            if (seen.Contains(pt)) {
                return;
            }

            if (item is Maze) {
                next.Add(pt);
            } else if (item is OuterJump) {
                var jump = item as OuterJump;

                if (jump is not null) {
                    dists[jump.label] = dist + 1;
                }
            } else if (item is InnerJump) {
                var jump = item as InnerJump;

                if (jump is not null) {
                    dists[jump.label] = dist + 1;
                }
            }
        }

        private void visit(List<Point> next, Point pt) {
            visitItem(next, pt.x, pt.y - 1);
            visitItem(next, pt.x, pt.y + 1);
            visitItem(next, pt.x + 1, pt.y);
            visitItem(next, pt.x - 1, pt.y);

            seen.Add(pt);
        }

        public Dictionary<string, int> walk() {
            dist = 0;

            while (toVisit.Count > 0) {
                var next = new List<Point>();

                foreach (var pt in toVisit) {
                    visit(next, pt);
                }

                dist += 1;
                toVisit = next;
            }

            return dists;
        }
    }

    class DistMapper
    {
        private Grid grid;

        public DistMapper(Grid grid) {
            this.grid = grid;
        }

        public Dictionary<string, Dictionary<string, int>> map() {
            var distMap = new Dictionary<string, Dictionary<string, int>>();
            var jumps = grid.jumps;

            foreach (var entry in jumps) {
                var walker = new DistWalker(grid, entry.Key);

                distMap.Add(entry.Key, walker.walk());
            }

            foreach (var entry in distMap["UX"]) {
                System.Console.WriteLine($"{entry.Key}: {entry.Value}");
            }

            return distMap;
        }
    }
}