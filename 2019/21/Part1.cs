namespace aoc.y2019.day21
{
    class Part1 : Solver
    {
        public Part1(string fileName, int exp) : base(fileName, exp) { }

        override protected string getSpeed() {
            return "WALK";
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