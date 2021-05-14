namespace aoc.y2019.day1
{
    class Part2 : Solver
    {
        public Part2(string fileName, int expected) : base(fileName, expected) { }

        private int calcFuel(int mass) {
            var total = 0;

            var fuel = findFuel(mass);
            while (fuel > 0) {
                total += fuel;
                fuel = findFuel(fuel);
            }

            return total;
        }

        protected override int doWork() {
            var masses = Parser.parseInput(this.inputFile);
            var total = 0;

            foreach (var mass in masses) {
                total += calcFuel(mass);
            }

            return total;
        }
    }
}