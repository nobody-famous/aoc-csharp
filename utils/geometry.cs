using System;
namespace aoc.utils
{
    record Point(int x, int y);

    record Line(Point start, Point end);

    class GeoFuncs
    {
        public static int manDist(Point pt1, Point pt2) {
            return Math.Abs(pt2.x - pt1.x) + Math.Abs(pt2.y - pt1.y);
        }
    }
}
