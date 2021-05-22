using System.Collections.Generic;
using System.Threading.Tasks;
using aoc.intcode;

namespace aoc.y2019.day2
{
    class Part2 : Solver
    {
        private int target = 19690720;

        public Part2(string fileName, int exp) : base(fileName, exp) { }

        private (int, int)? findVerb(long[] prog, int noun) {
            var copy = new long[prog.Length];

            for (var verb = 0; verb < 100; verb += 1) {
                System.Array.Copy(prog, copy, prog.Length);

                var value = runMachine(copy, noun, verb);
                if (value == target) {
                    return (noun, verb);
                }
            }

            return null;
        }

        private (int, int)? findValues(long[] prog) {
            var tasks = new List<Task<(int, int)?>>();

            for (var noun = 0; noun < 100; noun += 1) {
                var n = noun;

                tasks.Add(Task.Run(() => findVerb(prog, n)));
            }

            foreach (var task in tasks) {
                var res = task.Result;

                if (res is (int, int) item) {
                    return item;
                }
            }

            return (0, 0);
        }

        protected override long doWork() {
            var prog = Parser.parseInput(inputFile);
            var copy = new long[prog.Length];

            var vals = findValues(prog);

            if (vals is (int, int) v) {
                var (noun, verb) = v;
                return noun * 100 + verb;
            }

            return 0;
        }
    }
}
