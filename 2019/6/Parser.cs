using System.Collections.Generic;

namespace aoc.y2019.day6
{
    class Parser
    {
        private static void addToMap(Dictionary<string, List<string>> map, string line) {
            var pieces = line.Split(')');
            var parent = pieces[0];
            var child = pieces[1];

            if (!map.ContainsKey(parent)) {
                map.Add(parent, new List<string>());
            }

            var kids = map[parent];

            kids.Add(child);
        }

        public static Dictionary<string, List<string>> parseInput(string fileName) {
            var lines = aoc.utils.Parser.readLines(fileName);
            var dict = new Dictionary<string, List<string>>();

            foreach (var line in lines) {
                addToMap(dict, line);
            }

            return dict;
        }
    }
}