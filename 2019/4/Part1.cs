namespace aoc.y2019.day4
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        private void findFirst(int[] start) {
            var ndx = 0;

            while (ndx < start.Length - 1 && start[ndx] <= start[ndx + 1]) {
                ndx += 1;
            }

            var ch = start[ndx];
            while (ndx < start.Length) {
                start[ndx] = ch;
                ndx += 1;
            }
        }

        private void increment(int[] value) {
            var ndx = value.Length - 1;

            while (ndx >= 0) {
                value[ndx] += 1;

                if (value[ndx] < 10) {
                    break;
                } else {
                    value[ndx] = 0;
                }

                ndx -= 1;
            }
        }

        protected override int doWork() {
            var (start, end) = Parser.parseInput(inputFile);

            findFirst(start);
            increment(start);

            foreach (var ch in start) {
                System.Console.Write($"{ch}");
            }
            System.Console.WriteLine();

            throw new System.NotImplementedException();
        }
    }
}