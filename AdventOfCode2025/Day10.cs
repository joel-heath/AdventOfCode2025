using AdventOfCode2025.Utilities;
using System;
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
        { "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}\r\n[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}\r\n[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}", "33" }
    };

    private static ulong Encode(bool[] bits)
    {
        ulong value = 0;

        for (int i = 0; i < bits.Length; i++)
        {
            if (bits[i])
                value |= 1UL << i;
        }

        return value;
    }

    private static bool[] Decode(ulong value, int length)
    {
        bool[] bits = new bool[length];

        for (int i = 0; i < length; i++)
            bits[i] = (value & (1UL << i)) != 0;

        return bits;
    }

    private static ulong Toggle(ulong indicatorLights, int light)
        => indicatorLights ^ (1UL << light);

    private static ulong PushButton(ulong indicatorLights, int[] button)
    {
        foreach (int i in button)
            indicatorLights = Toggle(indicatorLights, i);

        return indicatorLights;
    }

    private static bool LightsConfiguredCorrectly(ulong indicatorLights, ulong correct)
        => indicatorLights == correct;

    private static int CountIncorrectlyConfiguredLights(ulong indicatorLights, ulong correct)
    {
        ulong diff = indicatorLights ^ correct;
        int different = System.Numerics.BitOperations.PopCount(diff);
        return different;
    }

    private static int SafeIncrement(int value)
        => value == int.MaxValue ? int.MaxValue : value + 1;

    private static int Configure(Machine machine)
    {
        Queue<(ulong, int)> queue = [];
        Dictionary<ulong, int> visited = [];

        ulong correctIndicatorLights = Encode(machine.IndicatorLightsDiagram);

        queue.Enqueue((Encode(machine.IndicatorLights), 0));

        int min = int.MaxValue;
        while (queue.TryDequeue(out var data))
        {
            (ulong indicatorLights, int depth) = data;

            if (depth >= min)
                continue;

            if (LightsConfiguredCorrectly(indicatorLights, correctIndicatorLights))
            {
                min = depth;
                continue;
            }

            if (visited.TryGetValue(indicatorLights, out int visitedDepth) && visitedDepth >= depth)
                continue;

            visited[indicatorLights] = depth;

            var buttons = machine.ButtonWiringSchematics
                .Select(button =>
                {
                    ulong pushed = PushButton(indicatorLights, button);
                    int count = CountIncorrectlyConfiguredLights(pushed, correctIndicatorLights);
                    return (pushed, count);
                })
                .OrderBy(button => button.count);

            foreach (var (pushed, count) in buttons)
                queue.Enqueue((pushed, depth + 1));
        }

        return min;
    }

    private static bool JoltagesConfiguredCorrectly(int[] joltages, int[] reference)
        => joltages.SequenceEqual(reference);

    private static bool AnyJoltageTooLarge(int[] joltages, int[] reference)
        => joltages.Zip(reference).Any(t => t.First > t.Second);

    private static int[] PushButton(int[] joltages, int[] button)
    {
        int[] output = new int[joltages.Length];
        Array.Copy(joltages, output, joltages.Length);

        //for (int i = 0; i < button.Length; i++)
        //    output[i] += button[i];

        foreach (var index in button)
            output[index]++;

        return output;
    }

    private static int CalculateJoltageDifference(int[] joltages, int[] reference)
        => joltages.Zip(reference).Sum(t => t.Second - t.First);

    private static int Configure2(Machine machine)
    {
        Queue<(int[], int)> queue = [];
        Dictionary<string, int> visited = [];

        queue.Enqueue((new int[machine.JoltageRequirements.Length], 0));

        int min = int.MaxValue;
        while (queue.TryDequeue(out var data))
        {
            (int[] joltages, int depth) = data;

            if (depth >= min || AnyJoltageTooLarge(joltages, machine.JoltageRequirements))
                continue;

            if (JoltagesConfiguredCorrectly(joltages, machine.JoltageRequirements))
            {
                min = depth;
                continue;
            }

            string encoded = string.Join(',', joltages);

            if (visited.TryGetValue(encoded, out int visitedDepth) && visitedDepth >= depth)
                continue;

            visited[encoded] = depth;

            var buttons = machine.ButtonWiringSchematics
                .Select(button =>
                {
                    int[] pushed = PushButton(joltages, button);
                    int diff = CalculateJoltageDifference(pushed, machine.JoltageRequirements);
                    return (pushed, diff);
                })
                .OrderBy(button => button.diff);

            foreach (var (pushed, _) in buttons)
                queue.Enqueue((pushed, depth + 1));
        }

        return min;
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

        return $"{machines.Sum(Configure)}";
    }

    public string SolvePart2(string input)
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

        return $"{machines.Sum(Configure2)}";
    }
}