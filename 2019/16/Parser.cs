using System.Collections.Generic;

namespace aoc.y2019.day16
{
    class Parser
    {
        public static List<int> parseInput(string file) {
            var lines = aoc.utils.Parser.readLines(file);
            var line = lines[0];
            var input = new List<int>();

            foreach (var ch in line) {
                input.Add(ch - '0');
            }

            return input;
        }
    }
}