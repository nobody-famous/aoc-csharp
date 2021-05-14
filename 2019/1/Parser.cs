using System;

namespace aoc.y2019.day1
{
    class Parser
    {
        public static int[] parseInput(string fileName) {
            var lines = aoc.utils.Parser.readLines(fileName);

            return Array.ConvertAll(lines, line => int.Parse(line));
        }
    }
}