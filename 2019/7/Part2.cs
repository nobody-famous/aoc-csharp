using System.Collections.Generic;

namespace aoc.y2019.day7
{
    class Part2 : Solver
    {
        public Part2(string file, int exp) : base(file, exp) { }

        protected override long runChain(List<Amp> chain) {
            long? signal = 0;
            var lastSignal = 0;

            for (var ndx = 0; !chain[chain.Count - 1].mach.isHalted(); ndx = (ndx + 1) % chain.Count) {
                var amp = chain[ndx];

                if (signal == null) {
                    continue;
                }

                amp.provider.setSignal((int)signal);
                var nextSignal = runToOutput(amp);

                if (nextSignal == null) {
                    continue;
                }

                if (ndx == chain.Count - 1) {
                    lastSignal = (int)nextSignal;
                }

                signal = nextSignal;
            }

            return lastSignal;
        }

        protected override long doWork() {
            return base.doWork(new List<int>() { 5, 6, 7, 8, 9 });
        }
    }
}
