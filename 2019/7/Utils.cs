using System.Collections.Generic;
using aoc.y2019.intcode;

namespace aoc.y2019.day7
{
    class Provider : IOHandler
    {
        protected int phase;
        protected long? signal;
        protected bool sentPhase = false;
        protected long? lastValue;

        public Provider(int phase) {
            this.phase = phase;
        }

        public void setSignal(long value) { signal = value; }

        public long input() {
            if (!sentPhase) {
                sentPhase = true;
                return phase;
            }

            if (signal is long s) {
                return s;
            } else {
                throw new System.Exception("No signal for input");
            }
        }

        public void output(long value) { lastValue = value; }

        public bool hasValue() {
            return lastValue != null;
        }

        public long getValue() {
            if (lastValue is long v) {
                lastValue = null;

                return v;
            } else {
                throw new System.Exception("No value from output");
            }
        }
    }

    record Amp(Machine mach, Provider provider);

    abstract class Solver : aoc.utils.ProblemSolver<long>
    {
        protected long[] prog;

        public Solver(string file, int exp) : base(file, exp) {
            prog = aoc.y2019.intcode.Parser.parseInput(inputFile);
        }

        abstract protected long runChain(List<Amp> amps);

        protected Amp makeAmp(long[] prog, int phase) {
            var provider = new Provider(phase);

            return new Amp(new Machine(prog, provider), provider);
        }

        protected long? runToOutput(Amp amp) {
            while (!amp.provider.hasValue()) {
                amp.mach.step();
                if (amp.mach.isHalted()) {
                    return null;
                }
            }

            return amp.provider.getValue();
        }

        protected long runPerm(long[] prog, List<int> perm) {
            var amps = new List<Amp>() {
                makeAmp(prog, perm[0]),
                makeAmp(prog, perm[1]),
                makeAmp(prog, perm[2]),
                makeAmp(prog, perm[3]),
                makeAmp(prog, perm[4]),
            };

            return runChain(amps);
        }

        protected long doWork(List<int> phases) {
            var perms = aoc.utils.Helpers.getPerms(phases);
            var max = long.MinValue;

            foreach (var perm in perms) {
                var signal = runPerm(prog, perm);

                if (signal > max) {
                    max = signal;
                }
            }

            return max;
        }
    }
}