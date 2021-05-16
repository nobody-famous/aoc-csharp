using System.Collections.Generic;

namespace aoc.y2019.day7
{
    class Part1 : Solver
    {
        public Part1(string file, int exp) : base(file, exp) { }

        protected override int runChain(List<Amp> chain) {
            int? signal = 0;

            foreach (var amp in chain) {
                if (signal is int sig) {
                    amp.inp.setSignal(sig);
                    signal = runToOutput(amp);
                }
            }

            if (signal is int s) {
                return s;
            } else {
                return 0;
            }
        }

        protected override int doWork() {
            return base.doWork(new List<int>() { 0, 1, 2, 3, 4 });
        }
    }
}
