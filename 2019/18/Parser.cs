using aoc.utils.geometry;

namespace aoc.y2019.day18
{
    class Parser
    {
        static void parseLine(Grid grid, int row, string line) {
            for (var col = 0; col < line.Length; col += 1) {
                var ch = line[col];
                var pt = new Point(col, row);

                if (ch == '.') {
                    grid.spaces.Add(pt, new Space());
                } else if (ch >= 'a' && ch <= 'z') {
                    grid.keys.Add(pt, new Key(ch));
                    grid.allMasks |= grid.masks[ch];
                } else if (ch >= 'A' && ch <= 'Z') {
                    grid.doors.Add(pt, new Door(ch));
                } else if (ch == '@') {
                    grid.entrance = pt;
                    grid.spaces.Add(pt, new Space());
                }
            }
        }

        public static Grid parseInput(string fileName) {
            var lines = aoc.utils.Parser.readLines(fileName);
            var grid = new Grid();

            for (var row = 0; row < lines.Length; row += 1) {
                parseLine(grid, row, lines[row]);
            }

            return grid;
        }
    }
}