using aoc.intcode;
using aoc.utils.geometry;

namespace aoc.y2019.day15
{
    enum Direction { North = 1, South = 2, West = 3, East = 4 }
    enum Status { Wall = 0, Moved = 1, System = 2 }

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
                1 => Status.Moved,
                2 => Status.System,
                _ => throw new System.Exception($"Invalid output {value}"),
            };

            if (status == Status.Moved) {
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