using System.Collections.Generic;
using aoc.y2019.intcode;
using aoc.utils.geometry;

namespace aoc.y2019.day17
{
    record PathStep(char turn, int dist);

    class Routines
    {
        public string? A { get; set; }
        public string? B { get; set; }
        public string? C { get; set; }
        public string? main { get; set; }

        public Routines(Routines copy) {
            this.A = copy.A;
            this.B = copy.B;
            this.C = copy.C;
            this.main = copy.main;
        }

        public Routines(string? A, string? B, string? C, string? main) {
            this.A = A;
            this.B = B;
            this.C = C;
            this.main = main;
        }

        public bool assign(string str) {
            if (A is null || str.Equals(A)) {
                A = str;
                main = main is null ? "A" : $"{main},A";

                return true;
            } else if (B is null || str.Equals(B)) {
                B = str;
                main = main is null ? "B" : $"{main},B";

                return true;
            } else if (C is null || str.Equals(C)) {
                C = str;
                main = main is null ? "C" : $"{main},C";

                return true;
            }

            return false;
        }

        public override string ToString() {
            return $"[{A}, {B} ,{C} ,{main}]";
        }
    }

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

                for (var nxt = ndx + 1; nxt < steps.Count; nxt += 1) {
                    var nxtStep = steps[nxt];
                    var nxtStr = $"{str},{nxtStep.turn},{nxtStep.dist}";

                    if (nxtStr.Length > maxLength) {
                        break;
                    }

                    opts[ndx].Add(nxtStr);
                    str = nxtStr;
                }
            }

            return opts;
        }

        private bool hasAllRoutines(Routines routines) {
            return routines.A is not null
                && routines.B is not null
                && routines.C is not null
                && routines.main is not null;
        }

        private Routines? getRoutines(List<List<string>> opts, int ndx, Routines cur) {
            if (ndx >= opts.Count) {
                return hasAllRoutines(cur) ? cur : null;
            }

            var strs = opts[ndx];
            for (var strsNdx = strs.Count - 1; strsNdx >= 0; strsNdx -= 1) {
                var copy = new Routines(cur);
                var str = strs[strsNdx];

                if (!copy.assign(str)) {
                    continue;
                }

                var nxt = getRoutines(opts, ndx + strsNdx + 1, copy);
                if (nxt is Routines r) {
                    return r;
                }
            }

            return null;
        }

        private Routines? getRoutines(List<List<string>> opts) {
            return getRoutines(opts, 0, new Routines(null, null, null, null));
        }

        private void printOpts(List<List<string>> opts) {
            System.Console.WriteLine("OPTS");
            for (var ndx = 0; ndx < opts.Count; ndx += 1) {
                var strs = opts[ndx];
                System.Console.WriteLine($"{ndx}: {string.Join(", ", strs)}");
            }
        }

        private void sendRoutines(Robot robot, Routines routines) {
            var sentMain = false;
            var sentA = false;
            var sentB = false;
            var sentC = false;

            if (routines.main is null || routines.A is null || routines.B is null || routines.C is null) {
                throw new System.Exception("Routines not set");
            }

            while (!sentMain || !sentA || !sentB || !sentC) {
                var prompt = robot.getPrompt();

                if (prompt == "Main:") {
                    robot.send(routines.main);
                    sentMain = true;
                } else if (prompt == "Function A:") {
                    robot.send(routines.A);
                    sentA = true;
                } else if (prompt == "Function B:") {
                    robot.send(routines.B);
                    sentB = true;
                } else if (prompt == "Function C:") {
                    robot.send(routines.C);
                    sentC = true;
                }
            }
        }

        protected override int doWork() {
            var prog = Parser.parseInput(inputFile);
            prog[0] = 2;

            var robot = new Robot(prog);

            robot.getScaffold();

            var steps = new PathBuilder(robot).build();
            var opts = getOptions(steps, 20);
            var routines = getRoutines(opts);

            if (routines is null) {
                return 0;
            }

            sendRoutines(robot, routines);

            var prompt = robot.getPrompt();
            if ("Continuous video feed?".Equals(prompt)) {
                robot.send("n");
            } else {
                throw new System.Exception($"NOT EXPECTED: {prompt}");
            }

            robot.getScaffold();

            return (int)robot.getDust();
        }
    }
}
