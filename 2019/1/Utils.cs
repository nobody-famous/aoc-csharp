using aoc.utils;

namespace aoc.y2019.day1
{
    abstract class Solver : Problem
    {
        protected string inputFile;
        protected int expected;

        protected abstract int doWork();

        public void run()
        {
            var answer = doWork();

            if (this.expected != answer)
            {
                throw new System.Exception($"{answer} != {this.expected}");
            }
        }

        protected Solver(string inputFile, int expected)
        {
            this.inputFile = inputFile;
            this.expected = expected;
        }

        protected int findFuel(int mass)
        {
            return (mass / 3) - 2;
        }
    }
}