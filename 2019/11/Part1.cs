using aoc.intcode;

namespace aoc.y2019.day11
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        protected override int doWork() {
            var prog = Parser.parseInput(inputFile);
            var robot = new Robot(prog, Color.BLACK);

            robot.run();

            return robot.panels.Count;
        }
    }
}
