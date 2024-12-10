using Common.Models;

namespace Common.Helpers;

public static class Directions2d
{
    public static readonly IReadOnlyList<GridPos2d> All =
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

    public static readonly IReadOnlyList<GridPos2d> Diag =
    [
        new(1, 1),
        new(1, -1),
        new(-1, -1),
        new(-1, 1)
    ];

    public static readonly IReadOnlyList<GridPos2d> Side =
    [
        new(0, 1),
        new(1, 0),
        new(0, -1),
        new(-1, 0)
    ];
}