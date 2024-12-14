using System.Collections.Generic;
using Common.Models;

namespace AdventOfCode2024.Models;

public class Robot
{
    public GridPos2d Pos { get; set; }

    public GridPos2d Velocity { get; set; }

    public Robot(List<int> values)
    {
        Pos = new GridPos2d(values[0], values[1]);
        Velocity = new GridPos2d(values[2], values[3]);
    }

    public void Move(int maxRow, int maxCol)
    {
        var newPos = Pos + Velocity;
        if (!newPos.IsInside(maxRow, maxCol))
        {
            newPos = new GridPos2d((newPos.Row + maxRow) % maxRow, (newPos.Col + maxCol) % maxCol);
        }

        Pos = newPos;
    }
}