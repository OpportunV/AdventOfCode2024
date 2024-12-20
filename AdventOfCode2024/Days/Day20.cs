using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace AdventOfCode2024.Days;

public class Day20 : Day
{
    private readonly Grid<char> _grid;
    private readonly Dictionary<GridPos2d, int> _path;

    private const int Start = 'S';
    private const int End = 'E';
    private const int Wall = '#';
    private const int Threshold = 100;
    private const int Part1CheatTime = 2;
    private const int Part2CheatTime = 20;

    public Day20()
    {
        var grid = GetInput().Select(line => line.ToCharArray()).ToArray();
        _grid = new Grid<char>(grid);
        var startPos = _grid.Flatten().First(item => item.Value == Start).Pos;
        _path = GetPath(startPos);
    }

    public override string Part1()
    {
        var counter = new Dictionary<int, int>();
        foreach (var (pos, time) in _path)
        {
            CheckCheatsFromPos(pos, time, counter, Part1CheatTime);
        }

        return counter.Where(pair => pair.Key >= Threshold).Sum(pair => pair.Value).ToString();
    }

    public override string Part2()
    {
        var counter = new Dictionary<int, int>();
        foreach (var (pos, time) in _path)
        {
            CheckCheatsFromPos(pos, time, counter, Part2CheatTime);
        }

        return counter.Where(pair => pair.Key >= Threshold).Sum(pair => pair.Value).ToString();
    }

    private void CheckCheatsFromPos(GridPos2d pos, int time, Dictionary<int, int> counter, int maxDistance)
    {
        foreach (var (possibleExit, dist) in AdjacentSquare(pos, maxDistance))
        {
            if (_grid[possibleExit] == Wall)
            {
                continue;
            }

            var timeDelta = _path[possibleExit] - time - dist;
            if (timeDelta <= 0)
            {
                continue;
            }

            counter.TryAdd(timeDelta, 0);
            counter[timeDelta]++;
        }
    }

    private IEnumerable<(GridPos2d pos, int dist)> AdjacentSquare(GridPos2d pos, int manhattanDistance)
    {
        var i = manhattanDistance;
        var j = 0;
        while (j <= manhattanDistance)
        {
            for (var k = -i; k <= i; k++)
            {
                if (j == 0 && k == 0)
                {
                    continue;
                }

                var newPos = pos + new GridPos2d(k, j);
                var dist = Math.Abs(k) + j;
                if (_grid.Contains(newPos))
                {
                    yield return (newPos, dist);
                }

                if (j == 0)
                {
                    continue;
                }

                newPos = pos + new GridPos2d(k, -j);

                if (_grid.Contains(newPos))
                {
                    yield return (newPos, dist);
                }
            }

            i--;
            j++;
        }
    }

    private Dictionary<GridPos2d, int> GetPath(GridPos2d start)
    {
        var toVisit = new Queue<(GridPos2d pos, int time)>();
        var seen = new HashSet<GridPos2d>();
        var path = new Dictionary<GridPos2d, int>();
        toVisit.Enqueue((start, 0));
        while (toVisit.TryDequeue(out var cur))
        {
            var (pos, time) = cur;

            path[pos] = time;
            if (_grid[pos] == End)
            {
                break;
            }

            if (!seen.Add(pos))
            {
                continue;
            }

            foreach (var (nextVal, nextPos) in _grid.AdjacentSide(pos))
            {
                if (nextVal == Wall)
                {
                    continue;
                }

                if (path.TryGetValue(nextPos, out var nextTime))
                {
                    if (nextTime >= time)
                    {
                        toVisit.Enqueue((nextPos, time + 1));
                    }
                }
                else
                {
                    toVisit.Enqueue((nextPos, time + 1));
                }
            }
        }

        return path;
    }
}