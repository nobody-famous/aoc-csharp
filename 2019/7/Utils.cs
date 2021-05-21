using System.Collections.Generic;
using aoc.intcode;

namespace aoc.y2019.day7
{
    class Provider : Listener
    {
        protected int phase;
        protected int? signal;
        protected bool sentPhase = false;
        protected int? lastValue;

        public Provider(int phase) {
            this.phase = phase;
        }

        public void setSignal(int value) { signal = value; }

        public int input() {
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

        public void output(int value) { lastValue = value; }

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

    record Amp(Machine mach, Provider provider);

    abstract class Solver : aoc.utils.ProblemSolver<int>
    {
        protected int[] prog;

        public Solver(string file, int exp) : base(file, exp) {
            prog = aoc.intcode.Parser.parseInput(inputFile);
        }

        abstract protected int runChain(List<Amp> amps);

        protected Amp makeAmp(int[] prog, int phase) {
            var provider = new Provider(phase);

            return new Amp(new Machine(prog, provider), provider);
        }

        protected int? runToOutput(Amp amp) {
            while (!amp.provider.hasValue()) {
                amp.mach.step();
                if (amp.mach.isHalted()) {
                    return null;
                }
            }

            return amp.provider.getValue();
        }

        protected int runPerm(int[] prog, List<int> perm) {
            var amps = new List<Amp>() {
                makeAmp(prog, perm[0]),
                makeAmp(prog, perm[1]),
                makeAmp(prog, perm[2]),
                makeAmp(prog, perm[3]),
                makeAmp(prog, perm[4]),
            };

            return runChain(amps);
        }

        protected void swap(List<int> nums, int ndx1, int ndx2) {
            var tmp = nums[ndx1];
            nums[ndx1] = nums[ndx2];
            nums[ndx2] = tmp;
        }

        protected void getPerms(List<List<int>> perms, List<int> nums, int size) {
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

        protected List<List<int>> getPerms(List<int> nums) {
            var perms = new List<List<int>>();

            getPerms(perms, nums, nums.Count);

            return perms;
        }

        protected int doWork(List<int> phases) {
            var perms = getPerms(phases);
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