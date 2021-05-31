namespace aoc.y2019.day14
{
    class Part2 : Solver
    {
        public Part2(string file, long exp) : base(file, exp) { }

        private long getMid(long low, long high) {
            return low + ((high - low) / 2);
        }

        protected override long doWork() {
            var reacts = Parser.parseInput(inputFile);
            var target = 1000000000000L;
            var low = 0L;
            var high = target;
            var mid = getMid(low, high);

            while (low < mid) {
                var ore = computeOre(reacts, mid);

                if (ore > target) {
                    high = mid;
                } else {
                    low = mid;
                }

                mid = getMid(low, high);
            }

            return mid;
        }
    }
}
