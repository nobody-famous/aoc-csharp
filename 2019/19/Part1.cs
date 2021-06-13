using aoc.intcode;
using aoc.utils.geometry;

namespace aoc.y2019.day19
{
    class Drone : IOHandler
    {
        private const int READY = 0;
        private const int SEND_X = 1;
        private const int SEND_Y = 2;
        private const int READ_STATUS = 3;

        private int state = READY;

        private int? x_input = null;
        private int? y_input = null;
        private long? status = null;

        private Machine machine;

        public Drone(long[] prog) {
            this.machine = new Machine(prog, this);
        }

        private int getInputX() {
            if (x_input is null) {
                throw new System.Exception("No X input");
            }

            return (int)x_input;
        }

        private int getInputY() {
            if (y_input is null) {
                throw new System.Exception("No Y input");
            }

            return (int)y_input;
        }

        public long input() {
            switch (state) {
            case SEND_X:
                state = SEND_Y;
                return getInputX();
            case SEND_Y:
                state = READ_STATUS;
                return getInputY();
            default: throw new System.Exception($"Unhandled state {state}");
            }
        }

        public void output(long value) {
            if (state != READ_STATUS) {
                throw new System.NotImplementedException($"Invalid output state {state}");
            }

            status = value;
        }

        public int sendCoords(int x, int y) {
            x_input = x;
            y_input = y;
            status = null;

            state = SEND_X;
            while (status is null) {
                machine.step();
            }

            return (int)status;
        }
    }

    class Part1 : aoc.utils.ProblemSolver<int>
    {
        private long[] prog = new long[0];
        private int size = 50;
        public Part1(string file, int exp) : base(file, exp) { }

        private bool checkPoint(Point pt) {
            if (!isValid(pt)) {
                return false;
            }

            var drone = new Drone(prog);
            var status = drone.sendCoords(pt.x, pt.y);

            return status == 1;
        }

        private Point findFirst() {
            Point? first = null;
            var x = 1;
            var y = 1;

            while (first is null) {
                var drone = new Drone(prog);
                var status = drone.sendCoords(x, y);

                var pt = new Point(x, y);
                if (checkPoint(pt)) {
                    first = pt;
                }

                y += 1;
                if (y >= (2 * x)) {
                    y = 0;
                    x = (x + 1) % size;
                }
            }

            return first;
        }

        private bool isValid(Point pt) {
            return pt.x < size && pt.y < size;
        }

        private int countRow(Point pt) {
            var count = 0;
            var walker = new Point(pt);

            while (checkPoint(walker)) {
                count += 1;
                walker.x -= 1;
            }

            walker = new Point(pt.x + 1, pt.y);
            while (checkPoint(walker)) {
                count += 1;
                walker.x += 1;
            }

            return count;
        }

        private void drawBeam() {
            for (var x = 0; x < size; x += 1) {
                for (var y = 0; y < size; y += 1) {
                    System.Console.Write(checkPoint(new Point(x, y)) ? '#' : '.');
                }
                System.Console.WriteLine();
            }
        }

        protected override int doWork() {
            prog = Parser.parseInput(inputFile);

            var count = 1;
            var pt = findFirst();

            while (pt.x < size && pt.y < size) {
                var rowCount = countRow(pt);

                count += rowCount;

                pt.x += 1;
                pt.y += 1;
            }

            return count;
        }
    }
}
