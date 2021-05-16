using System.Collections.Generic;
using aoc.intcode;

namespace aoc.y2019.day7
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        protected class Input : InputProvider
        {
            private int phase;
            private int? signal;
            private bool sentPhase = false;

            public Input(int phase) {
                this.phase = phase;
            }

            public void setSignal(int value) { signal = value; }

            public int next() {
                if (!sentPhase) {
                    sentPhase = true;
                    return phase;
                }

                if (signal is int s) {
                    return s;
                } else {
                    throw new System.Exception("No signal for input");
                }
            }
        }

        protected class Output : OutputHandler
        {
            private int? lastValue;

            public void data(int value) { lastValue = value; }

            public bool hasValue() {
                return lastValue != null;
            }

            public int getValue() {
                if (lastValue is int v) {
                    lastValue = null;

                    return v;
                } else {
                    throw new System.Exception("No value from output");
                }
            }
        }

        protected record Amp(Machine mach, Input inp, Output outp);

        public Part1(string file, int exp) : base(file, exp) { }

        protected Amp makeAmp(int[] prog, int phase) {
            var inp = new Input(phase);
            var outp = new Output();

            return new Amp(new Machine(prog, inp, outp), inp, outp);
        }

        protected int runToOutput(Amp amp) {
            while (!amp.outp.hasValue()) {
                amp.mach.step();
            }

            return amp.outp.getValue();
        }

        private int runChain(List<Amp> chain) {
            var signal = 0;

            foreach (var amp in chain) {
                amp.inp.setSignal(signal);
                signal = runToOutput(amp);
            }

            return signal;
        }

        private void swap(List<int> nums, int ndx1, int ndx2) {
            var tmp = nums[ndx1];
            nums[ndx1] = nums[ndx2];
            nums[ndx2] = tmp;
        }

        private void getPerms(List<List<int>> perms, List<int> nums, int size) {
            if (size == 1) {
                var copy = new int[nums.Count];

                nums.CopyTo(copy);
                perms.Add(new List<int>(copy));

                return;
            }

            for (var ndx = 0; ndx < size; ndx += 1) {
                getPerms(perms, nums, size - 1);

                if (size % 2 == 0) {
                    swap(nums, ndx, size - 1);
                } else {
                    swap(nums, 0, size - 1);
                }
            }
        }

        private List<List<int>> getPerms(List<int> nums) {
            var perms = new List<List<int>>();

            getPerms(perms, nums, nums.Count);

            return perms;
        }

        private int runPerm(int[] prog, List<int> perm) {
            var amps = new List<Amp>() {
                makeAmp(prog, perm[0]),
                makeAmp(prog, perm[1]),
                makeAmp(prog, perm[2]),
                makeAmp(prog, perm[3]),
                makeAmp(prog, perm[4]),
            };

            return runChain(amps);
        }

        protected override int doWork() {
            var prog = aoc.intcode.Parser.parseInput(inputFile);

            var perms = getPerms(new List<int>() { 0, 1, 2, 3, 4 });
            var max = int.MinValue;

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
