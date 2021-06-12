using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day18
{
    class Part2 : aoc.utils.ProblemSolver<int>
    {
        private Dictionary<Point, Dictionary<Point, GraphNode>> graph = new Dictionary<Point, Dictionary<Point, GraphNode>>();

        public Part2(string file, int exp) : base(file, exp) { }

        private void addToGraph(Grid grid, Point pt) {
            var builder = new GraphBuilder(grid, pt);
            var nodes = builder.build();

            graph[pt] = nodes;
        }

        protected override int doWork() {
            var grid = Parser.parseInput(inputFile);

            foreach (var entry in grid.entrances) {
                addToGraph(grid, entry.Key);
            }

            foreach (var entry in grid.keys) {
                addToGraph(grid, entry.Key);
            }

            var walker = new DfsWalker(grid, graph);

            return walker.walk();
        }
    }
}
