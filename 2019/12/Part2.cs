using System.Collections.Generic;

namespace aoc.y2019.day12
{
    class LoopDetector
    {
        private List<List<Moon>> history;
        public int? xLoop { get; set; }
        public int? yLoop { get; set; }
        public int? zLoop { get; set; }

        public LoopDetector() {
            history = new List<List<Moon>>();
        }

        private List<Moon> copy(List<Moon> moons) {
            var cp = new List<Moon>();

            foreach (var moon in moons) {
                cp.Add(new Moon(moon));
            }

            return cp;
        }

        public void Add(List<Moon> moons) {
            history.Add(copy(moons));

            if (history.Count < 2) { return; }

            if (xLoop is null) {
                checkLoopX();
            }

            if (yLoop is null) {
                checkLoopY();
            }

            if (zLoop is null) {
                checkLoopZ();
            }
        }

        private bool checkAllX(int first, int last) {
            var entry1 = history[first];
            var entry2 = history[last];

            for (var ndx = 0; ndx < entry1.Count; ndx += 1) {
                if (entry1[ndx].pos.x != entry2[ndx].pos.x) {
                    return false;
                }
            }

            return true;
        }

        private void checkLoopX() {
            var first = 0;
            var last = history.Count - 1;

            while (first < last) {
                if (!checkAllX(first, last)) {
                    return;
                }

                first += 1;
                last -= 1;
            }

            xLoop = history.Count;
        }

        private bool checkAllY(int first, int last) {
            var entry1 = history[first];
            var entry2 = history[last];

            for (var ndy = 0; ndy < entry1.Count; ndy += 1) {
                if (entry1[ndy].pos.y != entry2[ndy].pos.y) {
                    return false;
                }
            }

            return true;
        }

        private void checkLoopY() {
            var first = 0;
            var last = history.Count - 1;

            while (first < last) {
                if (!checkAllY(first, last)) {
                    return;
                }

                first += 1;
                last -= 1;
            }

            yLoop = history.Count;
        }

        private bool checkAllZ(int first, int last) {
            var entrz1 = history[first];
            var entrz2 = history[last];

            for (var ndz = 0; ndz < entrz1.Count; ndz += 1) {
                if (entrz1[ndz].pos.z != entrz2[ndz].pos.z) {
                    return false;
                }
            }

            return true;
        }

        private void checkLoopZ() {
            var first = 0;
            var last = history.Count - 1;

            while (first < last) {
                if (!checkAllZ(first, last)) {
                    return;
                }

                first += 1;
                last -= 1;
            }

            zLoop = history.Count;
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

            System.Console.WriteLine($"x {x} y {y} z {z}");

            return 0;
        }
    }
}