using System;

namespace AdventOfCode2024.Extensions;

public static class RangeExt
{
    public static int Length(this Range range)
    {
        return range.End.Value - range.Start.Value;
    }
}