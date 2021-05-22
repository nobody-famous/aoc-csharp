using System.Collections.Generic;
using System.Threading.Tasks;
using aoc.utils.geometry;

namespace aoc.y2019.day10
{
    class Part1 : Solver
    {
        public Part1(string file, int exp) : base(file, exp) { }

        protected override int doWork() {
            var asteroids = Parser.parseInput(inputFile);
            var most = mostVisible(asteroids);

            return most.count;
        }
    }
}
