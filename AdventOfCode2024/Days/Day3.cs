using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

public class Day3 : Day
{
    private readonly Regex _regexMul = new(@"mul\((\d{1,3}),(\d{1,3})\)", RegexOptions.Compiled);

    private readonly Regex _regexDo = new(@"do\(\)[^d]*(?:d(?!on't\(\))[^d]*)*mul\((\d{1,3}),(\d{1,3})\)",
        RegexOptions.Compiled);

    private const string Do = "do()";

    public override string Part1()
    {
        var inp = GetInputRaw();
        var sum = _regexMul.Matches(inp)
            .Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));

        return sum.ToString();
    }

    public override string Part2()
    {
        var inp = Do + GetInputRaw();

        var sum = _regexDo.Matches(inp)
            .Sum(match => _regexMul.Matches(match.Groups[0].Value)
            .Sum(match2 => int.Parse(match2.Groups[1].Value) * int.Parse(match2.Groups[2].Value)));

        return sum.ToString();
    }
}