using System;
using System.Collections.Generic;
using System.Linq;
using Common.Helpers;
using Common.Models;

namespace AdventOfCode2024.Days;

public class Day15 : Day
{
    private readonly Dictionary<char, GridPos2d> _directions;

    private const char Robot = '@';
    private const char Box = 'O';
    private const char Wall = '#';
    private const char Empty = '.';
    private const char Right = '>';
    private const char Left = '<';
    private const char BoxLeft = '[';
    private const char BoxRight = ']';

    public Day15()
    {
        var directionSymbols = new[] { Right, 'v', Left, '^' };
        _directions = directionSymbols.Zip(Directions2d.Side)
            .ToDictionary(pair => pair.First, pair => pair.Second);
    }

    public override string Part1()
    {
        var (grid, moves) = ParseInput();
        var curPos = GetStartPos(grid);
        grid[curPos] = Empty;

        foreach (var dir in moves.Select(move => _directions[move]))
        {
            var newPos = curPos + dir;
            switch (grid[newPos])
            {
                case Wall:
                    continue;
                case Empty:
                    curPos = newPos;
                    continue;
            }

            var boxPos = newPos;
            while (grid[boxPos] == Box)
            {
                boxPos += dir;
            }

            if (grid[boxPos] == Wall)
            {
                continue;
            }

            grid[newPos] = Empty;
            grid[boxPos] = Box;
            curPos = newPos;
        }

        return grid
            .Flatten()
            .Sum(item => item.Value == Box ? 100 * item.Pos.Row + item.Pos.Col : 0)
            .ToString();
    }

    public override string Part2()
    {
        var (grid, moves) = ParseInputExtended();
        var curPos = GetStartPos(grid);
        grid[curPos] = Empty;

        foreach (var dir in moves.Select(move => _directions[move]))
        {
            var newPos = curPos + dir;
            switch (grid[newPos])
            {
                case Wall:
                    continue;
                case Empty:
                    curPos = newPos;
                    continue;
            }

            var boxPos = newPos;

            // Horizontal
            if (dir.Row == 0)
            {
                while (grid[boxPos] == grid[newPos])
                {
                    boxPos += dir * 2;
                }

                if (grid[boxPos] == Empty)
                {
                    curPos = newPos;
                    for (var i = Math.Abs((boxPos - newPos).Col); i >= 0; i--)
                    {
                        grid[newPos + dir * i] = grid[newPos + dir * (i - 1)];
                    }
                }

                continue;
            }

            // Vertical
            var pushActions = new Dictionary<GridPos2d, Action>();
            var left = grid[boxPos] == BoxLeft ? boxPos : boxPos + _directions[Left];
            if (TryPushBigBox(grid, left, left + _directions[Right], dir, pushActions))
            {
                foreach (var action in pushActions.Values)
                {
                    action.Invoke();
                }

                curPos = newPos;
            }
        }

        return grid
            .Flatten()
            .Sum(item => item.Value == BoxLeft ? 100 * item.Pos.Row + item.Pos.Col : 0)
            .ToString();
    }

    private bool TryPushBigBox(Grid<char> grid, GridPos2d left, GridPos2d right, GridPos2d dir,
        Dictionary<GridPos2d, Action> pushActions)
    {
        void Push()
        {
            grid[left + dir] = BoxLeft;
            grid[right + dir] = BoxRight;
            grid[left] = Empty;
            grid[right] = Empty;
        }

        if (grid[left + dir] == Wall || grid[right + dir] == Wall)
        {
            return false;
        }

        if (grid[left + dir] == Empty && grid[right + dir] == Empty)
        {
            pushActions[left] = Push;

            return true;
        }

        if (grid[left + dir] == grid[left])
        {
            if (TryPushBigBox(grid, left + dir, right + dir, dir, pushActions))
            {
                pushActions[left] = Push;
                return true;
            }
        }

        if (grid[left + dir] == grid[right] && grid[right + dir] == Empty)
        {
            if (TryPushBigBox(grid, left + _directions[Left] + dir, left + dir, dir, pushActions))
            {
                pushActions[left] = Push;
                return true;
            }
        }

        if (grid[left + dir] == Empty && grid[right + dir] == grid[left])
        {
            if (TryPushBigBox(grid, right + dir, right + _directions[Right] + dir, dir, pushActions))
            {
                pushActions[left] = Push;
                return true;
            }
        }

        if (grid[left + dir] == grid[right] && grid[right + dir] == grid[left])
        {
            if (TryPushBigBox(grid, left + _directions[Left] + dir, left + dir, dir, pushActions)
                && TryPushBigBox(grid, right + dir, right + _directions[Right] + dir, dir, pushActions))
            {
                pushActions[left] = Push;
                return true;
            }
        }

        return false;
    }

    private static GridPos2d GetStartPos(Grid<char> grid)
    {
        return grid.Flatten().FirstOrDefault(item => item.Value == Robot)?.Pos
            ?? throw new ArgumentNullException(nameof(grid), "Cannot find the robot position.");
    }

    private (Grid<char> grid, string _moves) ParseInput()
    {
        var split = GetInputRaw().Split("\n\n");

        var gridData = split[0].Split("\n").Select(x => x.ToCharArray()).ToArray();
        var grid = new Grid<char>(gridData);

        return (grid, split[1].ReplaceLineEndings(""));
    }

    private (Grid<char> grid, string _moves) ParseInputExtended()
    {
        var split = GetInputRaw().Split("\n\n");

        var gridData = split[0].Split("\n")
            .Select(x => x
                .Replace(Wall.ToString(), $"{Wall}{Wall}")
                .Replace(Box.ToString(), $"{BoxLeft}{BoxRight}")
                .Replace(Empty.ToString(), $"{Empty}{Empty}")
                .Replace(Robot.ToString(), $"{Robot}{Empty}")
                .ToCharArray())
            .ToArray();
        var grid = new Grid<char>(gridData);

        return (grid, split[1].ReplaceLineEndings(""));
    }
}