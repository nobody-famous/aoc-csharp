using System;

namespace aoc.intcode
{
    class Parser
    {
        public static long[] parseInput(string fileName) {
            var lines = aoc.utils.Parser.readLines(fileName);
            var line = lines[0];
            var ints = line.Split(',');

            return Array.ConvertAll(ints, n => long.Parse(n));
        }
    }
}