using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace AdventOfCode2024.Days;

public class Day6 : Day
{
    private readonly Grid<char> _grid;

    private static readonly List<GridPos2d> _directionsSide =
    [
        new(-1, 0),
        new(0, 1),
        new(1, 0),
        new(0, -1)
    ];

    public Day6()
    {
        var grid = GetInput().Select(line => line.ToCharArray()).ToArray();
        _grid = new Grid<char>(grid);
    }

    public override string Part1()
    {
        var positions = GetPossiblePositions();
        return positions.Count.ToString();
    }

    public override string Part2()
    {
        var positions = GetPossiblePositions();
        var counter = 0;
        var seen = new HashSet<(GridPos2d, int)>();
        foreach (var (row, col) in positions)
        {
            var pos = GetStartPos();
            if (row == pos.Row && col == pos.Col)
            {
                continue;
            }

            _grid[row, col] = '#';
            var dirIndex = 0;
            seen.Clear();
            while (true)
            {
                if (!seen.Add((pos, dirIndex)))
                {
                    counter++;
                    break;
                }

                if (!TrySimulateStep(ref pos, ref dirIndex))
                {
                    break;
                }
            }

            _grid[row, col] = '.';
        }

        return counter.ToString();
    }

    private HashSet<GridPos2d> GetPossiblePositions()
    {
        var pos = GetStartPos();
        var dirIndex = 0;
        var seen = new HashSet<GridPos2d>();

        while (true)
        {
            seen.Add(pos);
            if (!TrySimulateStep(ref pos, ref dirIndex))
            {
                break;
            }
        }

        return seen;
    }

    private bool TrySimulateStep(ref GridPos2d pos, ref int dirIndex)
    {
        var newPos = pos + _directionsSide[dirIndex];
        if (!IsInside(newPos))
        {
            return false;
        }

        if (_grid[newPos] == '#')
        {
            dirIndex = NextDirIndex(dirIndex);
        }
        else
        {
            pos = newPos;
        }

        return true;
    }

    private int NextDirIndex(int dirIndex)
    {
        return (dirIndex + 1) % _directionsSide.Count;
    }

    private bool IsInside(GridPos2d pos)
    {
        return pos.IsInside(_grid);
    }

    private GridPos2d GetStartPos()
    {
        var start = _grid.LinearIterationPairs().FirstOrDefault(item => item.Value == '^');
        if (start is not null)
        {
            return start.Pos;
        }

        throw new ArgumentOutOfRangeException(nameof(_grid), "Starting position is not found");
    }
}