using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode2024.Days;

namespace AdventOfCode2024;

public class Solver
{
    private static readonly Stopwatch _stopwatch = new();

    public void PrintAllAnswers()
    {
        var classes = GetClasses();

        foreach (var cls in classes)
        {
            if (Activator.CreateInstance(cls) is not Day instance)
            {
                continue;
            }

            PrintParts(instance);
        }
    }

    public void PrintDay(int day)
    {
        var classes = GetClasses();
        var targetDay = classes.FirstOrDefault(cls => cls.Name == $"{nameof(Day)}{day}");
        if (targetDay != null && Activator.CreateInstance(targetDay) is Day instance)
        {
            PrintParts(instance);
        }
    }

    private static IEnumerable<Type> GetClasses()
    {
        var baseClassType = typeof(Day);
        var enumerable = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type is
                {
                    IsClass: true,
                    IsAbstract: false
                }
                && baseClassType.IsAssignableFrom(type));
        return enumerable;
    }

    private static void PrintParts(Day day)
    {
        Console.WriteLine(day.GetType().Name);
        _stopwatch.Restart();
        Console.WriteLine($"Part 1:\t {day.Part1()}");
        Console.WriteLine($"Took {_stopwatch.Elapsed.TotalMilliseconds}ms\n");
        _stopwatch.Restart();
        Console.WriteLine($"Part 2:\t {day.Part2()}");
        Console.WriteLine($"Took {_stopwatch.Elapsed.TotalMilliseconds}ms");
    }
}