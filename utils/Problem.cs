namespace aoc.utils
{
    interface Problem
    {
        void run();
    }

    abstract class ProblemSolver<T> : Problem
    {
        protected string inputFile;
        protected T expected;

        protected abstract T doWork();

        public void run()
        {
            var answer = doWork();

            if (!this.expected.Equals(answer))
            {
                throw new System.Exception($"{answer} != {this.expected}");
            }
        }

        protected ProblemSolver(string inputFile, T expected)
        {
            this.inputFile = inputFile;
            this.expected = expected;
        }

    }
}
