using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day12
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        private void updateX(Moon first, Moon second) {
            if (first.pos.x < second.pos.x) {
                first.vel = new Point3d(first.vel.x + 1, first.vel.y, first.vel.z);
                second.vel = new Point3d(second.vel.x - 1, second.vel.y, second.vel.z);
            } else if (first.pos.x > second.pos.x) {
                first.vel = new Point3d(first.vel.x - 1, first.vel.y, first.vel.z);
                second.vel = new Point3d(second.vel.x + 1, second.vel.y, second.vel.z);
            }
        }

        private void updateY(Moon first, Moon second) {
            if (first.pos.y < second.pos.y) {
                first.vel = new Point3d(first.vel.x, first.vel.y + 1, first.vel.z);
                second.vel = new Point3d(second.vel.x, second.vel.y - 1, second.vel.z);
            } else if (first.pos.y > second.pos.y) {
                first.vel = new Point3d(first.vel.x, first.vel.y - 1, first.vel.z);
                second.vel = new Point3d(second.vel.x, second.vel.y + 1, second.vel.z);
            }
        }

        private void updateZ(Moon first, Moon second) {
            if (first.pos.z < second.pos.z) {
                first.vel = new Point3d(first.vel.x, first.vel.y, first.vel.z + 1);
                second.vel = new Point3d(second.vel.x, second.vel.y, second.vel.z - 1);
            } else if (first.pos.z > second.pos.z) {
                first.vel = new Point3d(first.vel.x, first.vel.y, first.vel.z - 1);
                second.vel = new Point3d(second.vel.x, second.vel.y, second.vel.z + 1);
            }
        }

        private void applyGravity(List<List<Moon>> pairs) {
            foreach (var pair in pairs) {
                var first = pair[0];
                var second = pair[1];

                updateX(first, second);
                updateY(first, second);
                updateZ(first, second);
            }
        }

        private void applyVelocity(List<Moon> moons) {
            foreach (var moon in moons) {
                moon.pos = new Point3d(moon.pos.x + moon.vel.x, moon.pos.y + moon.vel.y, moon.pos.z + moon.vel.z);
            }
        }

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
            var pairs = aoc.utils.Helpers.getPairs(moons);

            for (var loop = 0; loop < 1000; loop += 1) {
                applyGravity(pairs);
                applyVelocity(moons);
            }

            return energy(moons);
        }
    }
}
