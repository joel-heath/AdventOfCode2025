using AdventOfCode2025.Utilities;
using System.Text.RegularExpressions;
using Highs;

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
        => new(IndicatorLightsDiagram, ButtonWiringSchematics, JoltageRequirements) { IndicatorLights = IndicatorLights };

    private void ToggleLight(int light)
        => IndicatorLights ^= 1UL << light;

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

public partial class Day10 : IDay
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
        => $"{Parse(input).Sum(Configure)}";

    private static IEnumerable<Machine> Parse(string input)
        => input.Lines().Select(line => new Machine(
            [..LightDiagramPattern().Match(line)
                .Groups.Values.ElementAt(1).Value
                .Select(l => l == '#')],
            [..ButtonSchematicPattern().Matches(line)
                .Select(match => match.Groups.Values.ElementAt(1).Value)
                .Select(counters => counters.Split(',').Select(int.Parse).ToArray())],
            [..JoltageRequirementsPattern().Match(line)
                .Groups.Values.ElementAt(1).Value
                .Split(',').Select(int.Parse)]));

    // ILP arrangement: 

    // Each variable will represent the number of times we push the corresponding button
    // Objective: Minimize the sum of all variables (total button presses)

    // Constraints:
    // The counters must equal the required configuration
    // So we have n constraints given n counters (and n requirements)
    public string SolvePart2(string input)
        => $"{Parse(input).Sum(machine => Solve(
            [.. Enumerable.Repeat(1, machine.ButtonWiringSchematics.Length)],
            [..machine.JoltageRequirements
                .Select((counter, i) =>
                    machine.ButtonWiringSchematics
                        .Select(b => b.Contains(i) ? 1.0 : 0.0)
                        .Append(counter)
                        .ToArray())]))}";


    // AI generated function below:
    // HiGHS has some GOOFY API
    private static int Solve(double[] objectiveFunction, double[][] rawConstraints)
    {
        int numCols = objectiveFunction.Length;
        int numRows = rawConstraints.Length;

        // Arrays for HiGHS
        double[] rowLower = new double[numRows];
        double[] rowUpper = new double[numRows];

        // Sparse Matrix Builders
        List<int> astart = [];
        List<int> aindex = [];
        List<double> avalue = [];

        // Assuming all variables are >= 0. Change to -1e30 if they can be negative.
        double[] colLower = new double[numCols]; // 0s by default
        double[] colUpper = [..Enumerable.Repeat(1.0e30, numCols)];

        int currentNzCount = 0;

        for (int i = 0; i < numRows; i++)
        {
            // Mark the start of this row in the sparse value array
            astart.Add(currentNzCount);

            // 1. EXTRACT RHS: The last element of your array
            double rhs = rawConstraints[i][numCols]; // or rawConstraints[i].Last()

            // 2. DEFINE EQUALITY: Lower Bound == Upper Bound == RHS
            rowLower[i] = rhs;
            rowUpper[i] = rhs;

            // 3. BUILD SPARSE MATRIX: Iterate only the coefficients
            for (int j = 0; j < numCols; j++)
            {
                double coeff = rawConstraints[i][j];

                // OPTIMIZATION: Only add non-zero values. 
                // This is the "Sparse" part that makes HiGHS fast.
                if (Math.Abs(coeff) > 1e-9)
                {
                    aindex.Add(j);      // Which column (variable) is this?
                    avalue.Add(coeff);  // What is the value?
                    currentNzCount++;
                }
            }
        }
        // Add final entry to astart (standard CSR format requirement)
        astart.Add(currentNzCount);


        // --- 3. Set Integrality (All Integers) ---
        int[] integrality = Enumerable.Repeat((int)HighsIntegrality.kInteger, numCols).ToArray();

        // --- 4. Configure & Run ---
        HighsModel model = new(
            objectiveFunction,
            colLower,
            colUpper,
            rowLower,
            rowUpper,
            [..astart],
            [..aindex],
            [..avalue],
            integrality,
            0,
            HighsMatrixFormat.kRowwise, // Important: We built it Row-wise
            HighsObjectiveSense.kMinimize
        );

        HighsLpSolver solver = new();
        solver.setBoolOptionValue("output_flag", 0); // no spamming

        solver.passMip(model); // passMip for Integers
        solver.run();

        return (int)Math.Round(solver.getInfo().ObjectiveValue);
    }

    [GeneratedRegex(@"\[([.#]+)\]")]
    private static partial Regex LightDiagramPattern();
    [GeneratedRegex(@"\((\d+(,\d+)*)\)")]
    private static partial Regex ButtonSchematicPattern();
    [GeneratedRegex(@"\{(\d+(,\d+)*)\}")]
    private static partial Regex JoltageRequirementsPattern();
}