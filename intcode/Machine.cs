namespace aoc.intcode
{
    interface Listener
    {
        int input();

        void output(int value);
    }

    record Instr(int code, int modes);

    class Machine
    {
        int[] prog;
        int ip = 0;
        bool halt = false;
        bool debug = false;
        Listener? listener = null;

        private abstract record Instruction(Machine parent, int length)
        {
            public void run() {
                exec();
                parent.ip += length;
            }

            public abstract void exec();
        }

        private record Add(Machine parent, int arg1, int arg2, int addr) : Instruction(parent, 4)
        {
            public override void exec() { parent.prog[addr] = arg1 + arg2; }
        }

        private record Mul(Machine parent, int arg1, int arg2, int addr) : Instruction(parent, 4)
        {
            public override void exec() { parent.prog[addr] = arg1 * arg2; }
        }

        private record Inp(Machine parent, int addr) : Instruction(parent, 2)
        {
            public override void exec() {
                if (parent.listener is not null) {
                    parent.prog[addr] = parent.listener.input();
                } else {
                    throw new System.Exception("Input listener is null");
                }
            }
        }

        private record Out(Machine parent, int value) : Instruction(parent, 2)
        {
            public override void exec() {
                if (parent.listener is not null) {
                    parent.listener.output(value);
                } else {
                    throw new System.Exception("Output listener is null");
                }
            }
        }

        private record Jnz(Machine parent, int arg1, int arg2) : Instruction(parent, 0)
        {
            public override void exec() {
                parent.ip = arg1 switch
                {
                    0 => parent.ip + 3,
                    _ => arg2,
                };
            }
        }

        private record Jez(Machine parent, int arg1, int arg2) : Instruction(parent, 0)
        {
            public override void exec() {
                parent.ip = arg1 switch
                {
                    0 => arg2,
                    _ => parent.ip + 3,
                };
            }
        }

        private record Lt(Machine parent, int arg1, int arg2, int addr) : Instruction(parent, 4)
        {
            public override void exec() { parent.prog[addr] = (arg1 < arg2) ? 1 : 0; }
        }

        private record Eql(Machine parent, int arg1, int arg2, int addr) : Instruction(parent, 4)
        {
            public override void exec() { parent.prog[addr] = (arg1 == arg2) ? 1 : 0; }
        }

        private record Hlt(Machine parent) : Instruction(parent, 1)
        {
            public override void exec() { parent.halt = true; }
        }

        public Machine(int[] prog, Listener? listener = null) {
            this.prog = new int[prog.Length];
            System.Array.Copy(prog, this.prog, prog.Length);

            this.listener = listener;
        }

        public int this[int ndx]
        {
            get { return prog[ndx]; }
            set { prog[ndx] = value; }
        }

        public void setDebug(bool debug) { this.debug = debug; }

        public bool isHalted() { return halt; }

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

        private Instruction parseInstr() {
            var op = prog[ip];
            var code = op % 100;
            var modes = op / 100;

            return code switch
            {
                1 => new Add(this, getArg(modes, 1), getArg(modes, 2), prog[ip + 3]),
                2 => new Mul(this, getArg(modes, 1), getArg(modes, 2), prog[ip + 3]),
                3 => new Inp(this, prog[ip + 1]),
                4 => new Out(this, getArg(modes, 1)),
                5 => new Jnz(this, getArg(modes, 1), getArg(modes, 2)),
                6 => new Jez(this, getArg(modes, 1), getArg(modes, 2)),
                7 => new Lt(this, getArg(modes, 1), getArg(modes, 2), prog[ip + 3]),
                8 => new Eql(this, getArg(modes, 1), getArg(modes, 2), prog[ip + 3]),
                99 => new Hlt(this),
                _ => throw new System.Exception($"Unhandled op code {code}"),
            };
        }

        public void step() {
            if (halt || ip > prog.Length) {
                return;
            }

            var instr = parseInstr();

            if (debug) System.Console.WriteLine($"{ip} {instr}");

            instr.run();
        }
    }
}
