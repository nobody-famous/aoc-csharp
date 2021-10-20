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
                "NOT A T",
                "NOT B J",
                "OR T J",
                "NOT C T",
                "OR T J",
                "NOT E T",
                "AND H T",
                "OR E T",
                "AND T J",
                "AND D J",
             };
        }
    }
}
