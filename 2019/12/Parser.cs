using System.Collections.Generic;
using System.Text.RegularExpressions;
using aoc.utils.geometry;

namespace aoc.y2019.day12
{
    class Parser
    {
        public static List<Moon> parseInput(string fileName) {
            var lines = aoc.utils.Parser.readLines(fileName);
            var moons = new List<Moon>();

            foreach (var line in lines) {
                var pt = parsePoint(line);
                moons.Add(new Moon(pt));
            }

            return moons;
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
