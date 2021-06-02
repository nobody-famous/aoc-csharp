using System.Collections.Generic;

namespace aoc.y2019.day16
{
    class Parser
    {
        public static int[] parseInput(string file) {
            var lines = aoc.utils.Parser.readLines(file);
            var line = lines[0];
            // var input = new List<int>();
            var input = new int[line.Length];

            for (var ndx = 0; ndx < line.Length; ndx += 1) {
                input[ndx] = line[ndx] - '0';
            }

            // foreach (var ch in line) {
            //     input.Add(ch - '0');
            // }

            return input;
        }
    }
}