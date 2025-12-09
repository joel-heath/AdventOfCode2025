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
        (long maxX, long minX) = (a.X > b.X) ? (a.X, b.X) : (b.X, a.X);
        (long maxY, long minY) = (a.Y > b.Y) ? (a.Y, b.Y) : (b.Y, a.Y);

        return minX < p.X && p.X < maxX
            && minY < p.Y && p.Y < maxY;
    }

    public static bool IsRectFullyInside(Point[] poly, Point a, Point b)
    {
        (long maxX, long minX) = (a.X > b.X) ? (a.X, b.X) : (b.X, a.X);
        (long maxY, long minY) = (a.Y > b.Y) ? (a.Y, b.Y) : (b.Y, a.Y);

        // Check crossings on each of the 4 rectangle sides
        if (EdgeCrossesExterior(poly, (minX, minY), (maxX, minY))) return false; // top
        if (EdgeCrossesExterior(poly, (minX, maxY), (maxX, maxY))) return false; // bottom
        if (EdgeCrossesExterior(poly, (minX, minY), (minX, maxY))) return false; // left
        if (EdgeCrossesExterior(poly, (maxX, minY), (maxX, maxY))) return false; // right

        return true;
    }

    public static bool EdgeCrossesExterior(Point[] polygon, Point a, Point b)
    {
        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            if (LinesIntersect(polygon[j], polygon[i], a, b))
                return true;
        }
        return false;
    }

    // cross‑product 
    public static bool LinesIntersect(Point a1, Point a2, Point b1, Point b2)
    {
        long det = (a2.X - a1.X) * (b2.Y - b1.Y) - (a2.Y - a1.Y) * (b2.X - b1.X);
        if (det == 0)
            return false; // parallel lines
        long ua = (b2.X - b1.X) * (a1.Y - b1.Y) - (b2.Y - b1.Y) * (a1.X - b1.X);
        long ub = (a2.X - a1.X) * (a1.Y - b1.Y) - (a2.Y - a1.Y) * (a1.X - b1.X);
        if (det < 0)
        {
            ua = -ua;
            ub = -ub;
            det = -det;
        }
        return 0 < ua && ua < det && 0 < ub && ub < det;
    }


}
