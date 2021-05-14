using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day3
{
    class Parser : aoc.utils.Parser
    {
        private static Point strToDiff(string str) {
            var dirChar = str[0];
            var distStr = str.Substring(1);
            var dist = int.Parse(distStr);

            switch (dirChar) {
            case 'U': return new Point(0, dist);
            case 'D': return new Point(0, -dist);
            case 'R': return new Point(dist, 0);
            case 'L': return new Point(-dist, 0);
            default:
                throw new System.Exception($"Invalid direction {dirChar}");
            }
        }

        private static List<Line> pointsToLines(Point[] pts) {
            var curPoint = new Point(0, 0);
            var lines = new List<Line>();

            foreach (var pt in pts) {
                var nextPoint = new Point(curPoint.x + pt.x, curPoint.y + pt.y);

                lines.Add(new Line(curPoint, nextPoint));
                curPoint = nextPoint;
            }

            return lines;
        }

        private static List<Line> parseWire(string wire) {
            var steps = wire.Split(',');
            var pts = System.Array.ConvertAll(steps, s => strToDiff(s));

            return pointsToLines(pts);
        }

        public static (List<Line>, List<Line>) parseInput(string file) {
            var lines = readLines(file);
            var wire1 = parseWire(lines[0]);
            var wire2 = parseWire(lines[1]);

            return (wire1, wire2);
        }
    }
}