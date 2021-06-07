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
        public int allMasks = 0;

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

    class GraphWalker
    {
        record Node(Point3d pt, int dist);

        private Grid grid;
        private Dictionary<char, Dictionary<Point, GraphNode>> graph;
        private Dictionary<Point3d, int> graph3d = new Dictionary<Point3d, int>();
        private Dictionary<Point3d, int> toVisit = new Dictionary<Point3d, int>();
        private Dictionary<Point3d, bool> marked = new Dictionary<Point3d, bool>();
        private int? pathDist;

        public GraphWalker(Grid grid, Dictionary<char, Dictionary<Point, GraphNode>> graph) {
            this.grid = grid;
            this.graph = graph;
        }

        private Point3d findClosest() {
            Point3d? closest = null;
            var dist = int.MaxValue;

            foreach (var pt in toVisit) {
                var ptDist = graph3d[pt.Key];

                if (ptDist < dist) {
                    dist = ptDist;
                    closest = pt.Key;
                }
            }

            if (closest is null) {
                throw new System.Exception("Could not find closest");
            }

            return closest;
        }

        private void doRound() {
            var closest = findClosest();
            var dist = graph3d[closest];
            var pt2d = new Point(closest.x, closest.y);
            var ch = pt2d.Equals(grid.entrance) ? '@' : grid.keys[pt2d].ch;
            var candidates = graph[ch];

            if (closest.z == grid.allMasks) {
                pathDist = dist;
                return;
            }

            toVisit.Remove(closest);
            marked[closest] = true;

            foreach (var entry in candidates) {
                var node = entry.Value;

                if ((closest.z == 0 && node.needKeys == 0) || (closest.z & node.needKeys) == node.needKeys) {
                    var keyItem = (Key)node.item;
                    var keyMask = closest.z | grid.masks[keyItem.ch];
                    var pt3d = new Point3d(node.pt.x, node.pt.y, keyMask);

                    if (marked.ContainsKey(pt3d)) {
                        continue;
                    }

                    var newDist = dist + node.dist;
                    var old = graph3d.ContainsKey(pt3d) ? graph3d[pt3d] : int.MaxValue;
                    if (newDist < old) {
                        graph3d[pt3d] = newDist;
                    }

                    toVisit[pt3d] = graph3d[pt3d];
                }
            }
        }

        public int walk() {
            if (grid.entrance is null) {
                return 0;
            }

            var start = grid.entrance;
            var pt = new Point3d(start.x, start.y, 0);

            graph3d[pt] = 0;
            toVisit[pt] = 0;

            while (pathDist is null) {
                doRound();
            }

            return (int)pathDist;
        }
    }

    class GridCrawler
    {
        private Grid grid;

        class Crawler
        {
            private Grid grid;
            private Dictionary<Point, bool> seen = new Dictionary<Point, bool>();
            private List<Point> toVisit = new List<Point>();

            public int keys { get; }

            public Crawler(Grid grid, Point start) : this(grid, start, 0) { }

            public Crawler(Grid grid, Point start, int keys) {
                this.grid = grid;
                this.keys = keys;

                toVisit.Add(start);
            }

            private void visit(List<Crawler> newCrawlers, List<Point> toVisit, Point pt) {
                if (seen.ContainsKey(pt)) {
                    return;
                }

                if (grid.spaces.ContainsKey(pt)) {
                    toVisit.Add(pt);
                } else if (grid.keys.ContainsKey(pt)) {
                    var key = grid.keys[pt];
                    var mask = grid.masks[key.ch];

                    newCrawlers.Add(new Crawler(grid, pt, keys | mask));
                } else if (grid.doors.ContainsKey(pt)) {
                    var door = grid.doors[pt];
                    var mask = grid.masks[door.ch];

                    if ((mask & keys) != 0) {
                        toVisit.Add(pt);
                    }
                }
            }

            public List<Crawler> step() {
                var newCrawlers = new List<Crawler>();
                var newVisits = new List<Point>();

                foreach (var pt in toVisit) {
                    seen[pt] = true;

                    visit(newCrawlers, newVisits, pt);
                    visit(newCrawlers, newVisits, new Point(pt.x, pt.y - 1));
                    visit(newCrawlers, newVisits, new Point(pt.x, pt.y + 1));
                    visit(newCrawlers, newVisits, new Point(pt.x + 1, pt.y));
                    visit(newCrawlers, newVisits, new Point(pt.x - 1, pt.y));
                }

                if (newVisits.Count > 0) {
                    newCrawlers.Add(this);
                }

                toVisit = newVisits;

                return newCrawlers;
            }
        }

        public GridCrawler(Grid grid) {
            this.grid = grid;
        }

        public int crawl() {
            if (grid.entrance is null) {
                throw new System.Exception("No entrance");
            }

            var crawlers = new List<Crawler>() { new Crawler(grid, grid.entrance) };
            var dist = 0;
            var pathDist = 0;

            while (pathDist == 0) {
                var next = new List<Crawler>();

                foreach (var crawler in crawlers) {
                    if (crawler.keys == grid.allMasks) {
                        pathDist = dist;
                    }
                    var newCrawlers = crawler.step();
                    next.AddRange(newCrawlers);
                }

                dist += 1;
                crawlers = next;
            }

            return pathDist;
        }
    }
}

