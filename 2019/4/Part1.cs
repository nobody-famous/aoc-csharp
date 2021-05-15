namespace aoc.y2019.day4
{
    class Part1 : Solver
    {
        public Part1(string file, int exp) : base(file, exp) { }


        protected override bool isValid(int[] value) {
            for (var ndx = 1; ndx < value.Length; ndx += 1) {
                if (value[ndx - 1] == value[ndx]) {
                    return true;
                }
            }

            return false;
        }
    }
}