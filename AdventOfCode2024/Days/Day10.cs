using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace AdventOfCode2024.Days;

public class Day10 : Day
{
    private readonly Grid<int> _grid;

    private const int StepSize = 1;
    private const int Target = 9;

    public Day10()
    {
        var grid = GetInput().Select(line => line.Select(chr => chr - '0').ToArray()).ToArray();
        _grid = new Grid<int>(grid);
    }

    public override string Part1()
    {
        return _grid
            .Flatten()
            .Select(pair => pair.Value == 0 ? Score(GetTrailheadEnds(pair.Pos)) : 0)
            .Sum()
            .ToString();
    }

    public override string Part2()
    {
        return _grid
            .Flatten()
            .Select(pair => pair.Value == 0 ? Rating(GetTrailheadEnds(pair.Pos)) : 0)
            .Sum()
            .ToString();
    }

    private static int Score(List<GridPos2d> ends)
    {
        return new HashSet<GridPos2d>(ends).Count;
    }

    private static int Rating(List<GridPos2d> ends)
    {
        return ends.Count;
    }

    private List<GridPos2d> GetTrailheadEnds(GridPos2d pos)
    {
        var toVisit = new Queue<GridPos2d>();
        toVisit.Enqueue(pos);

        var visited = new List<GridPos2d>();
        while (toVisit.TryDequeue(out var current))
        {
            var value = _grid[current];
            if (value == Target)
            {
                visited.Add(current);
                continue;
            }

            foreach (var (val, next) in _grid.AdjacentSide(current))
            {
                if (val - value == StepSize)
                {
                    toVisit.Enqueue(next);
                }
            }
        }

        return visited;
    }
}