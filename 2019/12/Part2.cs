using System.Collections.Generic;
using aoc.utils;

namespace aoc.y2019.day12
{
    class LoopDetector
    {
        private List<int[]> xHistory;
        private List<int[]> yHistory;
        private List<int[]> zHistory;
        public int? xLoop = null;
        public int? yLoop = null;
        public int? zLoop = null;

        public LoopDetector() {
            xHistory = new List<int[]>();
            yHistory = new List<int[]>();
            zHistory = new List<int[]>();
        }

        public void Add(List<Moon> moons) {
            if (xLoop is null) {
                var xList = new List<int>();

                xHistory.Add(new int[] {
                    moons[0].pos.x,
                    moons[1].pos.x,
                    moons[2].pos.x,
                    moons[3].pos.x,
                });

                if (xHistory.Count > 2) {
                    xLoop = checkLoop(xHistory);
                }
            }

            if (yLoop is null) {
                var yList = new List<int>();

                yHistory.Add(new int[] {
                    moons[0].pos.y,
                    moons[1].pos.y,
                    moons[2].pos.y,
                    moons[3].pos.y,
                });

                if (yHistory.Count > 2) {
                    yLoop = checkLoop(yHistory);
                }
            }

            if (zLoop is null) {
                var zList = new List<int>();

                zHistory.Add(new int[] {
                    moons[0].pos.z,
                    moons[1].pos.z,
                    moons[2].pos.z,
                    moons[3].pos.z,
                });

                if (zHistory.Count > 2) {
                    zLoop = checkLoop(zHistory);
                }
            }
        }

        private bool checkAll(int[] first, int[] last) {
            for (var ndx = 0; ndx < first.Length; ndx += 1) {
                if (first[ndx] != last[ndx]) {
                    return false;
                }
            }

            return true;
        }

        private int? checkLoop(List<int[]> history) {
            var first = 0;
            var last = history.Count - 1;

            while (first < last) {
                if (!checkAll(history[first], history[last])) {
                    return null;
                }

                first += 1;
                last -= 1;
            }

            return history.Count;
        }
    }

    class Part2 : Solver<long>
    {
        public Part2(string file, long exp) : base(file, exp) { }

        private (int, int, int) getLoops(List<Moon> moons) {
            var detector = new LoopDetector();

            while (detector.xLoop is null || detector.yLoop is null || detector.zLoop is null) {
                detector.Add(moons);
                runLoop(moons);
            }

            return ((int)detector.xLoop, (int)detector.yLoop, (int)detector.zLoop);
        }

        protected override long doWork() {
            var moons = Parser.parseInput(inputFile);
            var (x, y, z) = getLoops(moons);
            var lcm = Helpers.lcm(Helpers.lcm(x, y), z);

            return lcm;
        }
    }
}