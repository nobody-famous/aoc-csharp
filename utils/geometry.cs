using System;

namespace aoc.utils.geometry
{
    record Point(int x, int y);

    record Line(Point start, Point end);

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
    }

    class Funcs
    {
        public static int manDist(Point pt1, Point pt2) {
            return Math.Abs(pt2.x - pt1.x) + Math.Abs(pt2.y - pt1.y);
        }
    }
}
