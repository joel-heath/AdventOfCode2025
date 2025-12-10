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

        // Arrange ILP

        // Each variable will represent the number of times we push the corresponding button
        // Objective: Minimize the sum of all variables (total button presses)

        // Constraints:
        // The counters must equal the required configuration
        // So we have n constraints given n counters (and n requirements)


        return $"{machines.Sum(machine =>
        {
            double[] objectiveFunction = new double[machine.ButtonWiringSchematics.Length];
            for (int i = 0; i < objectiveFunction.Length; i++)
                objectiveFunction[i] = 1;

            double[][] constraints = new double[machine.JoltageRequirements.Length][];
            for (int i = 0; i < constraints.Length; i++)
            {
                constraints[i] = new double[machine.ButtonWiringSchematics.Length + 1];
                for (int j = 0; j < machine.ButtonWiringSchematics.Length; j++)
                    constraints[i][j] = machine.ButtonWiringSchematics[j].Contains(i) ? 1 : 0;

                constraints[i][^1] = machine.JoltageRequirements[i];
            }

            (int minimum, int[] assignments) = IntegerSimplex.Minimise(objectiveFunction, [], constraints, []);

            return minimum;
        })}";
    }
}