using System.Collections.Generic;
using System.Linq;
using Common.Helpers;
using Common.Models;

namespace AdventOfCode2024.Days;

public class Day12 : Day
{
    private readonly Grid<char> _grid;
    private const int MaxPerimeter = 4;

    public Day12()
    {
        var grid = GetInput().Select(line => line.ToCharArray()).ToArray();
        _grid = new Grid<char>(grid);
    }

    public override string Part1()
    {
        var seen = new HashSet<GridPos2d>();

        var counter = 0;
        foreach (var gridItem in _grid.Flatten())
        {
            if (seen.Contains(gridItem.Pos))
            {
                continue;
            }

            var visited = GetRegion(gridItem, out var perimeter);
            counter += perimeter * visited.Count;
            seen.UnionWith(visited);
        }

        return counter.ToString();
    }

    public override string Part2()
    {
        var seen = new HashSet<GridPos2d>();

        var counter = 0;
        foreach (var gridItem in _grid.Flatten())
        {
            if (seen.Contains(gridItem.Pos))
            {
                continue;
            }

            var visited = GetRegion(gridItem, out _);
            var sides = CountSides(visited);
            counter += sides * visited.Count;
            seen.UnionWith(visited);
        }

        return counter.ToString();
    }

    private HashSet<GridPos2d> GetRegion(GridItem<char> gridItem, out int perimeter)
    {
        var toVisit = new Queue<GridItem<char>>();
        toVisit.Enqueue(gridItem);
        perimeter = 0;
        var visited = new HashSet<GridPos2d>();
        while (toVisit.TryDequeue(out var current))
        {
            if (visited.Contains(current.Pos))
            {
                continue;
            }

            var currentItemPerimeter = MaxPerimeter;
            visited.Add(current.Pos);
            foreach (var next in _grid.AdjacentSide(current))
            {
                if (next.Value != current.Value)
                {
                    continue;
                }

                currentItemPerimeter--;
                if (visited.Contains(next.Pos))
                {
                    continue;
                }

                toVisit.Enqueue(next);
            }

            perimeter += currentItemPerimeter;
        }

        return visited;
    }

    private int CountSides(HashSet<GridPos2d> region)
    {
        var sides = 0;
        foreach (var pos in region)
        {
            var right = region.Contains(pos + Directions2d.Side[0]);
            var down = region.Contains(pos + Directions2d.Side[1]);
            var left = region.Contains(pos + Directions2d.Side[2]);
            var up = region.Contains(pos + Directions2d.Side[3]);

            var downRight = region.Contains(pos + Directions2d.Diag[0]);
            var downLeft = region.Contains(pos + Directions2d.Diag[1]);
            var upLeft = region.Contains(pos + Directions2d.Diag[2]);
            var upRight = region.Contains(pos + Directions2d.Diag[3]);

            if (up && left && !upLeft)
            {
                sides++;
            }

            if (!up && !left)
            {
                sides++;
            }

            if (up && right && !upRight)
            {
                sides++;
            }

            if (!up && !right)
            {
                sides++;
            }

            if (down && left && !downLeft)
            {
                sides++;
            }

            if (!down && !left)
            {
                sides++;
            }

            if (down && right && !downRight)
            {
                sides++;
            }

            if (!down && !right)
            {
                sides++;
            }
        }

        return sides;
    }
}