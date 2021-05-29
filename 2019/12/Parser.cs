using System.Collections.Generic;
using System.Text.RegularExpressions;
using aoc.utils.geometry;

namespace aoc.y2019.day12
{
    class Parser
    {
        public static List<Point3d> parseInput(string fileName) {
            var lines = aoc.utils.Parser.readLines(fileName);
            var points = new List<Point3d>();

            foreach (var line in lines) {
                var pt = parsePoint(line);
                points.Add(pt);
            }

            return points;
        }

        private static Point3d parsePoint(string line) {
            var re = new Regex(@"<x=(-?\d+), y=(-?\d+), z=(-?\d+)>");
            var matches = re.Matches(line);
            var match = matches[0];
            var groups = match.Groups;

            return new Point3d(int.Parse(groups[1].Value), int.Parse(groups[2].Value), int.Parse(groups[3].Value));
        }
    }
}
