using System;
using System.Collections.Generic;
using System.Linq;
using Common.Helpers;
using Common.Models;

namespace AdventOfCode2024.Days;

public class Day16 : Day
{
    private readonly Grid<char> _grid;
    private readonly int _directionsCount = Directions2d.Side.Count;
    private int _bestPathsCount = -1;

    private const int RotationPoints = 1000;
    private const int MovingPoints = 1;
    private const int Start = 'S';
    private const int End = 'E';
    private const int Wall = '#';

    public Day16()
    {
        var grid = GetInput().Select(line => line.ToCharArray()).ToArray();
        _grid = new Grid<char>(grid);
    }

    public override string Part1()
    {
        var startPos = _grid.Flatten().First(item => item.Value == Start).Pos;

        return CalculateBestScore(startPos, 0).ToString();
    }

    public override string Part2()
    {
        var startPos = _grid.Flatten().First(item => item.Value == Start).Pos;
        if (_bestPathsCount == -1)
        {
            CalculateBestScore(startPos, 0);
        }

        return _bestPathsCount.ToString();
    }

    private int CalculateBestScore(GridPos2d pos, int dir)
    {
        var toVisit = new PriorityQueue<(GridPos2d pos, int dir, HashSet<GridPos2d> path), int>();
        var path = new HashSet<GridPos2d> { pos };
        var bestPaths = new HashSet<GridPos2d>();
        toVisit.Enqueue((pos, dir, path), 0);
        var seen = new Dictionary<(GridPos2d pos, int dir), int>();
        var best = int.MaxValue;

        while (toVisit.TryDequeue(out var item, out var curScore))
        {
            var (curPos, curDir, curPath) = item;

            if (curScore > best)
            {
                break;
            }

            if (seen.TryGetValue((curPos, curDir), out var score) && score < curScore)
            {
                continue;
            }

            seen[(curPos, curDir)] = curScore;

            if (_grid[curPos] == End)
            {
                best = Math.Min(best, curScore);
                bestPaths.UnionWith(curPath);
                continue;
            }

            var newPos = curPos + Directions2d.Side[curDir];
            if (_grid[newPos] != Wall)
            {
                toVisit.Enqueue((newPos, curDir, [..curPath, newPos]), curScore + MovingPoints);
            }

            var newDir = RotateLeft(curDir);
            toVisit.Enqueue((curPos, newDir, curPath), curScore + RotationPoints);

            newDir = RotateRight(curDir);
            toVisit.Enqueue((curPos, newDir, curPath), curScore + RotationPoints);
        }

        _bestPathsCount = bestPaths.Count;

        return best;
    }

    private int RotateLeft(int curDir)
    {
        return (curDir - 1 + _directionsCount) % _directionsCount;
    }

    private int RotateRight(int curDir)
    {
        return (curDir + 1) % _directionsCount;
    }
}