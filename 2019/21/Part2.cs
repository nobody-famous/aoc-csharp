namespace aoc.y2019.day21
{
    class Part2 : Solver
    {
        public Part2(string fileName, int exp) : base(fileName, exp) { }

        override protected string getSpeed() {
            return "RUN";
        }

        protected override string[] getCommands() {
            return new string[] {
                "NOT A J",
                "NOT B T",
                "OR T J",
                "NOT C T",
                "OR T J",
                "AND D J",
             };
        }
    }
}