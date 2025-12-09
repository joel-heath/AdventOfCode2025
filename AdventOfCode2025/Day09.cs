using AdventOfCode2025.Utilities;

namespace AdventOfCode2025;

public class Day09 : IDay
{
    public int Day => 9;
    public Dictionary<string, string> UnitTestsP1 { get; } = new() 
    {
        { "7,1\r\n11,1\r\n11,7\r\n9,7\r\n9,5\r\n2,5\r\n2,3\r\n7,3", "50" },
    };
    public Dictionary<string, string> UnitTestsP2 { get; } = new()
    {
        { "7,1\r\n11,1\r\n11,7\r\n9,7\r\n9,5\r\n2,5\r\n2,3\r\n7,3", "24" },
    };

    public string SolvePart1(string input)
        => $"{input.Lines()
                   .Select(l => l.Split(',').Select(long.Parse).ToPoint())
                   .Choose(2).Select(x => x.ToTuple())
                   .Max(x => new Rectangle(x.A, x.B).Area())}";

    public string SolvePart2(string input)
    {
        Point[] reds = [..input.Lines().Select(l => l.Split(',').Select(long.Parse).ToPoint())];

        return $"{reds
            .Choose(2)
            .Select(x => x.ToTuple())
            .Select(x => new Rectangle(x.A, x.B))
            .Where(rectangle =>
                reds.Edges().All(edge => !Polygons.LineIntersectsRectangle(edge, rectangle)))
            .Max(r => r.Area())}";
    }
}