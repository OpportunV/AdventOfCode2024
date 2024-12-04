using System.Collections.Generic;
using System.Linq;
using AdventOfCode2024.Models;

namespace AdventOfCode2024.Days;

public class Day4 : Day
{
    private readonly char[][] _grid;
    private readonly int _rows;
    private readonly int _cols;

    private const string Xmas = "XMAS";
    private const string Mas = "MAS";

    public Day4()
    {
        var inp = GetInput();
        _grid = inp.Select(line => line.ToCharArray()).ToArray();
        _rows = _grid.Length;
        _cols = _grid[0].Length;
    }

    public override string Part1()
    {
        var counter = 0;
        for (var row = 0; row < _rows; row++)
        {
            for (var col = 0; col < _cols; col++)
            {
                if (_grid[row][col] != 'X')
                {
                    continue;
                }

                var pos = new GridPos2d(row, col);
                counter += pos.AdjacentAll(_rows, _cols, Xmas.Length)
                    .Select(wordCoords => wordCoords.Select(newPos => _grid[newPos.Row][newPos.Col]).ToArray())
                    .Select(chars => new string(chars)).Count(word => word == Xmas);
            }
        }

        return counter.ToString();
    }

    public override string Part2()
    {
        var wordPositions = new List<(GridPos2d start, GridPos2d end)>();

        for (var row = 0; row < _rows; row++)
        {
            for (var col = 0; col < _cols; col++)
            {
                if (_grid[row][col] != 'M')
                {
                    continue;
                }

                var pos = new GridPos2d(row, col);
                var wordCoords = pos.AdjacentDiag(_rows, _cols, Mas.Length)
                    .Where(wordCoords =>
                        new string(wordCoords.Select(newPos => _grid[newPos.Row][newPos.Col]).ToArray()) == Mas)
                    .Select(coords => coords.OrderBy(coord => coord.Row).ThenBy(coord => coord.Col).ToArray())
                    .Select(coords => (coords[0], coords[^1])).ToHashSet();
                wordPositions.AddRange(wordCoords);
            }
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