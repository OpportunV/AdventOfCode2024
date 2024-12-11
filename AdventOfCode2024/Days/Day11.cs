using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024.Days;

public class Day11 : Day
{
    private const long Multiplier = 2024;

    public override string Part1()
    {
        var stones = GetStones();
        for (var i = 0; i < 25; i++)
        {
            stones = SimulateBlink(stones);
        }

        return stones.Values.Sum().ToString();
    }

    public override string Part2()
    {
        var stones = GetStones();
        for (var i = 0; i < 75; i++)
        {
            stones = SimulateBlink(stones);
        }

        return stones.Values.Sum().ToString();
    }

    private static Dictionary<long, long> SimulateBlink(Dictionary<long, long> stones)
    {
        var newStones = new Dictionary<long, long>();
        foreach (var (stone, amount) in stones)
        {
            if (stone == 0)
            {
                Modify(newStones, 1, amount);
                continue;
            }

            var valueStr = stone.ToString();
            var len = valueStr.Length;
            if (len % 2 == 0)
            {
                var left = long.Parse(valueStr[..(len / 2)]);
                var right = long.Parse(valueStr[(len / 2)..]);
                Modify(newStones, left, amount);
                Modify(newStones, right, amount);
                continue;
            }

            Modify(newStones, stone * Multiplier, amount);
        }

        stones = newStones;

        return stones;
    }

    private static void Modify(Dictionary<long, long> stones, long key, long amount)
    {
        if (!stones.TryAdd(key, amount))
        {
            stones[key] += amount;
        }
    }

    private Dictionary<long, long> GetStones()
    {
        return GetInputRaw().Split(" ").Select(long.Parse)
            .ToDictionary(stone => stone, _ => 1L);
    }
}