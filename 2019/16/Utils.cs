namespace aoc.y2019.day16
{
    abstract class Solver : aoc.utils.ProblemSolver<long>
    {
        protected int[] pattern = new int[] { 0, 1, 0, -1 };

        public Solver(string file, long exp) : base(file, exp) {
        }

        protected int[] buildSumTable(int[] input) {
            var table = new int[input.Length];

            table[0] = input[0];
            for (var ndx = 1; ndx < input.Length; ndx += 1) {
                table[ndx] = table[ndx - 1] + input[ndx];
            }

            return table;
        }

        protected int applyPattern(int[] sumTable, int[] input, int count) {
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

        protected void applyPattern(int[] input) {
            var sumTable = buildSumTable(input);

            for (var ndx = 0; ndx < input.Length; ndx += 1) {
                input[ndx] = applyPattern(sumTable, input, ndx + 1);
            }
        }

        protected long getValue(int[] input, int digits) {
            return getValue(input, digits, 0);
        }

        protected long getValue(int[] input, int digits, long offset) {
            var str = "";

            for (var ndx = offset; ndx < offset + digits; ndx += 1) {
                str += input[ndx];
            }

            return long.Parse(str);
        }
    }
}
