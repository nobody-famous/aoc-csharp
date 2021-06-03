using System.Collections.Generic;

namespace aoc.y2019.day16
{
    class Part1 : Solver
    {
        public Part1(string file, long exp) : base(file, exp) { }

        protected override long doWork() {
            var input = Parser.parseInput(inputFile);

            for (var loop = 0; loop < 100; loop += 1) {
                applyPattern(input);
            }

            return getValue(input, 8);
        }
    }
}
