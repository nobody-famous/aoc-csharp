using System;

namespace aoc.utils.geometry
{
    record Line(Point start, Point end);

    class Point
    {
        public int x { get; set; }
        public int y { get; set; }

        public Point(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public override string ToString() {
            return $"({x},{y})";
        }
    }

    class Point3d
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }

        public Point3d(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString() {
            return $"({x},{y},{z})";
        }
    }

    class Funcs
    {
        public static int manDist(Point pt1, Point pt2) {
            return Math.Abs(pt2.x - pt1.x) + Math.Abs(pt2.y - pt1.y);
        }
    }
}
