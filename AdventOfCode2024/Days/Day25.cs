using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024.Days;

public class Day25 : Day
{
    private readonly List<List<int>> _locks = [];
    private readonly List<List<int>> _keys = [];

    public Day25()
    {
        var grids = GetInputRaw().Split("\n\n");

        foreach (var grid in grids)
        {
            var lines = grid.Split("\n");

            var first = lines[0][0];

            var heights = new List<int>();
            for (var col = 0; col < lines[0].Length; col++)
            {
                var row = 1;
                while (lines[row][col] == first)
                {
                    row++;
                }

                heights.Add(row);
            }

            if (first == '#')
            {
                _locks.Add(heights);
            }
            else
            {
                _keys.Add(heights);
            }
        }
    }

    public override string Part1()
    {
        var ans = _locks
            .Sum(@lock => _keys
                .Count(key => @lock.Zip(key)
                    .All(pair => pair.First <= pair.Second)));

        return ans.ToString();
    }

    public override string Part2()
    {
        return "Done!";
    }
}