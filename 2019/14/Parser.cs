using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace aoc.y2019.day14
{
    class Parser
    {
        private static List<Chemical> splitList(string list) {
            var chems = new List<Chemical>();
            var re = new Regex(@"(\d+) (\S+),?");
            var matches = re.Matches(list);

            foreach (Match match in matches) {
                var groups = match.Groups;
                var amount = int.Parse(groups[1].Value);
                var name = groups[2].Value;

                chems.Add(new Chemical(amount, name));
            }

            return chems;
        }

        private static Reaction splitLine(string line) {
            var re = new Regex(@"(.*) => (\d+) (\S+)");
            var match = re.Match(line);
            var groups = match.Groups;
            var input = splitList(groups[1].Value);
            var output = new Chemical(int.Parse(groups[2].Value), groups[3].Value);

            return new Reaction(input, output);
        }

        public static List<Reaction> parseInput(string fileName) {
            var lines = aoc.utils.Parser.readLines(fileName);
            var reacts = new List<Reaction>();

            foreach (var line in lines) {
                var react = splitLine(line);
                reacts.Add(react);
            }

            return reacts;
        }
    }
}