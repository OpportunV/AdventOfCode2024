using System;
using System.Collections.Generic;
using Common.Helpers;
using Common.Models;

namespace AdventOfCode2024.Days;

public class Day8 : Day
{
    private readonly int _rows;
    private readonly int _cols;
    private readonly Dictionary<char, List<GridPos2d>> _stations = new();

    public Day8()
    {
        var inp = GetInput();
        _rows = inp.Length;
        _cols = inp[0].Length;

        for (var i = 0; i < _rows; i++)
        {
            var line = inp[i];
            for (var j = 0; j < _cols; j++)
            {
                var cur = line[j];
                if (cur == '.')
                {
                    continue;
                }

                if (!_stations.ContainsKey(cur))
                {
                    _stations[cur] = [];
                }

                _stations[cur].Add(new GridPos2d(i, j));
            }
        }
    }

    public override string Part1()
    {
        return CalculateAllAntiNodes(AddAntiNodes).ToString();
    }

    public override string Part2()
    {
        return CalculateAllAntiNodes(AddAntiNodesResonance).ToString();
    }

    private int CalculateAllAntiNodes(Action<GridPos2d, GridPos2d, HashSet<GridPos2d>> antiNodesSelector)
    {
        var antiNodes = new HashSet<GridPos2d>();
        foreach (var stations in _stations.Values)
        {
            antiNodes.UnionWith(GetAntiNodes(stations, antiNodesSelector));
        }

        return antiNodes.Count;
    }

    private HashSet<GridPos2d> GetAntiNodes(List<GridPos2d> stations,
        Action<GridPos2d, GridPos2d, HashSet<GridPos2d>> antiNodesSelector)
    {
        var antiNodes = new HashSet<GridPos2d>();
        foreach (var (first, second) in Combinations.GenerateAllPairs(stations))
        {
            var distance = first - second;
            antiNodesSelector.Invoke(first, distance, antiNodes);
            antiNodesSelector.Invoke(second, -distance, antiNodes);
        }

        return antiNodes;
    }

    private void AddAntiNodesResonance(GridPos2d station, GridPos2d distance, HashSet<GridPos2d> antiNodes)
    {
        while (station.IsInside(_rows, _cols))
        {
            antiNodes.Add(station);
            station += distance;
        }
    }

    private void AddAntiNodes(GridPos2d station, GridPos2d distance, HashSet<GridPos2d> antiNodes)
    {
        var antiNode = station + distance;
        if (antiNode.IsInside(_rows, _cols))
        {
            antiNodes.Add(antiNode);
        }
    }
}