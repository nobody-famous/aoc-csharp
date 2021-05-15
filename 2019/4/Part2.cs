namespace aoc.y2019.day4
{
    class Part2 : Solver
    {
        public Part2(string file, int exp) : base(file, exp) { }

        protected override bool isValid(int[] value) {
            var count = 1;

            for (var ndx = 1; ndx < value.Length; ndx += 1) {
                if (value[ndx - 1] == value[ndx]) {
                    count += 1;
                } else if (count == 2) {
                    return true;
                } else {
                    count = 1;
                }
            }

            return count == 2;
        }
    }
}