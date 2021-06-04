using aoc.intcode;

namespace aoc.y2019.day17
{
    class Part2 : aoc.utils.ProblemSolver<int>
    {
        public Part2(string file, int exp) : base(file, exp) { }

        protected override int doWork() {
            var prog = Parser.parseInput(inputFile);
            var robot = new Robot(prog);

            robot.run();

            return 0;
        }
    }
}
