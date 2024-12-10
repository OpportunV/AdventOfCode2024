namespace AdventOfCode2024.Models;

public record GridItem<T>(T value, GridPos2d pos)
{
    public T Value { get; } = value;

    public GridPos2d Pos { get; } = pos;
}