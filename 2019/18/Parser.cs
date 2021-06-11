using aoc.utils.geometry;

namespace aoc.y2019.day18
{
    class Parser
    {
        private int mask = 1;

        private void addMask(Grid grid, Point pt) {
            grid.ptMasks[pt] = mask;
            grid.allPtMasks |= mask;

            mask <<= 1;
        }

        private void parseLine(Grid grid, int row, string line) {
            for (var col = 0; col < line.Length; col += 1) {
                var ch = line[col];
                var pt = new Point(col, row);

                if (ch == '.') {
                    grid.spaces[pt] = new Space();
                } else if (ch == '@') {
                    grid.entrances[pt] = new Enter();
                    grid.spaces[pt] = new Space();
                    // addMask(grid, pt);
                } else if (ch >= 'a' && ch <= 'z') {
                    grid.keys[pt] = new Key(ch);
                    grid.allMasks |= grid.masks[ch];
                    addMask(grid, pt);
                } else if (ch >= 'A' && ch <= 'Z') {
                    grid.doors[pt] = new Door(ch);
                }
            }
        }

        public Grid parseInput(string fileName) {
            var lines = aoc.utils.Parser.readLines(fileName);
            var grid = new Grid();

            for (var row = 0; row < lines.Length; row += 1) {
                parseLine(grid, row, lines[row]);
            }

            return grid;
        }
    }
}