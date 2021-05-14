using System.Collections.Generic;

namespace aoc.utils
{
    abstract class Parser
    {
        public static string[] readLines(string fileName)
        {
            return System.IO.File.ReadAllLines(fileName);
        }
    }
}