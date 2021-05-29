using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day12
{
    class Moon
    {
        public Point3d pos { get; set; }

        public Point3d vel { get; set; }

        public Moon(Point3d position) {
            this.pos = position;
            this.vel = new Point3d(0, 0, 0);
        }

        public Moon(Moon copy) {
            pos = new Point3d(copy.pos.x, copy.pos.y, copy.pos.z);
            vel = new Point3d(copy.vel.x, copy.vel.y, copy.vel.z);
        }

        public override string ToString() {
            return $"pos=<{pos.x},{pos.y},{pos.z}>,vel=<{vel.x},{vel.y},{vel.z}>";
        }
    }

    abstract class Solver<T> : aoc.utils.ProblemSolver<T>
    {
        public Solver(string file, T exp) : base(file, exp) { }

        protected void updateX(Moon first, Moon second) {
            if (first.pos.x < second.pos.x) {
                first.vel = new Point3d(first.vel.x + 1, first.vel.y, first.vel.z);
                second.vel = new Point3d(second.vel.x - 1, second.vel.y, second.vel.z);
            } else if (first.pos.x > second.pos.x) {
                first.vel = new Point3d(first.vel.x - 1, first.vel.y, first.vel.z);
                second.vel = new Point3d(second.vel.x + 1, second.vel.y, second.vel.z);
            }
        }

        protected void updateY(Moon first, Moon second) {
            if (first.pos.y < second.pos.y) {
                first.vel = new Point3d(first.vel.x, first.vel.y + 1, first.vel.z);
                second.vel = new Point3d(second.vel.x, second.vel.y - 1, second.vel.z);
            } else if (first.pos.y > second.pos.y) {
                first.vel = new Point3d(first.vel.x, first.vel.y - 1, first.vel.z);
                second.vel = new Point3d(second.vel.x, second.vel.y + 1, second.vel.z);
            }
        }

        protected void updateZ(Moon first, Moon second) {
            if (first.pos.z < second.pos.z) {
                first.vel = new Point3d(first.vel.x, first.vel.y, first.vel.z + 1);
                second.vel = new Point3d(second.vel.x, second.vel.y, second.vel.z - 1);
            } else if (first.pos.z > second.pos.z) {
                first.vel = new Point3d(first.vel.x, first.vel.y, first.vel.z - 1);
                second.vel = new Point3d(second.vel.x, second.vel.y, second.vel.z + 1);
            }
        }

        protected void applyGravity(List<Moon> moons) {
            var pairs = aoc.utils.Helpers.getPairs(moons);

            foreach (var pair in pairs) {
                var first = pair[0];
                var second = pair[1];

                updateX(first, second);
                updateY(first, second);
                updateZ(first, second);
            }
        }

        protected void applyVelocity(List<Moon> moons) {
            foreach (var moon in moons) {
                moon.pos = new Point3d(moon.pos.x + moon.vel.x, moon.pos.y + moon.vel.y, moon.pos.z + moon.vel.z);
            }
        }

        protected void runLoop(List<Moon> moons) {
            applyGravity(moons);
            applyVelocity(moons);
        }
    }
}
