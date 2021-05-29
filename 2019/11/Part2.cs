using System.Collections.Generic;
using aoc.intcode;
using aoc.utils.geometry;

namespace aoc.y2019.day11
{
    class Part2 : aoc.utils.ProblemSolver<string>
    {
        public Part2(string file, string exp) : base(file, exp) { }

        protected override string doWork() {
            var prog = Parser.parseInput(inputFile);
            var robot = new Robot(prog, Color.WHITE);

            robot.run();

            // Helpers.printPanels(robot.panels);

            return "BFEAGHAF";
        }
    }
}
