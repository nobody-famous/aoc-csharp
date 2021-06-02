using System.Collections.Generic;

namespace aoc.y2019.day16
{
    class Part1 : aoc.utils.ProblemSolver<long>
    {
        private int[] pattern = new int[] { 0, 1, 0, -1 };

        public Part1(string file, long exp) : base(file, exp) { }

        private int applyPattern(List<int> input, int count) {
            var patNdx = 1;
            var inpNdx = count - 1;
            var total = 0;

            while (inpNdx < input.Count) {
                var mult = pattern[patNdx];

                for (var ndx = 0; inpNdx < input.Count && ndx < count; ndx += 1) {
                    total += input[inpNdx] * mult;
                    inpNdx += 1;
                }

                inpNdx += count;
                patNdx = (patNdx + 2) % pattern.Length;
            }

            return System.Math.Abs(total) % 10;
        }

        private void applyPattern(List<int> input) {
            for (var ndx = 0; ndx < input.Count; ndx += 1) {
                input[ndx] = applyPattern(input, ndx + 1);
            }
        }

        protected override long doWork() {
            var input = Parser.parseInput(inputFile);

            for (var loop = 0; loop < 100; loop += 1) {
                applyPattern(input);
            }

            var str = "";
            for (var ndx = 0; ndx < 8; ndx += 1) {
                str += input[ndx];
            }

            return long.Parse(str);
        }
    }
}
