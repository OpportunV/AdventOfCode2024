using System.Text;
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

    public Grid(Grid<T> grid)
    {
        Rows = grid.Rows;
        Cols = grid.Cols;
        _grid = new T[Rows][];
        for (var i = 0; i < Rows; i++)
        {
            _grid[i] = [..grid._grid[i]];
        }
    }

    public Grid(T[][] grid)
    {
        _grid = grid;
        Rows = grid.Length;
        Cols = grid[0].Length;
    }

    public Grid(GridPos2d size, T @default) : this(size.Row, size.Col, @default)
    {
    }

    public Grid(int rows, int cols, T @default)
    {
        _grid = new T[rows][];
        for (var i = 0; i < rows; i++)
        {
            _grid[i] = Enumerable.Repeat(@default, cols).ToArray();
        }

        Rows = rows;
        Cols = cols;
    }

    public bool Contains(GridPos2d pos)
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

    public IEnumerable<GridItem<T>> AdjacentSide(GridItem<T> item)
    {
        return AdjacentSide(item.Pos);
    }

    public IEnumerable<GridItem<T>> AdjacentDiag(GridItem<T> item)
    {
        return AdjacentDiag(item.Pos);
    }

    public IEnumerable<GridItem<T>> AdjacentAll(GridItem<T> item)
    {
        return AdjacentAll(item.Pos);
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

    public string ToString(Func<T, object>? itemFormatter)
    {
        var res = new StringBuilder();
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Cols; j++)
            {
                res.Append(itemFormatter?.Invoke(this[i, j]) ?? this[i, j]);
            }

            res.AppendLine();
        }

        return res.ToString();
    }

    public override string ToString()
    {
        return ToString(null);
    }

    private IEnumerable<GridItem<T>> Adjacent(IEnumerable<GridPos2d> adjacents)
    {
        return adjacents.Where(Contains)
            .Select(newPos => new GridItem<T>(this[newPos], newPos));
    }
}