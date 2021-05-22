using System.Collections.Generic;

namespace aoc.y2019.day8
{
    class Parser
    {
        private static (int, Layer) parseLayer(string line, int ndx, int width, int height) {
            var rows = new List<string>();

            for (var row = 0; row < height; row += 1) {
                rows.Add(line.Substring(ndx, width));
                ndx += width;
            }

            return (ndx, new Layer(rows));
        }

        private static List<Layer> parseLayers(string line, int width, int height) {
            var ndx = 0;
            var layers = new List<Layer>();

            while (ndx < line.Length) {
                var (newNdx, layer) = parseLayer(line, ndx, width, height);

                layers.Add(layer);
                ndx = newNdx;
            }

            return layers;
        }

        public static List<Layer> parseInput(string fileName, int width, int height) {
            var lines = aoc.utils.Parser.readLines(fileName);

            return parseLayers(lines[0], width, height);
        }
    }
}