namespace AdventOfCode2025.Utilities;

public static class Polygons
{

    public static bool IsOnLineSegment(Point a, Point b, Point p)
        => (a.Y == b.Y) // horizontal
            ? p.Y == a.Y &&
                p.X >= Math.Min(a.X, b.X) &&
                p.X <= Math.Max(a.X, b.X)
            : (a.X == b.X) && p.X == a.X &&
                p.Y >= Math.Min(a.Y, b.Y) &&
                p.Y <= Math.Max(a.Y, b.Y);

    public static bool IsStrictlyInterior(Point[] polygon, Point p)
    {
        bool inside = false;

        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            Point a = polygon[i],
                  b = polygon[j];

            // Ignore horizontal edges lying on ray
            if (a.Y == b.Y)
                continue;

            bool intersects =
                (a.Y > p.Y) != (b.Y > p.Y) &&
                p.X < a.X + (double)(b.X - a.X) * (p.Y - a.Y) / (b.Y - a.Y);

            if (intersects)
                inside = !inside;
        }

        return inside;
    }

    public static bool IsOnBoundary(Point[] polygon, Point p)
    {
        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            if (IsOnLineSegment(polygon[j], polygon[i], p))
                return true;
        }
        return false;
    }

    public static bool IsInside(Point[] polygon, Point p)
        => IsOnBoundary(polygon, p) || IsStrictlyInterior(polygon, p);


    public static bool IsInsideRectangle(Point a, Point b, Point p)
    {
        long maxX, maxY, minX, minY;
        (maxX, minX) = (a.X > b.X) ? (a.X, b.X) : (b.X, a.X);
        (maxY, minY) = (a.Y > b.Y) ? (a.Y, b.Y) : (b.Y, a.Y);

        return minX <= p.X && p.X <= maxX
            && minY <= p.Y && p.Y <= maxY;
    }

    public static bool IsStrictlyInsideRectangle(Point a, Point b, Point p)
    {
        long maxX, maxY, minX, minY;
        (maxX, minX) = (a.X > b.X) ? (a.X, b.X) : (b.X, a.X);
        (maxY, minY) = (a.Y > b.Y) ? (a.Y, b.Y) : (b.Y, a.Y);

        return minX < p.X && p.X < maxX
            && minY < p.Y && p.Y < maxY;
    }
}
