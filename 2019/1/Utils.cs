using aoc.utils;

namespace aoc.y2019.day1
{
    abstract class Solver : ProblemSolver<int>
    {
        protected Solver(string fileName, int expected) : base(fileName, expected) { }

        protected int findFuel(int mass) {
            return (mass / 3) - 2;
        }
    }
}