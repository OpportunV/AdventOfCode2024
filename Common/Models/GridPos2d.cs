using Common.Models.Interfaces;

namespace Common.Models;

public record GridPos2d(int Row, int Col)
{
    private static readonly List<GridPos2d> _directionsAll =
    [
        new(0, 1),
        new(1, 1),
        new(1, 0),
        new(1, -1),
        new(0, -1),
        new(-1, -1),
        new(-1, 0),
        new(-1, 1)
    ];

    private static readonly List<GridPos2d> _directionsDiag =
    [
        new(1, 1),
        new(1, -1),
        new(-1, -1),
        new(-1, 1)
    ];

    private static readonly List<GridPos2d> _directionsSide =
    [
        new(0, 1),
        new(1, 0),
        new(0, -1),
        new(-1, 0)
    ];

    public IEnumerable<IEnumerable<GridPos2d>> AdjacentAll(int rows, int cols, int length)
    {
        return Adjacent(rows, cols, length, _directionsAll);
    }

    public IEnumerable<IEnumerable<GridPos2d>> AdjacentDiag(int rows, int cols, int length)
    {
        return Adjacent(rows, cols, length, _directionsDiag);
    }

    public IEnumerable<IEnumerable<GridPos2d>> AdjacentSide(int rows, int cols, int length)
    {
        return Adjacent(rows, cols, length, _directionsSide);
    }

    public IEnumerable<GridPos2d> AdjacentSide(int rows, int cols)
    {
        return Adjacent(rows, cols, _directionsSide);
    }

    public bool IsInside(GridPos2d min, GridPos2d max)
    {
        return Row >= min.Row && Row < max.Row && Col >= min.Col && Col < max.Col;
    }

    public bool IsInside(IGrid grid)
    {
        return IsInside(grid.Rows, grid.Cols);
    }

    public bool IsInside(int rows, int cols)
    {
        return Row >= 0 && Row < rows && Col >= 0 && Col < cols;
    }

    public static GridPos2d operator *(GridPos2d pos2d, int other)
    {
        return new GridPos2d(pos2d.Row * other, pos2d.Col * other);
    }

    public static GridPos2d operator +(GridPos2d pos2d, GridPos2d other)
    {
        return new GridPos2d(pos2d.Row + other.Row, pos2d.Col + other.Col);
    }

    public static GridPos2d operator -(GridPos2d pos2d, GridPos2d other)
    {
        return new GridPos2d(pos2d.Row - other.Row, pos2d.Col - other.Col);
    }

    public static GridPos2d operator -(GridPos2d pos2d)
    {
        return new GridPos2d(-pos2d.Row, -pos2d.Col);
    }

    private IEnumerable<IEnumerable<GridPos2d>> Adjacent(int rows, int cols, int length, List<GridPos2d> directions)
    {
        foreach (var dir in directions)
        {
            var endPos = this + dir * (length - 1);

            if (0 > endPos.Row || endPos.Row >= rows || 0 > endPos.Col || endPos.Col >= cols)
            {
                continue;
            }

            yield return Enumerable.Range(0, length).Select(i => this + dir * i);
        }
    }

    private IEnumerable<GridPos2d> Adjacent(int rows, int cols, List<GridPos2d> directions)
    {
        foreach (var dir in directions)
        {
            var endPos = this + dir;

            if (0 > endPos.Row || endPos.Row >= rows || 0 > endPos.Col || endPos.Col >= cols)
            {
                continue;
            }

            yield return endPos;
        }
    }
}