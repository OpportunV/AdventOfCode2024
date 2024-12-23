using System.Collections.Generic;
using System.Linq;
using Common.Helpers;

namespace AdventOfCode2024.Days;

public class Day23 : Day
{
    private readonly Dictionary<string, HashSet<string>> _graph;

    private const string Identifier = "t";

    public Day23()
    {
        _graph = GetGraph();
    }

    public override string Part1()
    {
        var seen = new HashSet<string>();
        foreach (var (pc, other) in _graph)
        {
            if (!pc.StartsWith(Identifier))
            {
                continue;
            }

            foreach (var (pc1, pc2) in Combinations.GenerateAllPairs(other.ToList()))
            {
                if (_graph[pc1].Contains(pc2))
                {
                    seen.Add(string.Join(",", new[] { pc, pc1, pc2 }.Order()));
                }
            }
        }

        return seen.Count.ToString();
    }

    public override string Part2()
    {
        var maxLength = int.MinValue;
        var best = string.Empty;
        foreach (var (pc, other) in _graph)
        {
            var network = new HashSet<string> { pc };
            foreach (var pc1 in other)
            {
                if (network.All(networkPc => _graph[pc1].Contains(networkPc)))
                {
                    network.Add(pc1);
                }
            }

            if (network.Count > maxLength)
            {
                best = string.Join(",", network.Order());
                maxLength = network.Count;
            }
        }

        return best;
    }

    private Dictionary<string, HashSet<string>> GetGraph()
    {
        var graph = new Dictionary<string, HashSet<string>>();

        foreach (var pair in GetInput())
        {
            var split = pair.Split("-");
            graph.TryAdd(split[0], []);
            graph.TryAdd(split[1], []);
            graph[split[0]].Add(split[1]);
            graph[split[1]].Add(split[0]);
        }

        return graph;
    }
}