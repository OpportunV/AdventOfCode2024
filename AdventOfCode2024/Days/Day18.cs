using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace AdventOfCode2024.Days;

public class Day18 : Day
{
    private readonly GridPos2d _size = new(71, 71);
    private readonly Grid<char> _grid;
    private readonly List<GridPos2d> _coords;
    private const int BytesAmount = 1024;
    private const char Wall = '#';
    private const char Empty = '.';

    public Day18()
    {
        _coords = GetInput()
            .Select(GridPos2d.Parse)
            .ToList();
        _grid = new Grid<char>(_size, Empty);
    }

    public override string Part1()
    {
        foreach (var pos in _coords.Take(BytesAmount))
        {
            _grid[pos] = Wall;
        }

        var total = GetPathLength(_grid);

        return total.ToString();
    }

    public override string Part2()
    {
        var grid = new Grid<char>(_grid);
        var left = BytesAmount + 1;
        var right = _coords.Count;
        while (left <= right)
        {
            var current = (left + right) / 2;
            for (var i = BytesAmount; i < _coords.Count; i++)
            {
                grid[_coords[i]] = i < current ? Wall : Empty;
            }

            if (GetPathLength(grid) == -1)
            {
                right = current - 1;
            }
            else
            {
                left = current + 1;
            }
        }

        return _coords[left - 1].ToString();
    }

    private int GetPathLength(Grid<char> grid)
    {
        var startPos = new GridPos2d(0, 0);
        var endPos = _size - 1;
        var toVisit = new PriorityQueue<GridPos2d, int>();
        toVisit.Enqueue(startPos, 0);
        var visited = new HashSet<GridPos2d>();
        var total = -1;
        while (toVisit.TryDequeue(out var curPos, out var steps))
        {
            if (!visited.Add(curPos))
            {
                continue;
            }

            if (curPos == endPos)
            {
                total = steps;
                break;
            }

            foreach (var (val, nextPos) in grid.AdjacentSide(curPos))
            {
                if (val != Wall)
                {
                    toVisit.Enqueue(nextPos, steps + 1);
                }
            }
        }

        return total;
    }
}