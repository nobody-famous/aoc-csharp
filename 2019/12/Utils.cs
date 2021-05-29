using aoc.utils.geometry;

namespace aoc.y2019.day12
{
    class Moon
    {
        public Point3d pos { get; }

        public Point3d vel { get; }

        public Moon(Point3d position) {
            this.pos = position;
            this.vel = new Point3d(0, 0, 0);
        }
    }
}