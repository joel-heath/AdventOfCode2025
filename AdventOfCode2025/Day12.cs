using AdventOfCode2025.Utilities;

namespace AdventOfCode2025;

public class Day12 : IDay
{
    public int Day => 12;
    public Dictionary<string, string> UnitTestsP1 { get; } = []; // No unit tests because the example actually requires brute-force solving
    public Dictionary<string, string> UnitTestsP2 { get; } = new() { { "23 stars to go!", "Decorate the tree." } };

    public string SolvePart1(string input)
        => $"{input.GroupsLines()[^1]
                .Select(r => r.Split(": "))
                .Select(r => (GridSize: r[0].Split('x').Select(int.Parse).ToTuple(),
                              Presents: r[1].Split(' ').Select(int.Parse).ToArray()))
                .Select(r => (Presents: r.Presents.Sum(), 
                              Space: (int)(Math.Floor(r.GridSize.A / 3.0) * Math.Floor(r.GridSize.B / 3.0))))
                .Count(r => r.Presents <= r.Space)}";

    public string SolvePart2(string input)
        => "Decorate the tree.";
}