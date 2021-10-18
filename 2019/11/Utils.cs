using System.Collections.Generic;
using aoc.y2019.intcode;
using aoc.utils.geometry;

namespace aoc.y2019.day11
{
    enum Direction { UP, DOWN, RIGHT, LEFT }

    enum Color
    {
        BLACK, WHITE
    }

    class Helpers
    {
        public static void printPanels(Dictionary<Point, Color> panels) {
            var (min, max) = findRange(panels);

            System.Console.WriteLine($"{min} {max}");

            for (var y = min.y; y <= max.y; y += 1) {
                for (var x = min.x; x <= max.x; x += 1) {
                    var pt = new Point(x, y);
                    var color = panels.ContainsKey(pt) ? panels[pt] : Color.BLACK;

                    printColor(color);
                }

                System.Console.WriteLine();
            }
        }

        private static void printColor(Color c) {
            if (c == Color.BLACK) {
                System.Console.Write(" ");
            } else if (c == Color.WHITE) {
                System.Console.Write("#");
            }
        }

        private static (Point, Point) findRange(Dictionary<Point, Color> panels) {
            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var maxX = int.MinValue;
            var maxY = int.MinValue;

            foreach (var entry in panels) {
                var pt = entry.Key;

                if (pt.x < minX) { minX = pt.x; }
                if (pt.x > maxX) { maxX = pt.x; }
                if (pt.y < minY) { minY = pt.y; }
                if (pt.y > maxY) { maxY = pt.y; }
            }

            return (new Point(minX, minY), new Point(maxX, maxY));
        }
    }

    class Robot : IOHandler
    {
        private Machine mach;
        private Direction dir;
        private Point loc;
        private bool outputColor;
        public Dictionary<Point, Color> panels { get; }

        public Robot(long[] prog, Color initialColor) {
            mach = new Machine(prog, this);
            panels = new Dictionary<Point, Color>();
            loc = new Point(0, 0);

            panels[loc] = initialColor;
            dir = Direction.UP;
            outputColor = true;
        }

        public void run() {
            while (!mach.isHalted()) {
                mach.step();
            }
        }

        public long input() {
            var color = panels.ContainsKey(loc) ? panels[loc] : Color.BLACK;

            return color switch
            {
                Color.BLACK => 0,
                Color.WHITE => 1,
                _ => throw new System.Exception($"Invalid color {color}")
            };
        }

        public void output(long value) {
            if (outputColor) {
                panels[loc] = value switch
                {
                    0 => Color.BLACK,
                    1 => Color.WHITE,
                    _ => throw new System.Exception($"Invalid color in output {value}")
                };

                outputColor = false;
                return;
            }

            if (value == 0) {
                turnLeft();
            } else if (value == 1) {
                turnRight();
            } else {
                throw new System.Exception($"Invalid turn {value}");
            }

            forward();
            outputColor = true;
        }

        private void forward() {
            var diff = dir switch
            {
                Direction.UP => new Point(0, -1),
                Direction.DOWN => new Point(0, 1),
                Direction.RIGHT => new Point(1, 0),
                Direction.LEFT => new Point(-1, 0),
                _ => new Point(0, 0),
            };

            loc = new Point(loc.x + diff.x, loc.y + diff.y);
        }

        private void turnLeft() {
            dir = dir switch
            {
                Direction.UP => Direction.LEFT,
                Direction.LEFT => Direction.DOWN,
                Direction.DOWN => Direction.RIGHT,
                Direction.RIGHT => Direction.UP,
                _ => throw new System.Exception($"Invalid direction {dir}")
            };
        }

        private void turnRight() {
            dir = dir switch
            {
                Direction.UP => Direction.RIGHT,
                Direction.RIGHT => Direction.DOWN,
                Direction.DOWN => Direction.LEFT,
                Direction.LEFT => Direction.UP,
                _ => throw new System.Exception($"Invalid direction {dir}")
            };
        }
    }
}