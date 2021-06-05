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

        private const int READ_SCAFFOLD = 0;
        private const int READ_PROMPT = 1;
        private const int READ_DUST = 2;
        private const int SEND = 3;

        private int state = READ_SCAFFOLD;

        private int nlCount = 0;
        private string prompt = "";
        private Queue<long> inputQueue = new Queue<long>();
        private long dust = 0;

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

        public void getScaffold() {
            state = READ_SCAFFOLD;
            nlCount = 0;

            while (state == READ_SCAFFOLD) {
                machine.step();
            }
        }

        public string getPrompt() {
            state = READ_PROMPT;
            prompt = "";

            while (state == READ_PROMPT) {
                machine.step();
            }

            return prompt;
        }

        public long getDust() {
            state = READ_DUST;

            while (!machine.isHalted()) {
                machine.step();
            }

            return dust;
        }

        public void send(string str) {
            var toSend = $"{str}\n";

            foreach (var ch in toSend) {
                inputQueue.Enqueue((long)ch);
            }

            while (state == SEND) {
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
        public long input() {
            if (inputQueue.Count == 0) {
                throw new System.Exception("No input");
            }

            var value = inputQueue.Dequeue();

            if (inputQueue.Count == 0) {
                state = READ_PROMPT;
            }

            return value;
        }

        public void output(long value) {
            if (state == READ_SCAFFOLD) {
                readScaffold(value);
            } else if (state == READ_PROMPT) {
                readPrompt(value);
            } else if (state == READ_DUST) {
                dust = value;
            } else if (state == SEND) {
                // Why does it send stuff here???
            } else {
                System.Console.WriteLine($"STATE {state} {value} {(char)value}");
            }
        }

        private void readPrompt(long value) {
            if (value == (int)'\n') {
                state = SEND;
            } else {
                prompt = $"{prompt}{(char)value}";
            }
        }

        private void readScaffold(long value) {
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
                nlCount += 1;

                if (nlCount > 1) {
                    state = READ_PROMPT;
                }
            } else {
                throw new System.Exception($"Invalid scaffold value {value}");
            }

            if (value != Cell.NEW_LINE) {
                nlCount = 0;
            }
        }
    }
}
