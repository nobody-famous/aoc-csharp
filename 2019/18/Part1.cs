using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day18
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        private Dictionary<char, Dictionary<Point, GraphNode>> graph = new Dictionary<char, Dictionary<Point, GraphNode>>();

        public Part1(string file, int exp) : base(file, exp) { }

        private void addToGraph(Grid grid, Point pt) {
            var builder = new GraphBuilder(grid, pt);
            var nodes = builder.build();

            if (pt.Equals(grid.entrance)) {
                graph['@'] = nodes;
            } else {
                var key = grid.keys[pt];

                graph[key.ch] = nodes;
            }
        }

        protected override int doWork() {
            var grid = Parser.parseInput(inputFile);

            if (grid.entrance is null) {
                throw new System.Exception("No entrance");
            }
            addToGraph(grid, grid.entrance);
            foreach (var entry in grid.keys) {
                addToGraph(grid, entry.Key);
            }

            var walker = new GraphWalker(grid, graph);
            return walker.walk();
        }
    }
}
