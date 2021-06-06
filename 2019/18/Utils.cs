using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day18
{
    interface GridItem { }

    record Space : GridItem;
    record Key(char ch) : GridItem;
    record Door(char ch) : GridItem;

    class Grid
    {
        public Point? entrance { get; set; } = null;
        public Dictionary<Point, Key> keys { get; } = new Dictionary<Point, Key>();
        public Dictionary<Point, Door> doors { get; } = new Dictionary<Point, Door>();
        public Dictionary<Point, Space> spaces { get; } = new Dictionary<Point, Space>();
        public Dictionary<char, int> masks = new Dictionary<char, int>();

        public Grid() {
            var mask = 1;

            for (var ch = (int)'A'; ch <= (int)'Z'; ch += 1) {
                var upper = (char)ch;
                var lower = System.Char.ToLower(upper);

                masks[upper] = mask;
                masks[lower] = mask;

                mask = mask << 1;
            }
        }
    }

    record GraphNode(Point pt, int dist, int needKeys, GridItem item);

    class GraphBuilder
    {
        private Grid grid;
        private Point start;
        private int dist = 0;
        private Dictionary<Point, GraphNode> nodes = new Dictionary<Point, GraphNode>();
        private Dictionary<Point, bool> seen = new Dictionary<Point, bool>();

        public GraphBuilder(Grid grid, Point start) {
            this.grid = grid;
            this.start = start;
        }

        private void visitPoint(List<GraphNode> toVisit, Point pt, int needKeys) {
            if (seen.ContainsKey(pt)) {
                return;
            }

            seen[pt] = true;

            if (grid.spaces.ContainsKey(pt)) {
                toVisit.Add(new GraphNode(pt, dist, needKeys, grid.spaces[pt]));
            } else if (grid.keys.ContainsKey(pt)) {
                var key = grid.keys[pt];
                var mask = grid.masks[key.ch];
                var node = new GraphNode(pt, dist, needKeys, key);

                if ((mask & needKeys) == 0) {
                    nodes[pt] = node;
                }

                toVisit.Add(node);
            } else if (grid.doors.ContainsKey(pt)) {
                var door = grid.doors[pt];
                var mask = grid.masks[door.ch];

                toVisit.Add(new GraphNode(pt, dist, needKeys | mask, door));
            }
        }

        private List<GraphNode> visit(GraphNode node) {
            var points = new List<GraphNode>();

            visitPoint(points, new Point(node.pt.x, node.pt.y - 1), node.needKeys);
            visitPoint(points, new Point(node.pt.x, node.pt.y + 1), node.needKeys);
            visitPoint(points, new Point(node.pt.x + 1, node.pt.y), node.needKeys);
            visitPoint(points, new Point(node.pt.x - 1, node.pt.y), node.needKeys);

            return points;
        }

        public Dictionary<Point, GraphNode> build() {
            seen[start] = true;

            var needKeys = (grid.keys.ContainsKey(start))
                ? grid.masks[grid.keys[start].ch]
                : 0;

            var toVisit = new List<GraphNode>() { new GraphNode(start, 0, needKeys, new Space()) };

            while (toVisit.Count > 0) {
                var neighbors = new List<GraphNode>();

                dist += 1;
                foreach (var node in toVisit) {
                    var points = visit(node);

                    neighbors.AddRange(points);
                }

                toVisit = neighbors;
            }

            return nodes;
        }
    }
}