using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AdventOfCode2024.Days;

public class Day9 : Day
{
    private readonly List<int> _diskMap;
    private const int FreeSpace = -1;

    public Day9()
    {
        _diskMap = GetInput()[0].Select(chr => chr - '0').ToList();
    }

    public override string Part1()
    {
        var unpacked = Unpack(_diskMap);
        var compressed = Compress(unpacked);
        var checksum = GetChecksum(compressed);

        return checksum.ToString();
    }

    public override string Part2()
    {
        var files = new List<Range>();
        var spacesStart = new Dictionary<Index, Range>();
        var spacesEnd = new Dictionary<Index, Range>();
        var isFile = true;
        var globalIndex = 0;
        foreach (var value in _diskMap)
        {
            if (isFile)
            {
                files.Add(new Range(globalIndex, globalIndex + value));
            }
            else
            {
                AddSpace(new Range(globalIndex, globalIndex + value), spacesStart, spacesEnd);
            }

            isFile = !isFile;
            globalIndex += value;
        }

        var checksum = 0L;
        for (var i = files.Count - 1; i >= 0; i--)
        {
            var file = files[i];
            var fileLength = file.Length();
            try
            {
                var space = spacesStart.Values.First(value =>
                    value.Length() >= fileLength && value.Start.Value <= file.Start.Value);

                AddSpace(new Range(file.Start, file.End), spacesStart, spacesEnd);
                files[i] = new Range(space.Start, space.Start.Value + fileLength);

                spacesStart.Remove(space.Start);
                spacesStart.Remove(space.End);

                var extraSpace = space.Length() - fileLength;
                if (extraSpace > 0)
                {
                    var start = space.Start.Value + fileLength;
                    AddSpace(new Range(start, start + extraSpace), spacesStart, spacesEnd);
                }
            }
            catch (InvalidOperationException)
            {
                // Ignored.
            }
            finally
            {
                checksum += Enumerable.Range(files[i].Start.Value, files[i].Length())
                    .Select(value => (long)value * i)
                    .Sum();
            }
        }

        return checksum.ToString();
    }

    private static void AddSpace(Range space, Dictionary<Index, Range> spacesStart, Dictionary<Index, Range> spacesEnd)
    {
        if (spacesStart.Remove(space.End, out var following))
        {
            space = new Range(space.Start, following.End);
        }

        if (spacesEnd.Remove(space.Start, out var preceding))
        {
            space = new Range(preceding.Start, space.End);
        }

        spacesStart[space.Start] = space;
        spacesEnd[space.End] = space;
    }

    private static List<int> Unpack(List<int> diskMap)
    {
        var unpacked = new List<int>();
        var isFile = true;
        var fileIndex = 0;
        foreach (var value in diskMap)
        {
            if (isFile)
            {
                unpacked.AddRange(Enumerable.Repeat(fileIndex, value));
                fileIndex++;
            }
            else
            {
                unpacked.AddRange(Enumerable.Repeat(FreeSpace, value));
            }

            isFile = !isFile;
        }

        return unpacked;
    }

    private static List<int> Compress(List<int> unpacked)
    {
        var left = 0;
        var right = unpacked.Count - 1;

        var compressed = new List<int>(unpacked);
        while (left < right)
        {
            if (compressed[left] != FreeSpace)
            {
                left++;
                continue;
            }

            if (compressed[right] == FreeSpace)
            {
                right--;
                continue;
            }

            compressed[left] = compressed[right];
            compressed[right] = FreeSpace;

            left++;
            right--;
        }

        return compressed;
    }

    private static long GetChecksum(List<int> unpacked)
    {
        return unpacked.Where(value => value != FreeSpace)
            .Select((value, index) => (long)value * index)
            .Sum();
    }
}