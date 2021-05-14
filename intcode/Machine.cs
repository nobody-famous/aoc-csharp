namespace aoc.intcode
{
    class Machine
    {
        int[] prog;
        int ip = 0;
        bool halt = false;
        bool debug = false;

        public Machine(int[] prog)
        {
            this.prog = prog;
        }

        public int this[int ndx]
        {
            get { return prog[ndx]; }
            set { prog[ndx] = value; }
        }

        public void setDebug(bool debug) { this.debug = debug; }

        public bool isHalted()
        {
            return halt;
        }

        private void opCode1()
        {
            var arg1 = prog[prog[ip + 1]];
            var arg2 = prog[prog[ip + 2]];
            var addr = prog[ip + 3];

            if (debug) { System.Console.WriteLine($"ADD {arg1} + {arg2} = {arg1 + arg2} => {addr}"); }

            prog[addr] = arg1 + arg2;

            ip += 4;
        }

        private void opCode2()
        {
            var arg1 = prog[prog[ip + 1]];
            var arg2 = prog[prog[ip + 2]];
            var addr = prog[ip + 3];

            prog[addr] = arg1 * arg2;

            ip += 4;
        }

        private void opCode99()
        {
            halt = true;
            ip += 1;
        }

        public void step()
        {
            if (halt || ip > prog.Length)
            {
                throw new System.Exception("HALTED");
            }

            var opCode = prog[ip];

            switch (opCode)
            {
                case 1: opCode1(); break;
                case 2: opCode2(); break;
                case 99: opCode99(); break;
                default:
                    throw new System.Exception($"Unhandled opcode {opCode}");
            }
        }
    }
}
