using Common.Models.Interfaces;

namespace Common.Models;

public class Grid<T> : IGrid
{
    public int Rows { get; }

    public int Cols { get; }

    public T this[int row, int col]
    {
        get => _grid[row][col];
        set => _grid[row][col] = value;
    }

    public T this[GridPos2d pos]
    {
        get => _grid[pos.Row][pos.Col];
        set => _grid[pos.Row][pos.Col] = value;
    }

    private readonly T[][] _grid;

    public Grid(T[][] grid)
    {
        _grid = grid;
        Rows = grid.Length;
        Cols = grid[0].Length;
    }

    public IEnumerable<GridItem<T>> LinearIterationPairs()
    {
        for (var row = 0; row < Rows; row++)
        {
            for (var col = 0; col < Cols; col++)
            {
                var pos = new GridPos2d(row, col);
                yield return new GridItem<T>(this[pos], pos);
            }
        }
    }

    public IEnumerable<GridItem<T>> AdjacentSidePairs(GridPos2d pos)
    {
        return pos.AdjacentSide(Rows, Cols).Select(newPos => new GridItem<T>(this[newPos], newPos));
    }
}