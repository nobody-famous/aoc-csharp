using aoc.y2019.intcode;

namespace aoc.y2019.day21
{
    abstract class Solver : aoc.utils.ProblemSolver<int>
    {
        public Solver(string fileName, int exp) : base(fileName, exp) { }

        abstract protected string getSpeed();
        abstract protected string[] getCommands();

        private void ReadPrompt(Controller ctrl) {
            var prompt = ctrl.ReadString();

            if (prompt != "Input instructions:") {
                throw new System.Exception($"Invalid prompt: {prompt}");
            }
        }

        private void SendCommands(Controller ctrl, string[] cmds) {
            foreach (var cmd in cmds) {
                ctrl.WriteString(cmd);
            }
        }

        private void ExpectString(Controller ctrl, string str) {
            var value = ctrl.ReadString();

            if (str != value) {
                throw new System.Exception($"Expected {str}, found {value}");
            }
        }

        private void SkipMsg(Controller ctrl, string msg) {
            ExpectString(ctrl, "");
            ExpectString(ctrl, $"{msg}...");
            ExpectString(ctrl, "");
        }

        private void SkipStatusMsg(Controller ctrl) {
            ExpectString(ctrl, "Didn't make it across:");
            ExpectString(ctrl, "");
        }

        private bool ShowFrame(Controller ctrl) {
            var lastFrame = false;
            var line = "";

            do {
                line = ctrl.ReadString();
                lastFrame = lastFrame || (line.Length > 0 && line[0] == '#' && line.Contains('@'));
                System.Console.WriteLine(line);
            } while (line.Length > 0);

            return lastFrame;
        }

        private void WalkFailed(Controller ctrl) {
            var lastFrame = false;
            while (!lastFrame) {
                lastFrame = ShowFrame(ctrl);
            }
        }

        protected override int doWork() {
            var prog = Parser.parseInput(inputFile);
            var ctrl = new Controller(prog);
            var speed = getSpeed();
            var cmds = getCommands();

            ReadPrompt(ctrl);
            SendCommands(ctrl, cmds);

            ctrl.WriteString(speed);
            SkipMsg(ctrl, speed == "WALK" ? "Walking" : "Running");

            var value = ctrl.ReadValue();

            if (value < 256) {
                SkipStatusMsg(ctrl);
                WalkFailed(ctrl);
                throw new System.Exception("FAILED");
            }

            return (int)value;
        }
    }

    class Controller : IOHandler
    {
        private Machine machine;
        private long? nextInput = null;
        private long? lastOutput = null;

        public Controller(long[] prog) {
            machine = new Machine(prog, this);
        }

        public bool isHalted() {
            return machine.isHalted();
        }

        public void WriteString(string str) {
            foreach (var ch in str) {
                WriteChar(ch);
            }

            WriteChar((char)10);
        }

        public string ReadString() {
            var str = new System.Text.StringBuilder();

            var ch = ReadChar();
            while (ch != (char)10) {
                str.Append(ch);
                ch = ReadChar();
            }

            return str.ToString();
        }

        public long ReadValue() {
            lastOutput = null;

            while (lastOutput is null) {
                machine.step();
            }

            return (long)lastOutput;
        }

        private void WriteChar(char ch) {
            nextInput = ch;

            while (nextInput is not null) {
                machine.step();
            }
        }

        private char ReadChar() {
            lastOutput = null;

            while (lastOutput is null) {
                machine.step();
            }

            return (char)lastOutput;
        }

        public long input() {
            if (nextInput is null) {
                throw new System.Exception("Input is null");
            }

            var value = nextInput;

            nextInput = null;

            return (long)value;
        }

        public void output(long value) {
            lastOutput = value;
        }
    }
}