using System.Collections.Generic;
using aoc.utils.geometry;

namespace aoc.y2019.day20
{
    interface GridItem { }

    record Empty : GridItem;
    record Wall : GridItem;
    record Maze : GridItem;
    record OuterJump(Point pt) : GridItem;
    record InnerJump(Point pt) : GridItem;

    record Grid(GridItem[,] items, Dictionary<string, Point> innerJumps, Dictionary<string, Point> outerJumps);

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

        private GridItem[,] convertNonJumps(char[,] grid) {
            var height = grid.GetLength(0);
            var width = grid.GetLength(1);
            var items = new GridItem[height, width];

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

            return items;
        }

        private Dictionary<string, Point> findOuterJumps(char[,] grid) {
            var jumps = new Dictionary<string, Point>();
            var pt = new Point(2, 2);

            while (grid[pt.y, pt.x] != ' ') {
                if (grid[pt.y, pt.x] == '.') {
                    addTop(jumps, grid, new Point(pt));
                }
                pt.x += 1;
            }

            pt.x -= 1;

            while (grid[pt.y, pt.x] != ' ') {
                if (grid[pt.y, pt.x] == '.') {
                    addRight(jumps, grid, new Point(pt));
                }
                pt.y += 1;
            }

            pt.y -= 1;

            while (grid[pt.y, pt.x] != ' ') {
                if (grid[pt.y, pt.x] == '.') {
                    addBottom(jumps, grid, new Point(pt));
                }
                pt.x -= 1;
            }

            pt.x += 1;

            while (grid[pt.y, pt.x] != ' ') {
                if (grid[pt.y, pt.x] == '.') {
                    addLeft(jumps, grid, new Point(pt));
                }
                pt.y -= 1;
            }

            return jumps;
        }

        private bool isNameChar(char ch) {
            return ch >= 'A' && ch <= 'Z';
        }

        private Dictionary<string, Point> findInnerJumps(char[,] grid) {
            var jumps = new Dictionary<string, Point>();
            var pt = new Point(2, 2);

            while (grid[pt.y, pt.x] != ' ') {
                pt.x += 1;
                pt.y += 1;
            }

            while (grid[pt.y, pt.x] == ' ' || isNameChar(grid[pt.y, pt.x])) {
                if (isNameChar(grid[pt.y, pt.x])) {
                    addBottom(jumps, grid, new Point(pt.x, pt.y - 1));
                }

                pt.x += 1;
            }

            pt.x -= 1;

            while (grid[pt.y, pt.x] == ' ' || isNameChar(grid[pt.y, pt.x])) {
                if (isNameChar(grid[pt.y, pt.x])) {
                    addLeft(jumps, grid, new Point(pt.x + 1, pt.y));
                }

                pt.y += 1;
            }

            pt.y -= 1;

            while (grid[pt.y, pt.x] == ' ' || isNameChar(grid[pt.y, pt.x])) {
                if (isNameChar(grid[pt.y, pt.x])) {
                    addTop(jumps, grid, new Point(pt.x, pt.y + 1));
                }

                pt.x -= 1;
            }

            pt.x += 1;

            while (grid[pt.y, pt.x] == ' ' || isNameChar(grid[pt.y, pt.x])) {
                if (isNameChar(grid[pt.y, pt.x])) {
                    addRight(jumps, grid, new Point(pt.x - 1, pt.y));
                }

                pt.y -= 1;
            }

            return jumps;
        }

        private void addTop(Dictionary<string, Point> jumps, char[,] grid, Point pt) {
            var name = $"{grid[pt.y - 2, pt.x]}{grid[pt.y - 1, pt.x]}";

            addJump(jumps, name, pt);
        }

        private void addBottom(Dictionary<string, Point> jumps, char[,] grid, Point pt) {
            var name = $"{grid[pt.y + 1, pt.x]}{grid[pt.y + 2, pt.x]}";

            addJump(jumps, name, pt);
        }

        private void addRight(Dictionary<string, Point> jumps, char[,] grid, Point pt) {
            var name = $"{grid[pt.y, pt.x + 1]}{grid[pt.y, pt.x + 2]}";

            addJump(jumps, name, pt);
        }

        private void addLeft(Dictionary<string, Point> jumps, char[,] grid, Point pt) {
            var name = $"{grid[pt.y, pt.x - 2]}{grid[pt.y, pt.x - 1]}";

            addJump(jumps, name, pt);
        }

        private void addJump(Dictionary<string, Point> jumps, string name, Point pt) {
            jumps[name] = pt;
        }

        private Grid convertToGrid(char[,] grid) {
            var height = grid.GetLength(0);
            var width = grid.GetLength(1);

            var items = convertNonJumps(grid);
            var outer = findOuterJumps(grid);
            var inner = findInnerJumps(grid);

            foreach (var entry in outer) {
                var name = entry.Key;
                var outerPt = entry.Value;

                if (!inner.ContainsKey(name)) {
                    items[outerPt.y, outerPt.x] = new Maze();
                    continue;
                }

                var innerPt = inner[name];
                items[outerPt.y, outerPt.x] = new OuterJump(innerPt);
                items[innerPt.y, innerPt.x] = new InnerJump(outerPt);
            }

            return new Grid(items, inner, outer);
        }

        public Grid parseInput() {
            var lines = aoc.utils.Parser.readLines(fileName);
            var grid = readGrid(lines);

            return convertToGrid(grid);
        }
    }
}
