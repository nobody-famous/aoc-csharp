using System.Collections.Generic;
using aoc.intcode;
using aoc.utils.geometry;

namespace aoc.y2019.day17
{
    interface Cell
    {
        const long SCAFFOLD = (long)'#';
        const long SPACE = (long)'.';
        const long ROBOT_UP = (long)'^';
        const long ROBOT_DOWN = (long)'v';
        const long ROBOT_LEFT = (long)'<';
        const long ROBOT_RIGHT = (long)'>';
        const long NEW_LINE = 10;
    }

    class Robot : IOHandler
    {
        private Machine machine;
        private int row = 0;
        private int col = 0;

        public long dir { get; set; } = Cell.ROBOT_UP;
        public Point loc { get; set; } = new Point(0, 0);
        public Dictionary<Point, long> scaffold { get; } = new Dictionary<Point, long>();

        public Robot(long[] prog) {
            this.machine = new Machine(prog, this);
        }

        public void run() {
            while (!machine.isHalted()) {
                machine.step();
            }
        }

        private bool isRobot(long cell) {
            switch (cell) {
            case Cell.ROBOT_UP:
            case Cell.ROBOT_DOWN:
            case Cell.ROBOT_RIGHT:
            case Cell.ROBOT_LEFT:
                return true;
            default:
                return false;
            }
        }
        public long input() { throw new System.NotImplementedException("input"); }

        public void output(long value) {
            if (value == Cell.SCAFFOLD) {
                scaffold[new Point(col, row)] = Cell.SCAFFOLD;
                col += 1;
            } else if (value == Cell.SPACE) {
                col += 1;
            } else if (isRobot(value)) {
                var pt = new Point(col, row);

                scaffold[pt] = Cell.SCAFFOLD;
                loc = pt;
                dir = value;

                col += 1;
            } else if (value == Cell.NEW_LINE) {
                row += 1;
                col = 0;
            } else {
                throw new System.Exception($"Unhandled output {value}");
            }
        }
    }
}
