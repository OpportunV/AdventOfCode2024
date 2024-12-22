using System.Numerics;

namespace Common.Extensions;

public static class NumberExt
{
    public static T Mod<T>(this T number, T mod) where T : INumber<T>
    {
        return (number % mod + mod) % mod;
    }
}