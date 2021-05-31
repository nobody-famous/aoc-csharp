using System.Collections.Generic;

namespace aoc.y2019.day14
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        private Dictionary<string, int> totalNeeded = new Dictionary<string, int>();
        private Dictionary<string, int> pile = new Dictionary<string, int>();
        private Dictionary<string, bool> fromOre = new Dictionary<string, bool>();

        public Part1(string file, int exp) : base(file, exp) { }

        private (int, int) getQuantity(int target, int need) {
            if (target % need == 0) {
                return (target / need, 0);
            }

            var mult = (target / need) + 1;
            var rem = (need * mult) - target;

            return (mult, rem);
        }

        private void addToEntry(Dictionary<string, int> dict, string name, int amount) {
            if (!dict.ContainsKey(name)) {
                dict[name] = 0;
            }

            dict[name] += amount;
        }

        private List<Chemical> getNeeded(Reaction react, int target) {
            var chems = new List<Chemical>();
            var (mult, rem) = getQuantity(target, react.amount);

            if (rem > 0) {
                addToEntry(pile, react.name, rem);
            }

            foreach (var chem in react.input) {
                chems.Add(new Chemical(chem.name, chem.amount * mult));
            }

            return chems;
        }

        private void checkPile(List<Chemical> needed) {
            var toRemove = new List<Chemical>();

            foreach (var chem in needed) {
                if (!pile.ContainsKey(chem.name)) {
                    continue;
                }

                var amount = pile[chem.name];

                if (amount == chem.amount) {
                    pile.Remove(chem.name);
                    toRemove.Add(chem);
                } else if (amount < chem.amount) {
                    chem.amount -= amount;
                    pile.Remove(chem.name);
                } else {
                    pile[chem.name] -= chem.amount;
                    toRemove.Add(chem);
                }
            }

            foreach (var chem in toRemove) {
                needed.Remove(chem);
            }
        }

        private void procNeeded(Dictionary<string, Reaction> reacts, List<Chemical> needed) {
            checkPile(needed);

            foreach (var chem in needed) {
                addToEntry(totalNeeded, chem.name, chem.amount);
            }
        }

        private void doRound(Dictionary<string, Reaction> reacts) {
            var toRemove = new List<string>();
            var toAdd = new List<Chemical>();

            foreach (var entry in totalNeeded) {
                if (fromOre.ContainsKey(entry.Key)) {
                    continue;
                }

                toRemove.Add(entry.Key);

                var needed = getNeeded(reacts[entry.Key], entry.Value);
                toAdd.AddRange(needed);
            }

            foreach (var name in toRemove) {
                totalNeeded.Remove(name);
            }

            procNeeded(reacts, toAdd);
        }

        private void getFromOre(Dictionary<string, Reaction> reacts) {
            foreach (var entry in reacts) {
                var input = entry.Value.input;

                if (input.Count == 1 && input[0].name == "ORE") {
                    fromOre[entry.Key] = true;
                }
            }
        }

        private bool allOre() {
            if (totalNeeded.Count == 0) {
                return false;
            }

            foreach (var entry in totalNeeded) {
                if (!fromOre.ContainsKey(entry.Key)) {
                    return false;
                }
            }

            return true;
        }

        protected override int doWork() {
            var reacts = Parser.parseInput(inputFile);

            getFromOre(reacts);
            totalNeeded["FUEL"] = 1;

            while (!allOre()) {
                doRound(reacts);
            }

            var sum = 0;
            foreach (var entry in totalNeeded) {
                var react = reacts[entry.Key];
                var ore = react.input[0];
                var (mult, rem) = getQuantity(entry.Value, react.amount);

                sum += mult * ore.amount;
            }

            return sum;
        }
    }
}
