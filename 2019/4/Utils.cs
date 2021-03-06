namespace aoc.y2019.day4
{
    abstract class Solver : aoc.utils.ProblemSolver<int>
    {
        public Solver(string file, int exp) : base(file, exp) { }

        protected abstract bool isValid(int[] value);

        protected void findFirst(int[] start) {
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

        protected void increment(int[] value) {
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

            var v = value[ndx];
            while (ndx < value.Length) {
                value[ndx] = v;
                ndx += 1;
            }
        }

        protected bool lessThan(int[] v1, int[] v2) {
            for (var ndx = 0; ndx < v1.Length; ndx += 1) {
                if (v1[ndx] < v2[ndx]) {
                    return true;
                } else if (v1[ndx] > v2[ndx]) {
                    return false;
                }
            }

            return false;
        }

        protected void printArray(int[] value) {
            for (var ndx = 0; ndx < value.Length; ndx += 1) {
                System.Console.Write(value[ndx]);
            }

            System.Console.WriteLine();
        }

        protected override int doWork() {
            var (start, end) = Parser.parseInput(inputFile);

            findFirst(start);

            var count = 0;
            while (lessThan(start, end)) {
                increment(start);
                if (isValid(start)) {
                    count += 1;
                }
            }

            return count;
        }
    }
}