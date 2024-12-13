using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

public class Day13 : Day
{
    private readonly double[] _coasts = [3, 1];
    private const double Part2Addition = 10000000000000;

    public override string Part1()
    {
        return GetMachines(0).Sum(GetMachineCost).ToString(CultureInfo.InvariantCulture);
    }

    public override string Part2()
    {
        return GetMachines(Part2Addition).Sum(GetMachineCost).ToString(CultureInfo.InvariantCulture);
    }

    private double GetMachineCost((double, double, double, double, double, double) machine)
    {
        var (ax, ay, bx, by, tx, ty) = machine;

        var bN = (tx * ay - ty * ax) / (bx * ay - by * ax);
        var aN = (tx - bN * bx) / ax;

        var cost = aN * _coasts[0] + bN * _coasts[1];

        return cost % 1 == 0 ? cost : 0;
    }

    private (double, double, double, double, double, double)[] GetMachines(double addition)
    {
        return GetInputRaw()
            .Split("\n\n")
            .Select(machine => Regex.Matches(machine, @"\d+")
                .Select(match => double.Parse(match.ToString())).ToArray())
            .Select(machine => (
                machine[0], machine[1],
                machine[2], machine[3],
                machine[4] + addition, machine[5] + addition))
            .ToArray();
    }
}