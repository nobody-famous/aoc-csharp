using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace aoc.y2019.day14
{
    class Parser
    {
        private static List<Chemical> splitList(string list) {
            var chems = new List<Chemical>();
            var re = new Regex(@"(\d+) ([A-Za-z]+),?");
            var matches = re.Matches(list);

            foreach (Match match in matches) {
                var groups = match.Groups;
                var amount = int.Parse(groups[1].Value);
                var name = groups[2].Value;

                chems.Add(new Chemical(name, amount));
            }

            return chems;
        }

        private static void addReaction(Dictionary<string, Reaction> reacts, string line) {
            var re = new Regex(@"(.*) => (\d+) (\S+)");
            var match = re.Match(line);
            var groups = match.Groups;

            var input = splitList(groups[1].Value);
            var amount = int.Parse(groups[2].Value);
            var chem = groups[3].Value;

            reacts.Add(chem, new Reaction(chem, amount, input));
        }

        public static Dictionary<string, Reaction> parseInput(string fileName) {
            var lines = aoc.utils.Parser.readLines(fileName);
            var reacts = new Dictionary<string, Reaction>();

            foreach (var line in lines) {
                addReaction(reacts, line);
            }

            return reacts;
        }
    }
}