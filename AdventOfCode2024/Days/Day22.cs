using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AdventOfCode2024.Days;

public class Day22 : Day
{
    private readonly List<long> _numbers;
    private readonly Dictionary<string, long> _counter = new();

    private const int Amount = 2000;
    private const int Mod = 16777216;
    private const long Multiplier1 = 64;
    private const long Multiplier2 = 2048;
    private const long Divider = 32;
    private const int SequenceLength = 4;

    public Day22()
    {
        _numbers = GetInput().Select(long.Parse).ToList();
    }

    public override string Part1()
    {
        return _numbers.Select(GenerateNumbers).Sum().ToString();
    }

    public override string Part2()
    {
        foreach (var number in _numbers)
        {
            var changes = new Queue<long>();
            var seen = new Dictionary<string, long>();
            var cur = number;
            var price = cur.Mod(10);
            for (var j = 0; j < Amount + 1; j++)
            {
                var next = GenerateNewNumber(cur);
                var nextPrice = next.Mod(10);
                if (changes.Count == SequenceLength)
                {
                    var sequence = string.Join(",", changes);
                    seen.TryAdd(sequence, price);
                    changes.Dequeue();
                }

                var diff = nextPrice - price;
                changes.Enqueue(diff);
                cur = next;
                price = nextPrice;
            }

            foreach (var (sequence, bananas) in seen)
            {
                _counter.TryAdd(sequence, 0);
                _counter[sequence] += bananas;
            }
        }

        return _counter.Values.Max().ToString();
    }

    private static long GenerateNumbers(long number)
    {
        for (var i = 0; i < Amount; i++)
        {
            number = GenerateNewNumber(number);
        }

        return number;
    }

    private static long GenerateNewNumber(long number)
    {
        number = MixPrune(number, number * Multiplier1);
        number = MixPrune(number, number / Divider);
        number = MixPrune(number, number * Multiplier2);
        return number;
    }

    private static long MixPrune(long number, long other)
    {
        number ^= other;
        number = number.Mod(Mod);
        return number;
    }
}