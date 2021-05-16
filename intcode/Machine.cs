namespace aoc.intcode
{
    interface InputProvider
    {
        int next();
    }

    interface OutputHandler
    {
        void data(int value);
    }

    record Instr(int code, int modes);

    class Machine
    {
        int[] prog;
        int ip = 0;
        bool halt = false;
        bool debug = false;
        InputProvider? input;
        OutputHandler? output;

        public Machine(int[] prog, InputProvider? input = null, OutputHandler? output = null) {
            this.prog = new int[prog.Length];
            System.Array.Copy(prog, this.prog, prog.Length);

            this.input = input;
            this.output = output;
        }

        public int this[int ndx]
        {
            get { return prog[ndx]; }
            set { prog[ndx] = value; }
        }

        public void setDebug(bool debug) { this.debug = debug; }

        public bool isHalted() {
            return halt;
        }

        private int getMode(int modes, int num) {
            var mask = (int)System.Math.Pow(10, num - 1);
            return (modes / mask) % 10;
        }

        private int getArg(int modes, int num) {
            var mode = getMode(modes, num);

            switch (mode) {
            case 0: return prog[prog[ip + num]];
            case 1: return prog[ip + num];
            default: throw new System.Exception($"Unhandled mode {mode}");
            }
        }

        private void opCode1(int modes) {
            var arg1 = getArg(modes, 1);
            var arg2 = getArg(modes, 2);
            var addr = prog[ip + 3];

            if (debug) { System.Console.WriteLine($"{ip} ADD {modes} {arg1} + {arg2} = {arg1 + arg2} => {addr}"); }

            prog[addr] = arg1 + arg2;

            ip += 4;
        }

        private void opCode2(int modes) {
            var arg1 = getArg(modes, 1);
            var arg2 = getArg(modes, 2);
            var addr = prog[ip + 3];

            if (debug) { System.Console.WriteLine($"{ip} MUL {modes} {arg1} + {arg2} = {arg1 * arg2} => {addr}"); }

            prog[addr] = arg1 * arg2;

            ip += 4;
        }

        private void opCode3() {
            if (input == null) {
                throw new System.Exception("No input provider");
            }

            var addr = prog[ip + 1];
            var value = input.next();

            if (debug) { System.Console.WriteLine($"{ip} INP {value} => {addr}"); }

            prog[addr] = value;

            ip += 2;
        }

        private void opCode4(int modes) {
            if (output == null) {
                throw new System.Exception("No output provider");
            }

            var value = getArg(modes, 1);

            output.data(value);

            if (debug) { System.Console.WriteLine($"{ip} OUT {value}"); }

            ip += 2;
        }

        private void opCode5(int modes) {
            var arg1 = getArg(modes, 1);
            var arg2 = getArg(modes, 2);

            if (debug) { System.Console.WriteLine($"{ip} JNE {arg1} {arg2}"); }

            if (arg1 != 0) {
                ip = arg2;
            } else {
                ip += 3;
            }
        }

        private void opCode6(int modes) {
            var arg1 = getArg(modes, 1);
            var arg2 = getArg(modes, 2);

            if (debug) { System.Console.WriteLine($"{ip} JEQ {arg1} {arg2}"); }

            if (arg1 == 0) {
                ip = arg2;
            } else {
                ip += 3;
            }
        }

        private void opCode7(int modes) {
            var arg1 = getArg(modes, 1);
            var arg2 = getArg(modes, 2);
            var addr = prog[ip + 3];

            if (debug) { System.Console.WriteLine($"{ip} LT {arg1} {arg2} => {addr}"); }

            prog[addr] = (arg1 < arg2) ? 1 : 0;

            ip += 4;
        }

        private void opCode8(int modes) {
            var arg1 = getArg(modes, 1);
            var arg2 = getArg(modes, 2);
            var addr = prog[ip + 3];

            if (debug) { System.Console.WriteLine($"{ip} EQ {arg1} {arg2} => {addr}"); }

            prog[addr] = (arg1 == arg2) ? 1 : 0;

            ip += 4;
        }

        private void opCode99() {
            halt = true;
            ip += 1;
        }

        public void step() {
            if (halt || ip > prog.Length) {
                return;
            }

            var instr = new Instr(prog[ip] % 100, prog[ip] / 100);

            switch (instr.code) {
            case 1: opCode1(instr.modes); break;
            case 2: opCode2(instr.modes); break;
            case 3: opCode3(); break;
            case 4: opCode4(instr.modes); break;
            case 5: opCode5(instr.modes); break;
            case 6: opCode6(instr.modes); break;
            case 7: opCode7(instr.modes); break;
            case 8: opCode8(instr.modes); break;
            case 99: opCode99(); break;
            default:
                throw new System.Exception($"Unhandled opcode {instr}");
            }
        }
    }
}
