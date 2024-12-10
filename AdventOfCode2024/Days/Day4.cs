using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace AdventOfCode2024.Days;

public class Day4 : Day
{
    private readonly Grid<char> _grid;

    private const string Xmas = "XMAS";
    private const string Mas = "MAS";

    public Day4()
    {
        var grid = GetInput().Select(line => line.ToCharArray()).ToArray();
        _grid = new Grid<char>(grid);
    }

    public override string Part1()
    {
        var counter = 0;
        foreach (var (value, pos) in _grid.LinearIterationPairs())
        {
            if (value != Xmas[0])
            {
                continue;
            }

            counter += pos.AdjacentAll(_grid.Rows, _grid.Cols, Xmas.Length)
                .Select(wordCoords => wordCoords.Select(newPos => _grid[newPos]).ToArray())
                .Select(chars => new string(chars)).Count(word => word == Xmas);
        }

        return counter.ToString();
    }

    public override string Part2()
    {
        var wordPositions = new List<(GridPos2d start, GridPos2d end)>();

        foreach (var (value, pos) in _grid.LinearIterationPairs())
        {
            if (value != Mas[0])
            {
                continue;
            }

            var wordCoords = pos.AdjacentDiag(_grid.Rows, _grid.Cols, Mas.Length)
                .Where(wordCoords =>
                    new string(wordCoords.Select(newPos => _grid[newPos]).ToArray()) == Mas)
                .Select(coords => coords.OrderBy(coord => coord.Row).ThenBy(coord => coord.Col).ToArray())
                .Select(coords => (coords[0], coords[^1])).ToHashSet();
            wordPositions.AddRange(wordCoords);
        }

        return wordPositions.Count(wordPos => wordPositions.Any(secondWordPos => IsCross(wordPos, secondWordPos)))
            .ToString();
    }

    private bool IsCross((GridPos2d start, GridPos2d end) pos1, (GridPos2d start, GridPos2d end) pos2)
    {
        var delta = Mas.Length - 1;
        // Assuming start and end are ordered by row then by col.
        return pos1.start.Row == pos2.start.Row && pos1.start.Col + delta == pos2.start.Col
            && pos1.end.Row == pos2.end.Row && pos1.end.Col - delta == pos2.end.Col;
    }
}