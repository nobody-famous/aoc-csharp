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

    record GraphNode(Point pt, int dist, int needKeys, int hasKeys, GridItem item);

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

        private void visitPoint(List<GraphNode> toVisit, Point pt, GraphNode node) {
            if (seen.ContainsKey(pt)) {
                return;
            }

            seen[pt] = true;

            if (grid.spaces.ContainsKey(pt)) {
                toVisit.Add(new GraphNode(pt, dist, node.needKeys, node.hasKeys, grid.spaces[pt]));
            } else if (grid.keys.ContainsKey(pt)) {
                var key = grid.keys[pt];
                var mask = grid.masks[key.ch];
                var newNode = new GraphNode(pt, dist, node.needKeys, node.hasKeys | mask, key);

                if ((mask & node.needKeys) == 0) {
                    nodes[pt] = newNode;
                }

                toVisit.Add(newNode);
            } else if (grid.doors.ContainsKey(pt)) {
                var door = grid.doors[pt];
                var mask = grid.masks[door.ch];

                toVisit.Add(new GraphNode(pt, dist, node.needKeys | mask, node.hasKeys, door));
            }
        }

        private List<GraphNode> visit(GraphNode node) {
            var points = new List<GraphNode>();

            visitPoint(points, new Point(node.pt.x, node.pt.y - 1), node);
            visitPoint(points, new Point(node.pt.x, node.pt.y + 1), node);
            visitPoint(points, new Point(node.pt.x + 1, node.pt.y), node);
            visitPoint(points, new Point(node.pt.x - 1, node.pt.y), node);

            return points;
        }

        public Dictionary<Point, GraphNode> build() {
            seen[start] = true;

            var needKeys = (grid.keys.ContainsKey(start))
                ? grid.masks[grid.keys[start].ch]
                : 0;

            var toVisit = new List<GraphNode>() { new GraphNode(start, 0, needKeys, 0, new Space()) };

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
                var nodeKey = grid.keys[node.pt];
                var nodeMask = grid.masks[nodeKey.ch];

                if ((nodeMask & closest.z) != 0) {
                    continue;
                }

                if ((closest.z == 0 && node.needKeys == 0) || (closest.z & node.needKeys) == node.needKeys) {
                    var keyItem = (Key)node.item;
                    var keyMask = closest.z | node.hasKeys;
                    var pt3d = new Point3d(node.pt.x, node.pt.y, keyMask);

                    if (marked.ContainsKey(pt3d)) {
                        continue;
                    }

                    var newDist = dist + node.dist;
                    var oldDist = graph3d.ContainsKey(pt3d) ? graph3d[pt3d] : int.MaxValue;
                    if (newDist < oldDist) {
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

    class DfsWalker
    {
        private Grid grid;
        private Dictionary<char, Dictionary<Point, GraphNode>> graph;
        private Dictionary<int, int> seen = new Dictionary<int, int>();
        private int pathDist = int.MaxValue;

        public DfsWalker(Grid grid, Dictionary<char, Dictionary<Point, GraphNode>> graph) {
            this.grid = grid;
            this.graph = graph;
        }

        private bool haveNeededKey(int needKeys, int foundKeys) {
            return (needKeys & foundKeys) == needKeys;
        }

        private bool alreadyHaveKey(Point point, int foundKeys) {
            var key = grid.keys[point];
            var mask = grid.masks[key.ch];

            return (mask & foundKeys) == mask;
        }

        private List<GraphNode> getCandidates(Dictionary<Point, GraphNode> nodes, int foundKeys) {
            var candidates = new List<GraphNode>();

            foreach (var entry in nodes) {
                var pt = entry.Key;
                var node = entry.Value;

                if (!alreadyHaveKey(pt, foundKeys) && haveNeededKey(node.needKeys, foundKeys)) {
                    candidates.Add(node);
                }
            }

            return candidates;
        }

        private void walk(Dictionary<Point, GraphNode> nodes, int dist, int keys) {
            // System.Console.WriteLine($"walk {dist} {keys}");

            if (seen.ContainsKey(keys) && seen[keys] < dist) {
                // System.Console.WriteLine($"  SEEN {keys} {grid.allMasks} {seen[keys]} < {dist}");
                dist = seen[keys];
            }

            // if (dist >= pathDist || (seen.ContainsKey(keys) && seen[keys] < dist)) {
            if (dist >= pathDist) {
                return;
            }

            if (keys == grid.allMasks) {
                if (dist < pathDist) {
                    pathDist = dist;
                }

                return;
            }

            seen[keys] = dist;
            var candidates = getCandidates(nodes, keys);

            foreach (var candidate in candidates) {
                var key = grid.keys[candidate.pt];
                var nextNodes = graph[key.ch];
                var newKeys = keys | candidate.hasKeys;
                var newDist = dist + candidate.dist;

                // System.Console.WriteLine($"  {key.ch} {keys}");
                walk(nextNodes, newDist, keys | candidate.hasKeys);
            }
        }

        public int walk() {
            if (grid.entrance is null) {
                return 0;
            }

            walk(graph['@'], 0, 0);

            return pathDist;
        }
    }
}

