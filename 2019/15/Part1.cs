using System.Collections.Generic;

namespace aoc.y2019.day15
{
    class Part1 : Solver
    {
        public Part1(string file, int exp) : base(file, exp) { }

        protected override int doWork() {
            var prog = aoc.y2019.intcode.Parser.parseInput(inputFile);
            var droids = new List<Droid>() { new Droid(prog) };
            var nxtDroids = new List<Droid>();
            var dist = 0;

            visited[droids[0].loc] = Status.Empty;

            while (oxygenSystem is null) {
                foreach (var droid in droids) {
                    var next = visitNeighbors(droid);
                    nxtDroids.AddRange(next);
                }

                dist += 1;

                droids = nxtDroids;
                nxtDroids = new List<Droid>();
            }

            return dist;
        }
    }
}
