using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

public class Day1 : Day
{
    public override string Part1()
    {
        GetTwoLists(out var left, out var right);
        left.Sort();
        right.Sort();
        return left.Zip(right)
            .Select(pair => Math.Abs(pair.Item1 - pair.Item2))
            .Sum()
            .ToString();
    }

    public override string Part2()
    {
        GetTwoLists(out var left, out var right);
        var rightLookup = right.GroupBy(num => num)
            .ToDictionary(group => group.Key, group => group.Count());

        return left.Select(num => num * rightLookup.GetValueOrDefault(num, 0))
            .Sum()
            .ToString();
    }

    private void GetTwoLists(out List<int> left, out List<int> right)
    {
        var inp = GetInput();
        left = new List<int>(inp.Length);
        right = new List<int>(inp.Length);
        foreach (var item in inp)
        {
            var nums = Regex.Matches(item, @"\d+")
                .Select(match => int.Parse(match.Value))
                .ToList();

            left.Add(nums[0]);
            right.Add(nums[1]);
        }
    }
}