namespace Common.Helpers;

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

    public static IEnumerable<(T first, T second)> GenerateAllPairs<T>(IList<T> values)
    {
        for (var i = 0; i < values.Count; i++)
        {
            var value = values[i];
            for (var j = i + 1; j < values.Count; j++)
            {
                yield return (value, values[j]);
            }
        }
    }
}