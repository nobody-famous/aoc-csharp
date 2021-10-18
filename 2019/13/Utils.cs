using System.Collections.Generic;
using aoc.y2019.intcode;
using aoc.utils.geometry;

namespace aoc.y2019.day13
{
    enum ReadState { X_POS, Y_POS, TILE_ID }

    enum Tile { EMPTY, WALL, BLOCK, PADDLE, BALL }

    class Cabinet : IOHandler
    {
        private Machine machine;
        private ReadState state;
        private long x = 0;
        private long y = 0;
        private Point? ball;
        private Point? paddle;

        public Dictionary<Point, Tile> grid { get; }
        public long score { get; set; }

        public Cabinet(long[] prog) {
            machine = new Machine(prog, this);
            grid = new Dictionary<Point, Tile>();
        }

        public void run() {
            while (!machine.isHalted()) {
                machine.step();
            }
        }

        public long input() {
            if (ball?.x == paddle?.x) {
                return 0;
            } else if (ball?.x > paddle?.x) {
                return 1;
            } else {
                return -1;
            }
        }

        public void output(long value) {
            if (state == ReadState.X_POS) {
                x = value;
                state = ReadState.Y_POS;
            } else if (state == ReadState.Y_POS) {
                y = value;
                state = ReadState.TILE_ID;
            } else if (state == ReadState.TILE_ID) {
                if (x == -1 && y == 0) {
                    score = value;
                } else {
                    var tile = value switch
                    {
                        0 => Tile.EMPTY,
                        1 => Tile.WALL,
                        2 => Tile.BLOCK,
                        3 => Tile.PADDLE,
                        4 => Tile.BALL,
                        _ => throw new System.Exception($"Invalid tile {value}"),
                    };

                    var pt = new Point((int)x, (int)y);
                    grid[pt] = tile;

                    if (tile == Tile.BALL) {
                        ball = pt;
                    } else if (tile == Tile.PADDLE) {
                        paddle = pt;
                    }
                }

                state = ReadState.X_POS;
            }
        }
    }
}