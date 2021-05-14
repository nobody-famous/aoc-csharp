using aoc.intcode;

namespace aoc.y2019.day2
{
    class Part2 : Solver
    {
        private int target = 19690720;

        public Part2(string fileName, int exp) : base(fileName, exp) { }

        private int? findVerb(int[] prog, int noun)
        {
            var copy = new int[prog.Length];

            for (var verb = 0; verb < 100; verb += 1)
            {
                System.Array.Copy(prog, copy, prog.Length);

                var value = runMachine(copy, noun, verb);
                if (value == target)
                {
                    return verb;
                }
            }

            return null;
        }

        private (int, int)? findValues(int[] prog)
        {
            for (var noun = 0; noun < 100; noun += 1)
            {
                var verb = findVerb(prog, noun);

                if (verb is int v)
                {
                    return (noun, v);
                }
            }

            return (0, 0);
        }

        protected override int doWork()
        {
            var prog = Parser.parseInput(inputFile);
            var copy = new int[prog.Length];

            var vals = findValues(prog);

            if (vals is (int, int) v)
            {
                var (noun, verb) = v;
                return noun * 100 + verb;
            }

            return 0;
        }
    }
}
