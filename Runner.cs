﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using aoc.utils;

namespace aoc_csharp
{
    class Runner
    {
        static readonly List<Problem> probs = new List<Problem>(){
            new aoc.y2019.day1.Part1("input/2019/1/puzzle.txt", 3279287),
            new aoc.y2019.day1.Part2("input/2019/1/puzzle.txt", 4916076),
            new aoc.y2019.day2.Part1("input/2019/2/puzzle.txt", 3101844),
            new aoc.y2019.day2.Part2("input/2019/2/puzzle.txt", 8478),
            new aoc.y2019.day3.Part1("input/2019/3/puzzle.txt", 529),
            new aoc.y2019.day3.Part2("input/2019/3/puzzle.txt", 20386),
            new aoc.y2019.day4.Part1("input/2019/4/puzzle.txt", 511),
            new aoc.y2019.day4.Part2("input/2019/4/puzzle.txt", 316),
            new aoc.y2019.day5.Part1("input/2019/5/puzzle.txt", 13294380),
            new aoc.y2019.day5.Part2("input/2019/5/puzzle.txt", 11460760),
            new aoc.y2019.day6.Part1("input/2019/6/puzzle.txt", 162816),
            new aoc.y2019.day6.Part2("input/2019/6/puzzle.txt", 304),
            new aoc.y2019.day7.Part1("input/2019/7/puzzle.txt", 21760),
            new aoc.y2019.day7.Part2("input/2019/7/puzzle.txt", 69816958),
            new aoc.y2019.day8.Part1("input/2019/8/puzzle.txt", 2356, 25, 6),
            new aoc.y2019.day8.Part2("input/2019/8/puzzle.txt", "PZEKB", 25, 6),
            new aoc.y2019.day9.Part1("input/2019/9/puzzle.txt", 2870072642),
            new aoc.y2019.day9.Part2("input/2019/9/puzzle.txt", 58534),
            new aoc.y2019.day10.Part1("input/2019/10/puzzle.txt", 347),
          };

        static void runAll(List<Problem> toRun) {
            var total = 0L;

            foreach (var prob in toRun) {
                var watch = new Stopwatch();

                watch.Start();

                try {
                    prob.run();
                } catch (System.Exception ex) {
                    Console.WriteLine($"{prob} FAILED: {ex.Message}");
                }

                watch.Stop();
                total += watch.ElapsedMilliseconds;

                Console.WriteLine($"{prob} {watch.ElapsedMilliseconds} ms");
            }

            Console.WriteLine($"Total {total} ms");
        }

        static void Main(string[] args) {
            var probs = new List<Problem>() { new aoc.y2019.day10.Part2("input/2019/10/puzzle.txt", 829) };

            runAll(probs);
        }
    }
}