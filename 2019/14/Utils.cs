using System.Collections.Generic;

namespace aoc.y2019.day14
{
    record Chemical(int amount, string name);

    record Reaction(List<Chemical> input, Chemical output);
}