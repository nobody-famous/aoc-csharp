using System.Collections.Generic;

namespace aoc.y2019.day14
{
    class Chemical
    {
        public string name { get; set; }
        public int amount { get; set; }

        public Chemical(string name, int amount) {
            this.name = name;
            this.amount = amount;
        }
    }

    record Reaction(string name, int amount, List<Chemical> input);
}