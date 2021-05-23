using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day10
{
    class Parser
    {
        private static void parseRow(int rowNum, string row, List<Point> points) {
            for (var col = 0; col < row.Length; col += 1) {
                if (row[col] == '#') {
                    points.Add(new Point(col, rowNum));
                }
            }
        }

        public static List<Point> parseInput(string fileName) {
            var lines = aoc.utils.Parser.readLines(fileName);
            var points = new List<Point>();

            for (var row = 0; row < lines.Length; row += 1) {
                parseRow(row, lines[row], points);
            }

            return points;
        }
    }
}