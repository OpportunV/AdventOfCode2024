using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024.Days;

public class Day24 : Day
{
    private const string And = "AND";
    private const string Or = "OR";
    private const string Xor = "XOR";
    private const string Y = "y";
    private const string X = "x";
    private const string Z = "z";
    private const string Zeros = "00";
    private readonly Dictionary<string, byte> _evaluated;
    private readonly Dictionary<string, (string left, string op, string right)> _complex;

    public Day24()
    {
        (_evaluated, _complex) = ParseInput();
    }

    public override string Part1()
    {
        foreach (var (key, data) in _complex)
        {
            if (key.StartsWith(Z))
            {
                Evaluate(key, data);
            }
        }

        return Convert.ToInt64(GetBits(Z), 2).ToString();
    }

    public override string Part2()
    {
        var z = GetBits(Z);
        var badKeys = new HashSet<string>();
        var inputs = _evaluated.Keys.Where(key => key.StartsWith(X) || key.StartsWith(Y)).ToHashSet();

        foreach (var (key, data) in _complex)
        {
            var isOutputKey = key.StartsWith(Z);
            var isLeftInput = inputs.Contains(data.left);
            var isRightInput = inputs.Contains(data.right);

            if (isOutputKey && key != $"{Z}{z.Length - 1}" && data.op != Xor)
            {
                badKeys.Add(key);
                continue;
            }

            if (!key.StartsWith(Z) && !isLeftInput && !isRightInput && data.op == Xor)
            {
                badKeys.Add(key);
                continue;
            }

            if (isLeftInput && isRightInput && data.left[1..] != Zeros && data.right[1..] != Zeros)
            {
                var expected = data.op == Xor ? Xor : Or;
                var correct = _complex
                    .Where(pair => pair.Key != key && pair.Value.op == expected)
                    .Any(pair => pair.Value.left == key || pair.Value.right == key);
                if (!correct)
                {
                    badKeys.Add(key);
                }
            }
        }

        return string.Join(",", badKeys.Order());
    }

    private string GetBits(string identifier)
    {
        return string.Join("", _evaluated
            .Where(pair => pair.Key.StartsWith(identifier))
            .OrderByDescending(pair => pair.Key)
            .Select(pair => pair.Value));
    }

    private void Evaluate(string key, (string left, string op, string right) data)
    {
        if (!_evaluated.ContainsKey(data.left))
        {
            Evaluate(data.left, _complex[data.left]);
        }

        if (!_evaluated.ContainsKey(data.right))
        {
            Evaluate(data.right, _complex[data.right]);
        }

        _evaluated[key] = data.op switch
        {
            And => (byte)(_evaluated[data.left] & _evaluated[data.right]),
            Or => (byte)(_evaluated[data.left] | _evaluated[data.right]),
            Xor => (byte)(_evaluated[data.left] ^ _evaluated[data.right]),
            _ => throw new InvalidOperationException()
        };
    }

    private (Dictionary<string, byte> evaluated, Dictionary<string, (string left, string op, string right)> pending)
        ParseInput()
    {
        var input = GetInputRaw().Split("\n\n");
        var evaluated = input[0]
            .Split("\n")
            .Select(line => line.Split(": "))
            .ToDictionary(pair => pair[0], pair => byte.Parse(pair[1]));

        var pending = input[1]
            .Split("\n")
            .Select(line => line.Split(" -> "))
            .ToDictionary(pair => pair[1], pair =>
            {
                var split = pair[0].Split(" ");
                return (split[0], split[1], split[2]);
            });

        return (evaluated, pending);
    }
}