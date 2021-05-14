namespace aoc.y2019.day4
{
    class Parser
    {
        public static (int[], int[]) parseInput(string fileName) {
            var lines = aoc.utils.Parser.readLines(fileName);
            var pieces = lines[0].Split('-');
            var start = pieces[0].ToCharArray();
            var end = pieces[0].ToCharArray();

            return (System.Array.ConvertAll(start, ch => ch - '0'),
                    System.Array.ConvertAll(end, ch => ch - '0')
            );
        }
    }
}