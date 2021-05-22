namespace aoc.y2019.day10
{
    class Part2 : Solver
    {
        public Part2(string file, int exp) : base(file, exp) { }

        protected override int doWork() {
            var asteroids = Parser.parseInput(inputFile);
            var vis = mostVisible(asteroids);

            System.Console.WriteLine($"{vis.pt} {vis.count}");

            return 0;
        }
    }
}
