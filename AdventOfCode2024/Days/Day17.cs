using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

public class Day17 : Day
{
    private readonly List<long> _registers;
    private readonly List<int> _program;

    public Day17()
    {
        var input = Regex.Matches(GetInputRaw(), @"\d+")
            .Select(match => match.Value)
            .ToList();

        _registers = input
            .Take(3)
            .Select(long.Parse)
            .ToList();

        _program = input
            .Skip(3)
            .Select(int.Parse)
            .ToList();
    }

    public override string Part1()
    {
        var output = Run(_program, [.._registers]);
        return string.Join(",", output);
    }

    public override string Part2()
    {
        var regs = new HashSet<long> { 0L };
        var ind = _program.Count - 1;
        while (ind >= 0)
        {
            var newRegs = new HashSet<long>();
            foreach (var reg in regs)
            {
                for (var i = 0; i < 8; i++)
                {
                    var cur = 8 * reg + i;
                    List<long> registers = [.._registers];
                    registers[0] = cur;
                    var output = Run(_program, registers);
                    if (output[0] == _program[ind])
                    {
                        newRegs.Add(cur);
                    }
                }
            }

            regs = newRegs;
            ind--;
        }

        return regs.Min().ToString();
    }

    private static List<byte> Run(List<int> program, List<long> registers)
    {
        var cur = 0;
        var output = new List<byte>();
        while (cur < program.Count)
        {
            var operand = program[cur + 1];
            switch (program[cur])
            {
                case 0:
                    registers[0] = (int)(registers[0] / (long)Math.Pow(2, Combo(operand, registers)));
                    break;
                case 1:
                    registers[1] ^= operand;
                    break;
                case 2:
                    registers[1] = PosMod(Combo(operand, registers), 8);
                    break;
                case 3:
                    if (registers[0] == 0)
                    {
                        break;
                    }

                    cur = operand;
                    continue;
                case 4:
                    registers[1] ^= registers[2];
                    break;
                case 5:
                    output.Add((byte)PosMod(Combo(operand, registers), 8));
                    break;
                case 6:
                    registers[1] = (int)(registers[0] / (long)Math.Pow(2, Combo(operand, registers)));
                    break;
                case 7:
                    registers[2] = (int)(registers[0] / (long)Math.Pow(2, Combo(operand, registers)));
                    break;
            }

            cur += 2;
        }

        return output;
    }

    private static long Combo(int operand, List<long> registers)
    {
        return operand switch
        {
            <= 3 => operand,
            >= 7 => throw new IndexOutOfRangeException(operand.ToString()),
            _ => registers[operand - 4]
        };
    }

    private static long PosMod(long value, int mod)
    {
        return (value % mod + mod) % mod;
    }
}