using aoc.y2019.intcode;

namespace aoc.y2019.day21
{
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