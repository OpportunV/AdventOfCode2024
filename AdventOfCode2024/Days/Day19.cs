using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024.Days;

public class Day19 : Day
{
    private readonly string[] _colors;
    private readonly string[] _towels;
    private readonly Dictionary<string, long> _cache = new();

    public Day19()
    {
        var split = GetInputRaw().Split("\n\n");
        _colors = split[0].Split(", ")
            .OrderByDescending(color => color.Length)
            .ThenByDescending(color => color)
            .ToArray();
        _towels = split[1].Split("\n");
    }

    public override string Part1()
    {
        return _towels.Count(towel => CheckTowel(towel) > 0).ToString();
    }

    public override string Part2()
    {
        return _towels.Sum(CheckTowel).ToString();
    }

    private long CheckTowel(string towel)
    {
        if (_cache.TryGetValue(towel, out var checkTowel))
        {
            return checkTowel;
        }

        if (towel.Length == 0)
        {
            return 1L;
        }

        var current = _colors
            .Where(towel.StartsWith)
            .Sum(color => CheckTowel(towel[color.Length..]));

        _cache[towel] = current;
        return current;
    }
}