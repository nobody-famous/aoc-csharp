using System.Collections.Generic;
using aoc.intcode;
using aoc.utils.geometry;

namespace aoc.y2019.day11
{
    enum Direction { UP, DOWN, RIGHT, LEFT }

    enum Color
    {
        BLACK, WHITE
    }

    class Robot : Listener
    {
        private Machine mach;
        private Direction dir;
        private Dictionary<Point, Color> panels;
        private Point loc;
        private bool outputColor;

        public Robot(long[] prog) {
            mach = new Machine(prog, this);
            panels = new Dictionary<Point, Color>();
            loc = new Point(0, 0);

            panels[loc] = Color.BLACK;
            dir = Direction.UP;
            outputColor = true;
        }

        public void run() {
            while (!mach.isHalted()) {
                mach.step();
            }
        }

        public int numPanels { get { return panels.Count; } }

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
                Direction.UP => new Point(0, 1),
                Direction.DOWN => new Point(0, -1),
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

    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        protected override int doWork() {
            var prog = Parser.parseInput(inputFile);
            var robot = new Robot(prog);

            robot.run();

            return robot.numPanels;
        }
    }
}
