using AdventOfCode2025.Utilities;
using System.ComponentModel;
using System.Text.RegularExpressions;

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
                   .Max(x => (Math.Abs(x.A.X - x.B.X) + 1) * (Math.Abs(x.A.Y - x.B.Y) + 1))}";

    private readonly Dictionary<Point, bool> Memo = [];
    private bool Contained(Point[] reds, Point p)
    {
        if (Memo.TryGetValue(p, out bool res))
            return res;

        return Memo[p] = Polygons.IsInteriorOrOnBoundary(reds, p);
    }

    public string SolvePart2(string input)
    {
        Point[] reds = [..input.Lines().Select(l => l.Split(',').Select(long.Parse).ToPoint())];

        return $"{reds.Choose(2).Select(x => x.ToTuple())
            .Where(vertices =>
            {
                Point[] fullVertices = [vertices.A, (vertices.A.X, vertices.B.Y), vertices.B, (vertices.B.X, vertices.A.Y)];
                Point[] bounds = 
                    [..fullVertices.Append(fullVertices[0])
                        .Window(2)
                        .SelectMany(x => x[0].LineTo(x[1], inclusive: false))];

                return bounds.All(p => Contained(reds, p));
            })
            .Max(x => (Math.Abs(x.A.X - x.B.X) + 1) * (Math.Abs(x.A.Y - x.B.Y) + 1))}";
    }
}