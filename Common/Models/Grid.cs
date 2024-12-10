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

    public bool IsInside(GridPos2d pos)
    {
        return pos.IsInside(Rows, Cols);
    }

    public IEnumerable<GridItem<T>> Flatten()
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

    public IEnumerable<GridItem<T>> VerticalFlatten()
    {
        for (var col = 0; col < Cols; col++)
        {
            for (var row = 0; row < Rows; row++)
            {
                var pos = new GridPos2d(row, col);
                yield return new GridItem<T>(this[pos], pos);
            }
        }
    }

    public IEnumerable<GridItem<T>> AdjacentSide(GridPos2d pos)
    {
        return Adjacent(pos.AdjacentSide());
    }

    public IEnumerable<GridItem<T>> AdjacentDiag(GridPos2d pos)
    {
        return Adjacent(pos.AdjacentDiag());
    }

    public IEnumerable<GridItem<T>> AdjacentAll(GridPos2d pos)
    {
        return Adjacent(pos.AdjacentAll());
    }

    private IEnumerable<GridItem<T>> Adjacent(IEnumerable<GridPos2d> adjacents)
    {
        return adjacents.Where(IsInside)
            .Select(newPos => new GridItem<T>(this[newPos], newPos));
    }
}