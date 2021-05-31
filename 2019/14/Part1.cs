using System.Collections.Generic;

namespace aoc.y2019.day14
{
    class Part1 : Solver
    {
        public Part1(string file, long exp) : base(file, exp) { }

        protected override long doWork() {
            var reacts = Parser.parseInput(inputFile);

            return computeOre(reacts);
        }
    }
}
