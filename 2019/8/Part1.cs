using System.Collections.Generic;

namespace aoc.y2019.day8
{
    class Counts
    {
        public int zeros { get; set; }
        public int ones { get; set; }
        public int twos { get; set; }

        public Counts() {
            zeros = 0;
            ones = 0;
            twos = 0;
        }
    }

    class Part1 : aoc.utils.ProblemSolver<int>
    {
        private int width;
        private int height;

        public Part1(string file, int exp, int width, int height) : base(file, exp) {
            this.width = width;
            this.height = height;
        }

        private Counts getCounts(Layer layer) {
            var counts = new Counts();

            foreach (var row in layer.data) {
                foreach (var ch in row) {
                    if (ch == '0') { counts.zeros += 1; }
                    if (ch == '1') { counts.ones += 1; }
                    if (ch == '2') { counts.twos += 1; }
                }
            }

            return counts;
        }

        private (Layer, Counts) findFewest(List<Layer> layers) {
            var minLayer = layers[0];
            var min = getCounts(minLayer);

            for (var ndx = 1; ndx < layers.Count; ndx += 1) {
                var layer = layers[ndx];
                var counts = getCounts(layer);

                if (counts.zeros < min.zeros) {
                    min = counts;
                    minLayer = layer;
                }
            }

            return (minLayer, min);
        }

        protected override int doWork() {
            var layers = Parser.parseInput(inputFile, width, height);
            var (layer, counts) = findFewest(layers);

            return counts.ones * counts.twos;
        }
    }
}