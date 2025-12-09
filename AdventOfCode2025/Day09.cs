using AdventOfCode2025.Utilities;
using System.Linq;

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

        //return Memo[p] = Polygons.IsInside(reds, p);
        return Memo[p] = Polygons.PIP(p, reds, strict: false);
    }

    public string SolvePart2(string input)
    {
        Point[] reds = [..input.Lines().Select(l => l.Split(',').Select(long.Parse).ToPoint())];
        (Point Min, Point Max)[] pairs = [..reds
            .Choose(2)
            .Select(x => x.ToTuple())
            .Select(x => x.A.X < x.B.X || x.A.Y < x.B.Y ? x : (x.B, x.A))];

        (Point Min, Point Max)[] edges = [.. reds
            .Append(reds[0])
            .Window(2)
            .Select(edge =>
                edge[0].X < edge[1].X ? edge.ToTuple()
                : edge[0].Y < edge[1].Y ? edge.ToTuple()
                : (edge[1], edge[0])
            )];

        return $"{pairs
            .Where(vertices =>
            {
                var (min, max) = vertices;
                Point[] fullVertices = [min, (min.X, max.Y), (max.X, min.Y), max];

                // 1. Implicit Corners & 2. Internal Vertices
                if (!Contained(reds, fullVertices[1]) || !Contained(reds, fullVertices[2])
                    || reds.Any(r => Polygons.PIP(r, new Rectangle(min, max))))
                    return false;

                // 3. Slicing Edges Check
                // (Ensures no walls run completely through the rectangle)
                if (edges.Any(e => Polygons.LineIntersectsRectangle(new Line(e.Min, e.Max), new Rectangle(min, max))))
                    return false;

                // 4. Midpoint Substance Check
                // We proved the rect is empty; now we must prove it is "filled" with polygon.
                var midpoint = new Point((min.X + max.X) / 2, (min.Y + max.Y) / 2);
                return Polygons.PIP(midpoint, reds, strict: false);
            })
            .Max(x => (Math.Abs(x.Min.X - x.Max.X) + 1) * (Math.Abs(x.Min.Y - x.Max.Y) + 1))}";
    }
}