using System.Collections.Generic;

namespace aoc.y2019.day7
{
    class Part1 : Solver
    {
        public Part1(string file, int exp) : base(file, exp) { }

        protected override long runChain(List<Amp> chain) {
            long? signal = 0;

            foreach (var amp in chain) {
                if (signal is long sig) {
                    amp.provider.setSignal(sig);
                    signal = runToOutput(amp);
                }
            }

            if (signal is long s) {
                return s;
            } else {
                return 0;
            }
        }

        protected override long doWork() {
            return base.doWork(new List<int>() { 0, 1, 2, 3, 4 });
        }
    }
}
