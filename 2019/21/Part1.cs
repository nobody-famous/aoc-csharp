using aoc.y2019.intcode;

namespace aoc.y2019.day21
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string fileName, int exp) : base(fileName, exp) { }

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

            ctrl.WriteString("WALK");
        }

        private void ExpectString(Controller ctrl, string str) {
            var value = ctrl.ReadString();

            if (str != value) {
                throw new System.Exception($"Expected {str}, found {value}");
            }
        }

        private void SkipWalkingMsg(Controller ctrl) {
            ExpectString(ctrl, "");
            ExpectString(ctrl, "Walking...");
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
            var cmds = new string[] {
                "NOT A J",
                "NOT B T",
                "OR T J",
                "NOT C T",
                "OR T J",
                "AND D J",
             };

            ReadPrompt(ctrl);
            SendCommands(ctrl, cmds);

            SkipWalkingMsg(ctrl);

            var value = ctrl.ReadValue();

            if (value < 256) {
                SkipStatusMsg(ctrl);
                WalkFailed(ctrl);
                throw new System.Exception("FAILED");
            }

            return (int)value;
        }
    }
}