using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day20
{
    interface GridItem { }

    record Empty : GridItem;
    record Wall : GridItem;
    record Maze : GridItem;
    record Jump(Point pt) : GridItem;

    record Grid(GridItem[,] items, Dictionary<string, List<Point>> jumps);

    class Parser
    {
        private string fileName;

        public Parser(string fileName) {
            this.fileName = fileName;
        }

        private char[,] readGrid(string[] lines) {
            var height = lines.Length;
            var width = lines[0].Length;
            var grid = new char[height, width];

            var row = 0;
            var col = 0;
            foreach (var line in lines) {
                col = 0;

                foreach (var ch in line) {
                    grid[row, col] = ch;
                    col += 1;
                }

                row += 1;
            }

            return grid;
        }

        private void convertNonJumps(GridItem[,] items, char[,] grid) {
            var height = grid.GetLength(0);
            var width = grid.GetLength(1);

            for (var row = 0; row < height; row += 1) {
                for (var col = 0; col < width; col += 1) {
                    var ch = grid[row, col];

                    items[row, col] = grid[row, col] switch
                    {
                        ' ' => new Empty(),
                        >= 'A' and <= 'Z' => new Empty(),
                        '#' => new Wall(),
                        '.' => new Maze(),
                        _ => throw new System.Exception($"Unhandled char {grid[row, col]}")
                    };
                }
            }
        }

        private void findOuterJumps(char[,] grid, Dictionary<string, List<Point>> jumps) {
            var pt = new Point(2, 2);

            while (grid[pt.y, pt.x] != ' ') {
                if (grid[pt.y, pt.x] == '.') {
                    addTop(grid, new Point(pt), jumps);
                }
                pt.x += 1;
            }

            pt.x -= 1;

            while (grid[pt.y, pt.x] != ' ') {
                if (grid[pt.y, pt.x] == '.') {
                    addRight(grid, new Point(pt), jumps);
                }
                pt.y += 1;
            }

            pt.y -= 1;

            while (grid[pt.y, pt.x] != ' ') {
                if (grid[pt.y, pt.x] == '.') {
                    addBottom(grid, new Point(pt), jumps);
                }
                pt.x -= 1;
            }

            pt.x += 1;

            while (grid[pt.y, pt.x] != ' ') {
                if (grid[pt.y, pt.x] == '.') {
                    addLeft(grid, new Point(pt), jumps);
                }
                pt.y -= 1;
            }
        }

        private bool isNameChar(char ch) {
            return ch >= 'A' && ch <= 'Z';
        }

        private void findInnerJumps(char[,] grid, Dictionary<string, List<Point>> jumps) {
            var pt = new Point(2, 2);

            while (grid[pt.y, pt.x] != ' ') {
                pt.x += 1;
                pt.y += 1;
            }

            while (grid[pt.y, pt.x] == ' ' || isNameChar(grid[pt.y, pt.x])) {
                if (isNameChar(grid[pt.y, pt.x])) {
                    addBottom(grid, new Point(pt.x, pt.y - 1), jumps);
                }

                pt.x += 1;
            }

            pt.x -= 1;

            while (grid[pt.y, pt.x] == ' ' || isNameChar(grid[pt.y, pt.x])) {
                if (isNameChar(grid[pt.y, pt.x])) {
                    addLeft(grid, new Point(pt.x + 1, pt.y), jumps);
                }

                pt.y += 1;
            }

            pt.y -= 1;

            while (grid[pt.y, pt.x] == ' ' || isNameChar(grid[pt.y, pt.x])) {
                if (isNameChar(grid[pt.y, pt.x])) {
                    addTop(grid, new Point(pt.x, pt.y + 1), jumps);
                }

                pt.x -= 1;
            }

            pt.x += 1;

            while (grid[pt.y, pt.x] == ' ' || isNameChar(grid[pt.y, pt.x])) {
                if (isNameChar(grid[pt.y, pt.x])) {
                    addRight(grid, new Point(pt.x - 1, pt.y), jumps);
                }

                pt.y -= 1;
            }
        }

        private void addTop(char[,] grid, Point pt, Dictionary<string, List<Point>> jumps) {
            var name = $"{grid[pt.y - 2, pt.x]}{grid[pt.y - 1, pt.x]}";

            addJump(jumps, name, pt);
        }

        private void addBottom(char[,] grid, Point pt, Dictionary<string, List<Point>> jumps) {
            var name = $"{grid[pt.y + 1, pt.x]}{grid[pt.y + 2, pt.x]}";

            addJump(jumps, name, pt);
        }

        private void addRight(char[,] grid, Point pt, Dictionary<string, List<Point>> jumps) {
            var name = $"{grid[pt.y, pt.x + 1]}{grid[pt.y, pt.x + 2]}";

            addJump(jumps, name, pt);
        }

        private void addLeft(char[,] grid, Point pt, Dictionary<string, List<Point>> jumps) {
            var name = $"{grid[pt.y, pt.x - 2]}{grid[pt.y, pt.x - 1]}";

            addJump(jumps, name, pt);
        }

        private void addJump(Dictionary<string, List<Point>> jumps, string name, Point pt) {
            if (!jumps.ContainsKey(name)) {
                jumps[name] = new List<Point>();
            }

            jumps[name].Add(pt);
        }

        private Dictionary<string, List<Point>> findJumps(char[,] grid) {
            var height = grid.GetLength(0);
            var width = grid.GetLength(1);
            var jumps = new Dictionary<string, List<Point>>();

            findOuterJumps(grid, jumps);
            findInnerJumps(grid, jumps);

            return jumps;
        }

        private Grid convertToGrid(char[,] grid) {
            var height = grid.GetLength(0);
            var width = grid.GetLength(1);
            var items = new GridItem[height, width];

            convertNonJumps(items, grid);

            var jumps = findJumps(grid);

            foreach (var entry in jumps) {
                var name = entry.Key;
                var pts = entry.Value;

                if (pts.Count == 2) {
                    var pt1 = pts[0];
                    var pt2 = pts[1];

                    items[pt1.y, pt1.x] = new Jump(pt2);
                    items[pt2.y, pt2.x] = new Jump(pt1);
                }
            }

            return new Grid(items, jumps);
        }

        public Grid parseInput() {
            var lines = aoc.utils.Parser.readLines(fileName);
            var grid = readGrid(lines);

            return convertToGrid(grid);
        }
    }
}
