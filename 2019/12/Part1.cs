using System.Collections.Generic;

namespace aoc.y2019.day12
{
    class Part1 : Solver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }


        private int energy(Moon moon) {
            var potential = System.Math.Abs(moon.pos.x)
            + System.Math.Abs(moon.pos.y)
            + System.Math.Abs(moon.pos.z);

            var kinetic = System.Math.Abs(moon.vel.x)
            + System.Math.Abs(moon.vel.y)
            + System.Math.Abs(moon.vel.z);

            return potential * kinetic;
        }

        private int energy(List<Moon> moons) {
            var total = 0;

            foreach (var moon in moons) {
                total += energy(moon);
            }

            return total;
        }

        protected override int doWork() {
            var moons = Parser.parseInput(inputFile);

            for (var loop = 0; loop < 1000; loop += 1) {
                runLoop(moons);
            }

            return energy(moons);
        }
    }
}
