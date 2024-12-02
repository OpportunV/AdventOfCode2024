using System;
using System.IO;

namespace AdventOfCode2024.Days;

public abstract class Day
{
    private readonly string _inputPath;

    public Day()
    {
        _inputPath = Path.Combine("Input", $"{GetType().Name}.txt");
        Console.WriteLine(_inputPath);
    }

    public abstract string Part1();

    public abstract string Part2();

    protected string[] GetInput()
    {
        return File.ReadAllLines(_inputPath);
    }

    protected string GetInputRaw()
    {
        return File.ReadAllText(_inputPath);
    }
}