using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024.Helpers;

public class CombinationsGenerator
{
    public static IEnumerable<string> Generate(IReadOnlyList<char> symbols, int length)
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