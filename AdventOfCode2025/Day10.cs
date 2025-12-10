using AdventOfCode2025.Utilities;
using System.Text.RegularExpressions;

namespace AdventOfCode2025;

public class Machine(ulong indicatorLightsDiagram, int[][] buttonWiringSchematics, int[] joltageRequirements)
{
    public ulong IndicatorLightsDiagram { get; } = indicatorLightsDiagram;
    public int[][] ButtonWiringSchematics { get; } = buttonWiringSchematics;
    public int[] JoltageRequirements { get; } = joltageRequirements;
    public ulong IndicatorLights { get; private set; } = 0;

    public Machine(bool[] indicatorLightsDiagram, int[][] buttonWiringSchematics, int[] joltageRequirements)
        : this(Encode(indicatorLightsDiagram), buttonWiringSchematics, joltageRequirements) { }

    public Machine ToMachine()
    {
        Machine output = new(IndicatorLightsDiagram, ButtonWiringSchematics, JoltageRequirements)
            { IndicatorLights = IndicatorLights };

        return output;
    }

    private void ToggleLight(int light)
        => IndicatorLights ^= (1UL << light);

    public void PushButtonLights(int[] button)
    {
        foreach (int i in button)
            ToggleLight(i);
    }

    public Machine PushButtonLightsOnNewMachine(int[] button)
    {
        Machine mach = ToMachine();

        foreach (int i in button)
            mach.ToggleLight(i);

        return mach;
    }

    public bool LightsConfiguredCorrectly()
        => IndicatorLights == IndicatorLightsDiagram;

    public int CountIncorrectlyConfiguredLights()
    {
        ulong diff = IndicatorLights ^ IndicatorLightsDiagram;
        int different = System.Numerics.BitOperations.PopCount(diff);
        return different;
    }

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

    private static int Configure(Machine machine)
    {
        Queue<(Machine, int)> queue = [];
        Dictionary<ulong, int> visited = [];

        ulong correctIndicatorLights = machine.IndicatorLightsDiagram;

        queue.Enqueue((machine, 0));

        int min = int.MaxValue;
        while (queue.TryDequeue(out var data))
        {
            (Machine mach, int depth) = data;

            if (depth >= min)
                continue;

            if (mach.LightsConfiguredCorrectly())
            {
                min = depth;
                continue;
            }

            if (visited.TryGetValue(mach.IndicatorLights, out int visitedDepth) && visitedDepth >= depth)
                continue;

            visited[mach.IndicatorLights] = depth;

            foreach (var pushed in machine.ButtonWiringSchematics.Select(mach.PushButtonLightsOnNewMachine))
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
        Console.WriteLine("configuring new machine");

        PriorityQueue<(int[], int), int> queue = new();
        Dictionary<string, int> visited = [];

        queue.Enqueue((new int[machine.JoltageRequirements.Length], 0), 0);

        int min = int.MaxValue;
        while (queue.TryDequeue(out var data, out _))
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
                });

            foreach (var (pushed, diff) in buttons)
                queue.Enqueue((pushed, depth + 1), 100 * diff + depth);
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