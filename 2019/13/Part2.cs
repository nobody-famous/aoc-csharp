using aoc.intcode;

namespace aoc.y2019.day13
{
    class Part2 : aoc.utils.ProblemSolver<int>
    {
        public Part2(string file, int exp) : base(file, exp) { }

        protected override int doWork() {
            var prog = Parser.parseInput(inputFile);

            prog[0] = 2;
            var game = new Cabinet(prog);

            game.run();

            return (int)game.score;
        }
    }
}
