using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Helpers;

namespace AdventOfCode2024.Days;

public class Day7 : Day
{
    private readonly List<List<long>> _equations;

    public Day7()
    {
        _equations = GetInput()
            .Select(line => Regex.Matches(line, @"\d+")
                .Select(m => long.Parse(m.Value))
                .ToList())
            .ToList();
    }

    public override string Part1()
    {
        var operators = new Dictionary<char, Func<long, long, long>>
        {
            ['+'] = (x, y) => x + y,
            ['*'] = (x, y) => x * y
        };

        return _equations.Sum(list => EquationResult(list, operators)).ToString();
    }

    public override string Part2()
    {
        var operators = new Dictionary<char, Func<long, long, long>>
        {
            ['+'] = (x, y) => x + y,
            ['*'] = (x, y) => x * y,
            ['|'] = Concatenate
        };

        return _equations.Sum(list => EquationResult(list, operators)).ToString();
    }

    private long EquationResult(List<long> equation, Dictionary<char, Func<long, long, long>> operators)
    {
        return IsValid(equation, operators) ? equation[0] : 0;
    }

    private bool IsValid(List<long> equation, Dictionary<char, Func<long, long, long>> operators)
    {
        var target = equation[0];
        var other = equation[1..];
        var combinations = Combinations.GeneratePermutations(operators.Keys.ToList(), other.Count - 1);

        foreach (var combination in combinations)
        {
            var current = other[0];
            var index = 1;
            foreach (var op in combination)
            {
                current = operators[op].Invoke(current, other[index++]);

                if (current > target)
                {
                    break;
                }
            }

            if (target == current)
            {
                return true;
            }
        }

        return false;
    }

    private long Concatenate(long first, long second)
    {
        // This is faster than long.Parse($"{first}{second}") 
        long factor = 1;
        while (factor <= second)
        {
            factor *= 10;
        }

        return first * factor + second;
    }
}