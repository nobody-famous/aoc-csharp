using System.Collections.Generic;

namespace aoc.intcode
{
    interface IOHandler
    {
        long input();

        void output(long value);
    }

    record Instr(int code, int modes);

    class Machine
    {
        long[] prog;
        Dictionary<long, long> mem;
        long relBase = 0;
        long ip = 0;
        bool halt = false;
        bool debug = false;
        IOHandler? listener = null;

        public Machine(Machine copy, IOHandler listener) {
            this.prog = new long[copy.prog.Length];
            System.Array.Copy(copy.prog, this.prog, copy.prog.Length);

            this.mem = new Dictionary<long, long>(copy.mem);
            this.listener = listener;
        }

        public Machine(long[] prog, IOHandler? listener = null) {
            this.prog = new long[prog.Length];
            this.mem = new Dictionary<long, long>();
            System.Array.Copy(prog, this.prog, prog.Length);

            this.listener = listener;
        }

        private abstract record Instruction(Machine parent, int length)
        {
            public void run() {
                exec();
                parent.ip += length;
            }

            public abstract void exec();
        }

        private record Add(Machine parent, long arg1, long arg2, long addr) : Instruction(parent, 4)
        {
            public override void exec() { parent[addr] = arg1 + arg2; }
        }

        private record Mul(Machine parent, long arg1, long arg2, long addr) : Instruction(parent, 4)
        {
            public override void exec() { parent[addr] = arg1 * arg2; }
        }

        private record Inp(Machine parent, long addr) : Instruction(parent, 2)
        {
            public override void exec() {
                if (parent.listener is not null) {
                    parent[addr] = parent.listener.input();
                } else {
                    throw new System.Exception("Input listener is null");
                }
            }
        }

        private record Out(Machine parent, long value) : Instruction(parent, 2)
        {
            public override void exec() {
                if (parent.listener is not null) {
                    parent.listener.output(value);
                } else {
                    throw new System.Exception("Output listener is null");
                }
            }
        }

        private record Jnz(Machine parent, long arg1, long arg2) : Instruction(parent, 0)
        {
            public override void exec() {
                parent.ip = arg1 switch
                {
                    0 => parent.ip + 3,
                    _ => arg2,
                };
            }
        }

        private record Jez(Machine parent, long arg1, long arg2) : Instruction(parent, 0)
        {
            public override void exec() {
                parent.ip = arg1 switch
                {
                    0 => arg2,
                    _ => parent.ip + 3,
                };
            }
        }

        private record Lt(Machine parent, long arg1, long arg2, long addr) : Instruction(parent, 4)
        {
            public override void exec() { parent[addr] = (arg1 < arg2) ? 1 : 0; }
        }

        private record Eql(Machine parent, long arg1, long arg2, long addr) : Instruction(parent, 4)
        {
            public override void exec() { parent[addr] = (arg1 == arg2) ? 1 : 0; }
        }

        private record RBO(Machine parent, long arg1) : Instruction(parent, 2)
        {
            public override void exec() { parent.relBase += arg1; }
        }

        private record Hlt(Machine parent) : Instruction(parent, 1)
        {
            public override void exec() { parent.halt = true; }
        }

        public long this[long ndx]
        {
            get
            {
                if (ndx < prog.Length) {
                    return prog[ndx];
                }

                return mem.ContainsKey(ndx) ? mem[ndx] : 0;
            }

            set
            {
                if (ndx < prog.Length) {
                    prog[ndx] = value;
                } else {
                    mem[ndx] = value;
                }
            }
        }

        public void setDebug(bool debug) { this.debug = debug; }

        public bool isHalted() { return halt; }

        public void stop() { halt = true; }

        private long getMode(long modes, int num) {
            var mask = (int)System.Math.Pow(10, num - 1);
            return (modes / mask) % 10;
        }

        private long getArg(long modes, int num) {
            var mode = getMode(modes, num);

            switch (mode) {
            case 0: return this[this[ip + num]];
            case 1: return this[ip + num];
            case 2: return this[relBase + this[ip + num]];
            default: throw new System.Exception($"Unhandled mode {mode}");
            }
        }

        private long writeAddr(long modes, int num) {
            var mode = getMode(modes, num);
            var addr = this[ip + num];

            return (mode == 2) ? relBase + addr : addr;
        }

        private Instruction parseInstr() {
            var op = prog[ip];
            var code = op % 100;
            var modes = op / 100;

            return code switch
            {
                1 => new Add(this, getArg(modes, 1), getArg(modes, 2), writeAddr(modes, 3)),
                2 => new Mul(this, getArg(modes, 1), getArg(modes, 2), writeAddr(modes, 3)),
                3 => new Inp(this, writeAddr(modes, 1)),
                4 => new Out(this, getArg(modes, 1)),
                5 => new Jnz(this, getArg(modes, 1), getArg(modes, 2)),
                6 => new Jez(this, getArg(modes, 1), getArg(modes, 2)),
                7 => new Lt(this, getArg(modes, 1), getArg(modes, 2), writeAddr(modes, 3)),
                8 => new Eql(this, getArg(modes, 1), getArg(modes, 2), writeAddr(modes, 3)),
                9 => new RBO(this, getArg(modes, 1)),
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
