using System.Collections.Generic;
using aoc.y2019.intcode;
using aoc.utils.geometry;

namespace aoc.y2019.day15
{
    enum Direction { North = 1, South = 2, West = 3, East = 4 }
    enum Status { Wall = 0, Empty = 1, System = 2 }

    abstract class Solver : aoc.utils.ProblemSolver<int>
    {
        protected Dictionary<Point, Status> visited = new Dictionary<Point, Status>();
        protected Point? oxygenSystem;

        protected void visit(List<Droid> neighbors, Droid droid, Point point, Direction dir) {
            if (visited.ContainsKey(point)) {
                return;
            }

            var copy = new Droid(droid);
            var result = copy.move(dir);

            visited[point] = result;

            if (result == Status.System) {
                oxygenSystem = point;
            }

            if (result == Status.Empty) {
                neighbors.Add(copy);
            }
        }

        protected List<Droid> visitNeighbors(Droid droid) {
            var neighbors = new List<Droid>();

            var north = new Point(droid.loc.x, droid.loc.y + 1);
            var south = new Point(droid.loc.x, droid.loc.y - 1);
            var east = new Point(droid.loc.x + 1, droid.loc.y);
            var west = new Point(droid.loc.x - 1, droid.loc.y);

            visit(neighbors, droid, north, Direction.North);
            visit(neighbors, droid, south, Direction.South);
            visit(neighbors, droid, east, Direction.East);
            visit(neighbors, droid, west, Direction.West);

            return neighbors;
        }

        public Solver(string file, int exp) : base(file, exp) { }
    }

    class Droid : IOHandler
    {
        private Machine machine;
        public Point loc { get; set; }
        private Direction? dir;
        private Status? result;

        public Droid(Droid copy) {
            this.machine = new Machine(copy.machine, this);
            this.loc = new Point(copy.loc.x, copy.loc.y);
        }

        public Droid(long[] prog) {
            machine = new Machine(prog, this);
            loc = new Point(0, 0);
        }

        public Status move(Direction dir) {
            this.dir = dir;
            while (result is null) {
                machine.step();
            }

            var output = (Status)result;
            result = null;

            return output;
        }

        public long input() {
            if (dir is null) {
                throw new System.NotImplementedException("No direction for input");
            }

            return (long)dir;
        }

        public void output(long value) {
            var status = value switch
            {
                0 => Status.Wall,
                1 => Status.Empty,
                2 => Status.System,
                _ => throw new System.Exception($"Invalid output {value}"),
            };

            if (status == Status.Empty) {
                if (dir == Direction.North) {
                    loc.y += 1;
                } else if (dir == Direction.South) {
                    loc.y -= 1;
                } else if (dir == Direction.East) {
                    loc.x += 1;
                } else if (dir == Direction.West) {
                    loc.x -= 1;
                }
            }

            result = status;
        }
    }
}