using System.Collections.Generic;
using aoc.intcode;
using aoc.utils.geometry;

namespace aoc.y2019.day13
{
    enum ReadState { X_POS, Y_POS, TILE_ID }

    enum Tile { EMPTY, WALL, BLOCK, PADDLE, BALL }

    class Cabinet : Listener
    {
        private Machine machine;
        private ReadState state;
        private long x = 0;
        private long y = 0;

        public Dictionary<Point, Tile> grid { get; }

        public Cabinet(long[] prog) {
            machine = new Machine(prog, this);
            grid = new Dictionary<Point, Tile>();
        }

        public void run() {
            while (!machine.isHalted()) {
                machine.step();
            }
        }

        public long input() { throw new System.NotImplementedException("input"); }

        public void output(long value) {
            if (state == ReadState.X_POS) {
                x = value;
                state = ReadState.Y_POS;
            } else if (state == ReadState.Y_POS) {
                y = value;
                state = ReadState.TILE_ID;
            } else if (state == ReadState.TILE_ID) {
                grid[new Point((int)x, (int)y)] = value switch
                {
                    0 => Tile.EMPTY,
                    1 => Tile.WALL,
                    2 => Tile.BLOCK,
                    3 => Tile.PADDLE,
                    4 => Tile.BALL,
                    _ => throw new System.Exception($"Invalid tile {value}"),
                };

                state = ReadState.X_POS;
            }
        }
    }

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
