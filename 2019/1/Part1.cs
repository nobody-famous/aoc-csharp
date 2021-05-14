namespace aoc.y2019.day1
{
    class Part1 : Solver
    {
        public Part1(string inputFile, int expected) : base(inputFile, expected) { }

        protected override int doWork() {
            var masses = Parser.parseInput(this.inputFile);
            var total = 0;

            foreach (var mass in masses) {
                total += findFuel(mass);
            }

            return total;
        }
    }
}
