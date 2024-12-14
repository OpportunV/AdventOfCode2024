using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2024.Models;
using Common.Models;

namespace AdventOfCode2024.Days;

public class Day14 : Day
{
    private const int Rows = 101;
    private const int Cols = 103;
    private const int MidRow = (Rows - 1) / 2;
    private const int MidCol = (Cols - 1) / 2;

    public override string Part1()
    {
        var robots = GetRobots();
        for (var i = 0; i < 100; i++)
        {
            foreach (var robot in robots)
            {
                robot.Move(Rows, Cols);
            }
        }

        return GetSafetyFactor(robots).ToString();
    }

    public override string Part2()
    {
        var robots = GetRobots();
        var second = 0;
        while (true)
        {
            foreach (var robot in robots)
            {
                robot.Move(Rows, Cols);
            }

            second++;

            if (IsTreeFormation(robots))
            {
                break;
            }
        }

        return second.ToString();
    }

    private static bool IsTreeFormation(HashSet<Robot> robots)
    {
        return robots.Select(robot => robot.Pos).ToHashSet().Count == robots.Count;
    }

    private static int GetSafetyFactor(HashSet<Robot> robots)
    {
        var quadrantCount = new Dictionary<int, int>
        {
            [0] = 0,
            [1] = 0,
            [2] = 0,
            [3] = 0
        };

        foreach (var robot in robots)
        {
            var quadrant = GetQuadrant(robot.Pos);
            if (quadrant == -1)
            {
                continue;
            }

            quadrantCount[quadrant] += 1;
        }

        return quadrantCount.Values.Aggregate(1, (current, quadrant) => current * quadrant);
    }

    private static int GetQuadrant(GridPos2d pos)
    {
        return pos.Row switch
        {
            < MidRow when pos.Col < MidCol => 0,
            < MidRow when pos.Col > MidCol => 1,
            > MidRow when pos.Col < MidCol => 2,
            > MidRow when pos.Col > MidCol => 3,
            _ => -1
        };
    }

    private HashSet<Robot> GetRobots()
    {
        return GetInput()
            .Select(line => Regex.Matches(line, @"-?\d+")
                .Select(match => int.Parse(match.Value))
                .ToList())
            .Select(values => new Robot(values))
            .ToHashSet();
    }
}