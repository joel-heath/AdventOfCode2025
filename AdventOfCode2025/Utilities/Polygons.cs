namespace AdventOfCode2025.Utilities;

public static class Polygons
{
    public static bool IsPointOnSegment(Point a, Point b, Point p)
    {
        // Vector cross product = 0 → collinear
        long cross = (p.Y - a.Y) * (b.X - a.X)
                   - (p.X - a.X) * (b.Y - a.Y);

        if (cross != 0)
            return false;

        // Dot product check → within the bounding box of the segment
        long dot = (p.X - a.X) * (b.X - a.X)
                 + (p.Y - a.Y) * (b.Y - a.Y);

        if (dot < 0)
            return false;

        long lenSq = (b.X - a.X) * (b.X - a.X)
                   + (b.Y - a.Y) * (b.Y - a.Y);

        return dot <= lenSq;
    }


    public static bool IsInteriorOrOnBoundary(Point[] polygon, Point p)
    {
        // Boundary Test
        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            if (IsPointOnSegment(polygon[j], polygon[i], p))
                return true;
        }
        return IsInterior(polygon, p);
    }

    public static bool IsInterior(Point[] polygon, Point p)
    {
        int n = polygon.Length;
        bool inside = false;
        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            if (((polygon[i].Y > p.Y) != (polygon[j].Y > p.Y)) &&
                (p.X < (double)(polygon[j].X - polygon[i].X) * (p.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                    inside = !inside;
        }
        return inside;
    }

    private static Point GetStart(Point[] boundary)
    {
        var minY = boundary.Min(p => p.Y);
        var maxY = boundary.Max(p => p.Y);
        var minX = boundary.Min(p => p.X);
        var maxX = boundary.Max(p => p.X);

        for (long y = minY; y <= maxY; y++)
        {
            for (long x = minX; x <= maxX; x++)
                if (IsInterior(boundary, (x, y)))
                    return (x, y);
        }

        throw new Exception("No start point found");
    }

    public static HashSet<Point> FloodFill(Point[] boundary)
    {
        HashSet<Point> filled = [];
        Queue<Point> toFill = new([GetStart(boundary)]);
        Point[] directions = [(-1, 0), (1, 0), (0, -1), (0, 1)];
        while (toFill.TryDequeue(out Point current))
        {
            if (filled.Contains(current) || !IsInterior(boundary, current))
                continue;
            filled.Add(current);
            foreach (var dir in directions)
            {
                Point neighbour = (current.X + dir.X, current.Y + dir.Y);
                if (!filled.Contains(neighbour))
                    toFill.Enqueue(neighbour);
            }
        }
        return filled;
    }
}
