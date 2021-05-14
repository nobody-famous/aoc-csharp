using System;
using System.Collections.Generic;
using aoc.utils;

namespace aoc_csharp
{
    class Runner
    {
        static readonly List<Problem> probs = new List<Problem>(){
            new aoc.y2019.day1.Part1("input/2019/1/puzzle.txt", 3279287),
            new aoc.y2019.day1.Part2("input/2019/1/puzzle.txt", 4916076),
        };

        static void runAll(List<Problem> toRun)
        {
            var total = 0;

            foreach (var prob in toRun)
            {
                var start = DateTime.Now.Millisecond;

                try
                {
                    prob.run();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"{prob} FAILED: {ex.Message}");
                }

                var diff = DateTime.Now.Millisecond - start;
                total += diff;

                Console.WriteLine($"{prob} {diff} ms");
            }

            Console.WriteLine($"Total {total} ms");
        }

        static void Main(string[] args)
        {
            // var probs = new List<Problem>() { new aoc.y2019.day1.Part2("input/2019/1/puzzle.txt", 4916076) };

            runAll(probs);
        }
    }
}
