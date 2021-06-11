using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day18
{
    interface GridItem { }

    record Space : GridItem;
    record Enter : GridItem;
    record Key(char ch) : GridItem;
    record Door(char ch) : GridItem;

    class Grid
    {
        public Dictionary<Point, Enter> entrances { get; } = new Dictionary<Point, Enter>();
        public Dictionary<Point, Key> keys { get; } = new Dictionary<Point, Key>();
        public Dictionary<Point, Door> doors { get; } = new Dictionary<Point, Door>();
        public Dictionary<Point, Space> spaces { get; } = new Dictionary<Point, Space>();
        public Dictionary<char, int> masks = new Dictionary<char, int>();
        public Dictionary<Point, int> ptMasks = new Dictionary<Point, int>();
        public int allMasks = 0;
        public int allPtMasks = 0;

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

        private int keyMaskForDoor(char ch) {
            var target = System.Char.ToLower(ch);

            foreach (var entry in grid.keys) {
                var keyPt = entry.Key;
                var key = entry.Value;

                if (key.ch == target) {
                    return grid.ptMasks[keyPt];
                }
            }

            return 0;
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
                var ptMask = grid.ptMasks[pt];

                var newNode = new GraphNode(pt, dist, node.needKeys, node.hasKeys | mask, key);

                if ((mask & node.needKeys) == 0) {
                    nodes[pt] = newNode;
                }

                toVisit.Add(newNode);
            } else if (grid.doors.ContainsKey(pt)) {
                var door = grid.doors[pt];
                var mask = grid.masks[door.ch];
                var keyMask = keyMaskForDoor(door.ch);

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

    class DfsWalker
    {
        private Grid grid;
        private Dictionary<Point, Dictionary<Point, GraphNode>> graph;
        private Dictionary<char, Dictionary<int, int>> below = new Dictionary<char, Dictionary<int, int>>();
        private int pathDist = int.MaxValue;

        public DfsWalker(Grid grid, Dictionary<Point, Dictionary<Point, GraphNode>> graph) {
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
            var candidates = new Dictionary<char, GraphNode>();
            var added = new Dictionary<char, int>();

            foreach (var entry in nodes) {
                var pt = entry.Key;
                var node = entry.Value;

                if (!alreadyHaveKey(pt, foundKeys) && haveNeededKey(node.needKeys, foundKeys)) {
                    var keyItem = (Key)node.item;

                    if (!added.ContainsKey(keyItem.ch)) {
                        added[keyItem.ch] = 0;
                    }

                    foreach (var maskEntry in grid.masks) {
                        var ch = maskEntry.Key;
                        var mask = maskEntry.Value;

                        if (added.ContainsKey(ch) && (mask & node.hasKeys) != 0 && ch >= 'a' && ch <= 'z') {
                            added[ch] += 1;
                        }
                    }

                    candidates[keyItem.ch] = node;
                }
            }

            foreach (var entry in added) {
                var ch = entry.Key;
                var count = entry.Value;

                if (count > 1) {
                    candidates.Remove(ch);
                }
            }

            return new List<GraphNode>(candidates.Values);
        }

        private int walk(List<GraphNode> candidates, int keys) {
            // if (keys == grid.allMasks) {
            if (keys == grid.allPtMasks) {
                return 0;
            }

            var minDist = int.MaxValue;

            foreach (var candidate in candidates) {
                var key = grid.keys[candidate.pt];
                var nextNodes = graph[candidate.pt];
                var newKeys = keys | candidate.hasKeys;

                if (!below.ContainsKey(key.ch)) {
                    below[key.ch] = new Dictionary<int, int>();
                }

                if (below[key.ch].ContainsKey(keys)) {
                    var dist = below[key.ch][keys] + candidate.dist;

                    if (dist < minDist) {
                        minDist = dist;
                    }

                    continue;
                }

                var nextCandidates = getCandidates(nextNodes, newKeys);

                var belowDist = walk(nextCandidates, newKeys);
                below[key.ch][keys] = belowDist;

                var newDist = belowDist + candidate.dist;
                if (newDist < minDist) {
                    minDist = newDist;
                }
            }

            return minDist;
        }

        public int walk() {
            var allCandidates = new List<GraphNode>();

            foreach (var entry in grid.entrances) {
                var nodes = graph[entry.Key];
                var candidates = getCandidates(nodes, 0);

                allCandidates.AddRange(candidates);
            }

            pathDist = walk(allCandidates, 0);

            // foreach (var entry in grid.entrances) {
            //     pathDist = walk(graph[entry.Key], 0);
            // }

            return pathDist;
        }
    }
}

