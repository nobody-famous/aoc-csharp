namespace aoc.y2019.day16
{
    class Part2 : Solver
    {
        public Part2(string file, long exp) : base(file, exp) { }

        private int[] getSignal(int[] input, int repeat) {
            var signal = new int[input.Length * repeat];
            var ndx = 0;

            for (var loop = 0; loop < repeat; loop += 1) {
                System.Array.Copy(input, 0, signal, ndx, input.Length);
                ndx += input.Length;
            }

            return signal;
        }

        private void reverseSum(int[] signal, long offset) {
            for (var ndx = signal.Length - 2; ndx >= offset; ndx -= 1) {
                var value = signal[ndx] + signal[ndx + 1];
                signal[ndx] = System.Math.Abs(value % 10);
            }
        }

        protected override long doWork() {
            var input = Parser.parseInput(inputFile);
            var signal = getSignal(input, 10000);
            var offset = getValue(signal, 7);

            for (var loop = 0; loop < 100; loop += 1) {
                reverseSum(signal, offset);
            }

            return getValue(signal, 8, offset);
        }
    }
}
