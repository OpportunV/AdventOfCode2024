using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024.Helpers;

public static class Combinations
{
    public static IEnumerable<string> GeneratePermutations(IReadOnlyList<char> symbols, int length)
    {
        var combinations = symbols.Select(c => c.ToString());

        for (var i = 1; i < length; i++)
        {
            combinations = combinations.SelectMany(
                _ => symbols,
                (comb, symbol) => comb + symbol
            );
        }

        return combinations;
    }
}