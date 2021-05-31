using System.Collections.Generic;

namespace aoc.y2019.day14
{
    class Chemical
    {
        public string name { get; set; }
        public long amount { get; set; }

        public Chemical(string name, long amount) {
            this.name = name;
            this.amount = amount;
        }
    }

    record Reaction(string name, long amount, List<Chemical> input);

    abstract class Solver : aoc.utils.ProblemSolver<long>
    {
        public Solver(string file, long exp) : base(file, exp) { }

        protected Dictionary<string, long> totalNeeded = new Dictionary<string, long>();
        protected Dictionary<string, long> pile = new Dictionary<string, long>();
        protected Dictionary<string, bool> fromOre = new Dictionary<string, bool>();

        protected void reset() {
            totalNeeded = new Dictionary<string, long>();
            pile = new Dictionary<string, long>();
            fromOre = new Dictionary<string, bool>();
        }

        protected long computeOre(Dictionary<string, Reaction> reacts) {
            return computeOre(reacts, 1);
        }

        protected long computeOre(Dictionary<string, Reaction> reacts, long amount) {
            reset();
            getFromOre(reacts);
            totalNeeded["FUEL"] = amount;

            while (!allOre()) {
                doRound(reacts);
            }

            var sum = 0L;
            foreach (var entry in totalNeeded) {
                var react = reacts[entry.Key];
                var ore = react.input[0];
                var (mult, rem) = getQuantity(entry.Value, react.amount);

                sum += mult * ore.amount;
            }

            return sum;
        }

        protected (long, long) getQuantity(long target, long need) {
            if (target % need == 0) {
                return (target / need, 0);
            }

            var mult = (target / need) + 1;
            var rem = (need * mult) - target;

            return (mult, rem);
        }

        protected void addToEntry(Dictionary<string, long> dict, string name, long amount) {
            if (!dict.ContainsKey(name)) {
                dict[name] = 0;
            }

            dict[name] += amount;
        }

        protected List<Chemical> getNeeded(Reaction react, long target) {
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

        protected void checkPile(List<Chemical> needed) {
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

        protected void procNeeded(Dictionary<string, Reaction> reacts, List<Chemical> needed) {
            checkPile(needed);

            foreach (var chem in needed) {
                addToEntry(totalNeeded, chem.name, chem.amount);
            }
        }

        protected void doRound(Dictionary<string, Reaction> reacts) {
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

        protected void getFromOre(Dictionary<string, Reaction> reacts) {
            foreach (var entry in reacts) {
                var input = entry.Value.input;

                if (input.Count == 1 && input[0].name == "ORE") {
                    fromOre[entry.Key] = true;
                }
            }
        }

        protected bool allOre() {
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
    }
}
