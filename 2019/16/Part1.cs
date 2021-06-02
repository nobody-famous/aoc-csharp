using System.Collections.Generic;

namespace aoc.y2019.day16
{
    class Part1 : aoc.utils.ProblemSolver<long>
    {
        private int[] pattern = new int[] { 0, 1, 0, -1 };
        private int[] sumTable = new int[] { 0 };

        public Part1(string file, long exp) : base(file, exp) { }

        private int[] buildSumTable(int[] input) {
            var table = new int[input.Length];

            table[0] = input[0];
            for (var ndx = 1; ndx < input.Length; ndx += 1) {
                table[ndx] = table[ndx - 1] + input[ndx];
            }

            return table;
        }

        private int applyPattern(int[] input, int count) {
            var patNdx = 1;
            var inpNdx = count - 1;
            var total = 0;

            while (inpNdx < input.Length) {
                if (pattern[patNdx] == 0) {
                    inpNdx += count;
                    patNdx = (patNdx + 1) % pattern.Length;
                    continue;
                }

                var mult = pattern[patNdx];
                var endNdx = System.Math.Min(inpNdx + count - 1, input.Length - 1);
                var toSub = inpNdx > 0 ? sumTable[inpNdx - 1] : 0;

                total += (sumTable[endNdx] - toSub) * mult;

                inpNdx += count;
                patNdx = (patNdx + 1) % pattern.Length;
            }

            return System.Math.Abs(total) % 10;
        }

        private void applyPattern(int[] input) {
            for (var ndx = 0; ndx < input.Length; ndx += 1) {
                input[ndx] = applyPattern(input, ndx + 1);
            }
        }

        protected override long doWork() {
            var input = Parser.parseInput(inputFile);


            for (var loop = 0; loop < 100; loop += 1) {
                sumTable = buildSumTable(input);
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
