using AdventOfCode2025.Utilities;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;

namespace AdventOfCode2025;

public class Machine
{
    public bool[] IndicatorLightsDiagram { get; }
    public int[][] ButtonWiringSchematics { get; }
    public int[] JoltageRequirements { get; }
    public bool[] IndicatorLights { get; private set; }

    public Machine(bool[] indicatorLightsDiagram, int[][] buttonWiringSchematics, int[] joltageRequirements)
    {
        IndicatorLightsDiagram = indicatorLightsDiagram;
        ButtonWiringSchematics = buttonWiringSchematics;
        JoltageRequirements = joltageRequirements;
        IndicatorLights = new bool[indicatorLightsDiagram.Length];
    }

    private void Toggle(int light)
        => IndicatorLights[light] = !IndicatorLights[light];

    public void PushButton(int[] button)
    {
        foreach (int i in button)
            Toggle(i);
    }

    public bool LightsConfiguredCorrectly()
        => Enumerable.SequenceEqual(IndicatorLights, IndicatorLightsDiagram);

    public int CountCorrectlyConfiguredLights()
        => IndicatorLights.Zip(IndicatorLightsDiagram)
                          .Count(x => x.First == x.Second);
}

public class Day10 : IDay
{
    public int Day => 10;
    public Dictionary<string, string> UnitTestsP1 { get; } = new() 
    {
        { "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}\r\n[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}\r\n[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}", "7" }
    };
    public Dictionary<string, string> UnitTestsP2 { get; } = new()
    {
        { "TestInput1", "ExpectedOutput1" }
    };

    private static int SafeIncrement(int value)
        => value == int.MaxValue ? int.MaxValue : value + 1;

    private static Dictionary<string, int> memo = [];

    private static int Configure(Machine machine, int depth)
    {
        if (depth == 0)
            return int.MaxValue;
        if (machine.LightsConfiguredCorrectly())
            return 0;

        string memoKey = string.Concat(machine.IndicatorLights.Select(c => c ? '#' : '.'));
        if (memo.TryGetValue(memoKey, out var memoValue))
            return memoValue;

        var buttons = machine.ButtonWiringSchematics
            .OrderByDescending(button =>
            {
                machine.PushButton(button);
                int count = machine.CountCorrectlyConfiguredLights();
                machine.PushButton(button);
                return count;
            });
        
        int min = int.MaxValue;
        foreach (var button in buttons)
        {
            machine.PushButton(button);
            int value = SafeIncrement(Configure(machine, depth - 1));
            machine.PushButton(button);
            if (value < min)
                min = value;
        }

        if (min != int.MaxValue)
            memo[memoKey] = min;

        return min;
    }

    private static int IDS(Machine machine)
    {
        memo = []; // memo is machine-dependent
        int depth = 0;
        int value = int.MaxValue;
        while (value == int.MaxValue)
        {
            depth++;
            value = Configure(machine, depth);
        }

        Console.WriteLine("Solved! at depth " + depth);

        return value;
    }

    public string SolvePart1(string input)
    {
        Machine[] machines = [..input.Lines().Select(line =>
        {
            bool[] indicators = [..Regex.Match(line, @"\[([.#]+)\]")
                .Groups.Values.ElementAt(1).Value
                .Select(l => l == '#')];

            int[][] schematics = [..Regex.Matches(line, @"\((\d+(,\d+)*)\)")
                .Select(match => match.Groups.Values.ElementAt(1).Value)
                .Select(thing => thing.Split(',').Select(int.Parse).ToArray())];

            int[] requirements = [..Regex.Match(line, @"\{(\d+(,\d+)*)\}")
                .Groups.Values.ElementAt(1).Value
                .Split(',').Select(int.Parse)];

            return new Machine(indicators, schematics, requirements);
        })];

        return $"{machines.Sum(IDS)}";
    }

    public string SolvePart2(string input)
    {
        return $"{string.Empty}";
    }
}