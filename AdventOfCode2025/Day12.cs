using AdventOfCode2025.Utilities;

namespace AdventOfCode2025;

using Present = string[];
using Region = (int Width, int Height, int[] Presents);

public class Day12 : IDay
{
    public int Day => 12;
    public Dictionary<string, string> UnitTestsP1 { get; } = new() 
    {
        { "0:\r\n###\r\n##.\r\n##.\r\n\r\n1:\r\n###\r\n##.\r\n.##\r\n\r\n2:\r\n.##\r\n###\r\n##.\r\n\r\n3:\r\n##.\r\n###\r\n##.\r\n\r\n4:\r\n###\r\n#..\r\n###\r\n\r\n5:\r\n###\r\n.#.\r\n###\r\n\r\n4x4: 0 0 0 0 2 0\r\n12x5: 1 0 1 0 2 2\r\n12x5: 1 0 1 0 3 2", "2" }
    };
    public Dictionary<string, string> UnitTestsP2 { get; } = new()
    {
        { "TestInput1", "ExpectedOutput1" },
        { "TestInput2", "ExpectedOutput2" }
    };

    private static (Present[] Presents, Region[] Regions) Parse(string input)
    {
        string[][] groups = input.GroupsLines();

        return (
            [.. groups[..^1].Select(p => p[1..])],
            [..groups[^1]
                .Select(r => r.Split(": "))
                .Select(r => (GridSize: r[0].Split('x').Select(int.Parse).ToTuple(),
                              Presents: r[1].Split(' ').Select(int.Parse).ToArray()))
                .Select(r => (Width: r.GridSize.A, Height: r.GridSize.B, r.Presents))]);
    }

    public string SolvePart1(string input)
    {
        (Present[] presents, Region[] regions) = Parse(input);
        return $"{regions.Count(r => r.Presents.Sum() <= (int)(Math.Floor(r.Width / 3.0) * Math.Floor(r.Height / 3.0)))}";
    }

    public string SolvePart2(string input)
    {
        return $"{string.Empty}";
    }
}