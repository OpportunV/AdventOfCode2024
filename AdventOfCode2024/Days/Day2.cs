using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024.Days;

public class Day2 : Day
{
    private const int Threshold = 3;

    public override string Part1()
    {
        var reports = GetReports();

        return reports.Count(CheckReport)
            .ToString();
    }

    public override string Part2()
    {
        var reports = GetReports();

        // Brute-forcing all variants.
        return reports.Count(report => report.Select((_, i) => report
                    .Where((_, index) => index != i)
                    .ToList())
                .Any(CheckReport))
            .ToString();
    }

    private IEnumerable<List<int>> GetReports()
    {
        var inp = GetInput();
        var reports = inp.Select(line => line.Split()
            .Select(int.Parse)
            .ToList());
        return reports;
    }

    private bool CheckReport(List<int> report)
    {
        var delta = report[1] - report[0];
        var sign = Math.Sign(delta);
        if (delta == 0 || Math.Abs(delta) > Threshold)
        {
            return false;
        }

        for (var i = 2; i < report.Count; i++)
        {
            delta = report[i] - report[i - 1];
            if (sign != Math.Sign(delta) || Math.Abs(delta) > Threshold)
            {
                return false;
            }
        }

        return true;
    }
}