using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

public class Day5 : Day
{
    private readonly List<List<int>> _updates;
    private readonly Dictionary<int, HashSet<int>> _rules;

    public Day5()
    {
        (_rules, _updates) = GetData();
    }

    public override string Part1()
    {
        var counter = 0;
        foreach (var update in _updates)
        {
            if (CheckUpdate(update, out var middle))
            {
                counter += middle;
            }
        }

        return counter.ToString();
    }

    public override string Part2()
    {
        var counter = 0;
        foreach (var update in _updates)
        {
            if (CheckUpdate(update, out _))
            {
                continue;
            }

            var corrected = update.OrderByDescending(page =>
                    _rules.TryGetValue(page, out var rules) ? rules.Intersect(update).Count() : 0)
                .ToList();

            counter += corrected[corrected.Count / 2];
        }

        return counter.ToString();
    }

    private bool CheckUpdate(List<int> update, out int middle)
    {
        var seen = new HashSet<int>();
        middle = update[update.Count / 2];
        foreach (var page in update)
        {
            if (_rules.TryGetValue(page, out var currentRule))
            {
                if (seen.Intersect(currentRule).Any())
                {
                    return false;
                }
            }

            seen.Add(page);
        }

        return true;
    }

    private (Dictionary<int, HashSet<int>> rules, List<List<int>> updates) GetData()
    {
        var inp = GetInputRaw();
        var rulePairs = Regex.Matches(inp, @"(\d+)\|(\d+)").Select(match =>
            (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)));

        var rules = new Dictionary<int, HashSet<int>>();
        foreach (var (before, after) in rulePairs)
        {
            if (!rules.ContainsKey(before))
            {
                rules[before] = [];
            }

            rules[before].Add(after);
        }

        var updates = Regex.Matches(inp, @"\d+,.*\d+")
            .Select(match => match.Groups[0].Value.Split(",")
                .Select(int.Parse)
                .ToList())
            .ToList();
        return (rules, updates);
    }
}