using System.Collections.Generic;
using aoc.intcode;
using aoc.utils.geometry;

namespace aoc.y2019.day17
{
    record PathStep(char turn, int dist);

    class PathBuilder
    {
        private Dictionary<Point, long> scaffold;
        private Point loc;
        private long dir;

        public PathBuilder(Robot robot) {
            this.scaffold = robot.scaffold;
            this.loc = robot.loc;
            this.dir = robot.dir;
        }

        private char? checkTurn(Point left, Point right) {
            if (this.scaffold.ContainsKey(left)) {
                return 'L';
            } else if (this.scaffold.ContainsKey(right)) {
                return 'R';
            } else {
                return null;
            }
        }

        private char? getTurn() {
            return dir switch
            {
                Cell.ROBOT_UP => checkTurn(new Point(loc.x - 1, loc.y), new Point(loc.x + 1, loc.y)),
                Cell.ROBOT_DOWN => checkTurn(new Point(loc.x + 1, loc.y), new Point(loc.x - 1, loc.y)),
                Cell.ROBOT_RIGHT => checkTurn(new Point(loc.x, loc.y - 1), new Point(loc.x, loc.y + 1)),
                Cell.ROBOT_LEFT => checkTurn(new Point(loc.x, loc.y + 1), new Point(loc.x, loc.y - 1)),
                _ => throw new System.Exception($"Invalid direction {dir}"),
            };
        }

        private void doTurn(char turn) {
            dir = dir switch
            {
                Cell.ROBOT_UP => turn == 'L' ? Cell.ROBOT_LEFT : Cell.ROBOT_RIGHT,
                Cell.ROBOT_DOWN => turn == 'L' ? Cell.ROBOT_RIGHT : Cell.ROBOT_LEFT,
                Cell.ROBOT_RIGHT => turn == 'L' ? Cell.ROBOT_UP : Cell.ROBOT_DOWN,
                Cell.ROBOT_LEFT => turn == 'L' ? Cell.ROBOT_DOWN : Cell.ROBOT_UP,
                _ => throw new System.Exception($"Invalid turn {dir}"),
            };
        }

        private int forward() {
            var dist = 0;
            var diff = dir switch
            {
                Cell.ROBOT_UP => new Point(0, -1),
                Cell.ROBOT_DOWN => new Point(0, 1),
                Cell.ROBOT_RIGHT => new Point(1, 0),
                Cell.ROBOT_LEFT => new Point(-1, 0),
                _ => throw new System.Exception($"Invalid direction {dir}"),
            };

            var newLoc = new Point(loc.x + diff.x, loc.y + diff.y);
            while (scaffold.ContainsKey(newLoc)) {
                loc = newLoc;
                newLoc = new Point(loc.x + diff.x, loc.y + diff.y);
                dist += 1;
            }

            return dist;
        }

        public List<PathStep> build() {
            var steps = new List<PathStep>();
            var turn = getTurn();

            while (turn is char t) {
                doTurn(t);
                var dist = forward();

                steps.Add(new PathStep(t, dist));
                turn = getTurn();
            }

            return steps;
        }
    }

    class Part2 : aoc.utils.ProblemSolver<int>
    {
        public Part2(string file, int exp) : base(file, exp) { }

        private List<List<string>> getOptions(List<PathStep> steps, int maxLength) {
            var opts = new List<List<string>>();

            for (var ndx = 0; ndx < steps.Count; ndx += 1) {
                var step = steps[ndx];
                var str = $"{step.turn},{step.dist}";

                opts.Add(new List<string>());
                opts[ndx].Add(str);

                for (var inv = ndx - 1; inv >= 0; inv -= 1) {
                    var strs = opts[inv];
                    var last = strs[strs.Count - 1];
                    var newStr = $"{last},{str}";

                    if (newStr.Length > maxLength) {
                        break;
                    }

                    strs.Add(newStr);
                }
            }

            return opts;
        }

        protected override int doWork() {
            var prog = Parser.parseInput(inputFile);
            var robot = new Robot(prog);

            robot.run();

            var steps = new PathBuilder(robot).build();
            var opts = getOptions(steps, 20);

            return 0;
        }
    }
}
