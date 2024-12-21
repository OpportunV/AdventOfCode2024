using System;
using System.Collections.Generic;
using System.Linq;
using Common.Helpers;
using Common.Models;

namespace AdventOfCode2024.Days;

public class Day21 : Day
{
    private readonly string[] _codes;

    private readonly Grid<char> _numGrid = new(new char[][]
    {
        ['7', '8', '9'],
        ['4', '5', '6'],
        ['1', '2', '3'],
        ['#', '0', 'A']
    });

    private readonly Grid<GridPos2d> _dirGrid = new(new GridPos2d[][]
    {
        [GridPos2d.One, Directions2d.Side[3], GridPos2d.Zero],
        [Directions2d.Side[2], Directions2d.Side[1], Directions2d.Side[0]]
    });

    private readonly Dictionary<GridPos2d, Dictionary<GridPos2d, int>> _dirCosts = new();
    private readonly GridPos2d _dirStartPos;
    private readonly GridPos2d _forbiddenDirPad = GridPos2d.One;

    private readonly
        Dictionary<Type, Dictionary<(GridPos2d startPos, GridPos2d startDir, GridPos2d target, int depth), long>>
        _caches = new();

    private const int NumRobotsPart1 = 2;
    private const int NumRobotsPart2 = 25;
    private const char Start = 'A';
    private const char ForbiddenNumpad = '#';
    private const string Separator = "|";

    public Day21()
    {
        _dirStartPos = _dirGrid.Flatten().First(item => item.Value == GridPos2d.Zero).Pos;
        _codes = GetInput();
        foreach (var dir1 in _dirGrid.Flatten())
        {
            foreach (var dir2 in _dirGrid.Flatten())
            {
                _dirCosts.TryAdd(dir1.Value, new Dictionary<GridPos2d, int>());
                _dirCosts[dir1.Value][dir2.Value] = dir1.Pos.ManhattanTo(dir2.Pos);
            }
        }
    }

    public override string Part1()
    {
        var ans = GetComplexitiesSum(NumRobotsPart1);

        return ans.ToString();
    }

    public override string Part2()
    {
        var ans = GetComplexitiesSum(NumRobotsPart2);

        return ans.ToString();
    }

    private long GetComplexitiesSum(int numRobots)
    {
        var startPos = _numGrid.Flatten().First(item => item.Value == Start).Pos;
        var ans = 0L;
        foreach (var code in _codes)
        {
            var total = Directions2d.Side.Select(startDir =>
                GetDirs(code.ToList(), _numGrid, startPos, startDir, ForbiddenNumpad, numRobots)).Min();

            ans += total * long.Parse(code[..^1]);
        }

        return ans;
    }

    private long GetDirs<T>(List<T> targets, Grid<T> grid, GridPos2d startPos, GridPos2d startDir, T forbidden,
        int depth)
    {
        var type = typeof(T);
        _caches.TryAdd(type,
            new Dictionary<(GridPos2d startPos, GridPos2d startDir, GridPos2d target, int depth), long>());
        var cache = _caches[type];

        var total = 0L;
        foreach (var target in targets)
        {
            var targetPos = grid.Flatten().First(item => item.Value.Equals(target)).Pos;
            if (cache.TryGetValue((startPos, startDir, targetPos, depth), out var val))
            {
                total += val;
                startPos = targetPos;
                continue;
            }

            var toVisit = new PriorityQueue<(GridPos2d pos, GridPos2d dir, string dirs), int>();
            toVisit.Enqueue((startPos, startDir, string.Empty), 0);
            var seen = new HashSet<GridPos2d>();

            var current = 0L;
            while (toVisit.TryDequeue(out var cur, out var dist))
            {
                var (pos, dir, dirs) = cur;

                if (grid[pos].Equals(target))
                {
                    var curDirs = dirs.Split(Separator, StringSplitOptions.RemoveEmptyEntries)
                        .Select(GridPos2d.Parse)
                        .ToList();
                    curDirs.Add(GridPos2d.Zero);
                    current = depth == 0
                        ? curDirs.Count
                        : Directions2d.Side.Select(newDir => GetDirs(curDirs, _dirGrid, _dirStartPos,
                            newDir, _forbiddenDirPad, depth - 1)).Min();
                    cache[(startPos, startDir, pos, depth)] = current;
                    startPos = pos;

                    break;
                }

                if (!seen.Add(pos))
                {
                    continue;
                }

                foreach (var (nextVal, nextPos) in grid.AdjacentSide(pos))
                {
                    if (nextVal.Equals(forbidden))
                    {
                        continue;
                    }

                    var nextDir = nextPos - pos;
                    var cost = dist + 1 + _dirCosts[nextDir][dir];
                    toVisit.Enqueue((nextPos, nextDir, string.Join(Separator, dirs, nextDir.ToString())), cost);
                }
            }

            total += current;
        }

        return total;
    }
}