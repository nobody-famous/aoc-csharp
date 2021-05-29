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

        public override string ToString() {
            return $"pos=<x={pos.x},y={pos.y},z={pos.z}>,vel=<x={vel.x},y={vel.y},z={vel.z}>";
        }
    }
}