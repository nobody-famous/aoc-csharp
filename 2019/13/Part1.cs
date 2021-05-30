using aoc.intcode;

namespace aoc.y2019.day13
{
    class Part1 : aoc.utils.ProblemSolver<int>
    {
        public Part1(string file, int exp) : base(file, exp) { }

        protected override int doWork() {
            var prog = Parser.parseInput(inputFile);
            var game = new Cabinet(prog);

            game.run();

            var blockCount = 0;

            foreach (var entry in game.grid) {
                if (entry.Value == Tile.BLOCK) {
                    blockCount += 1;
                }
            }

            return blockCount;
        }
    }
}
