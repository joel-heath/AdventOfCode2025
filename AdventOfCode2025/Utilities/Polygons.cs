using System.Collections;

namespace AdventOfCode2025.Utilities;
public readonly record struct Rectangle : IEnumerable<Point>
{
    public readonly Point MinMin { get; }
    public readonly Point MaxMin { get; }
    public readonly Point MinMax { get; }
    public readonly Point MaxMax { get; }

    public readonly long Area() => (MaxMax.X - MinMin.X + 1) * (MaxMax.Y - MinMin.Y + 1);

    public Rectangle(Point a, Point b)
    {
        (long maxX, long minX) = a.X > b.X ? (a.X, b.X) : (b.X, a.X);
        (long maxY, long minY) = a.Y > b.Y ? (a.Y, b.Y) : (b.Y, a.Y);

        MinMin = (minX, minY);
        MaxMin = (maxX, minY);
        MinMax = (minX, maxY);
        MaxMax = (maxX, maxY);
    }

    public readonly IEnumerator<Point> GetEnumerator()
    {
        yield return MinMin;
        yield return MaxMin;
        yield return MinMax;
        yield return MaxMax;
    }

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public readonly record struct Line : IEnumerable<Point>
{
    public readonly Point Min { get; }
    public readonly Point Max { get; }

    public readonly bool IsHorizontal => Min.Y == Max.Y;
    public readonly bool IsVertical => Min.X == Max.X;

    public Line(Point a, Point b)
    {
        (Min, Max) = a.X < b.X || a.Y < b.Y ? (a, b) : (b, a);
    }

    public readonly IEnumerator<Point> GetEnumerator()
    {
        yield return Min;
        yield return Max;
    }

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public static class Polygons
{
    /// <summary>
    /// Returns all edges from a collection of vertices, automatically closing the polygon for you.
    /// </summary>
    /// <param name="polygon">??? the polygon bro what</param>
    /// <returns>if only i had alr explained.</returns>
    public static IEnumerable<Line> Edges(this Point[] polygon)
    {
        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            yield return new Line(polygon[i], polygon[j]);
        }
    }

    /// <summary>
    /// Checks whether two rectangles intersect or overlap with axis-aligned bounding box intersection.
    /// </summary>
    /// <param name="a">The first rectangle to test for intersection.</param>
    /// <param name="b">The second rectangle to test for intersection.</param>
    /// <returns>Figure it out lil bro</returns>
    public static bool AABB(Rectangle a, Rectangle b)
    {
        bool aRightOfB = a.MinMin.X > b.MaxMax.X,
             aLeftOfB = a.MaxMax.X < b.MinMin.X,
             aAboveB = a.MaxMax.Y < b.MinMin.Y,
             aBelowB = a.MinMin.Y > b.MaxMax.Y;

        return !aRightOfB && !aLeftOfB && !aAboveB && !aBelowB;
    }

    /// <summary>
    /// Checks if a line intersects the strict interior of the rectangle, so returns false for a rectangle's own edges.
    /// Assumes line is horizontal or vertical.
    /// </summary>
    /// <param name="line">Line segment</param>
    /// <param name="rect">Rectangle</param>
    /// <returns>I alr explained this</returns>
    public static bool LineIntersectsRectangle(Line line, Rectangle rect)
        => line.IsHorizontal
            ? rect.MinMin.Y < line.Min.Y && line.Min.Y < rect.MaxMax.Y
                && line.Min.X < rect.MaxMax.X && line.Max.X > rect.MinMin.X
            : rect.MinMin.X < line.Min.X && line.Min.X < rect.MaxMax.X
                && line.Min.Y < rect.MaxMax.Y && line.Max.Y > rect.MinMin.Y;

    public static bool PointInLine(Point point, Line line)
        => line.Min.X <= point.X && point.X <= line.Max.X &&
           line.Min.Y <= point.Y && point.Y <= line.Max.Y;

    /// <summary>
    /// Determines whether a point lies in a rectangle, strict to exclude boundary.
    /// </summary>
    /// <param name="point">the point</param>
    /// <param name="rect">the rectangle or "rectange" as the kids call it these days</param>
    /// <param name="strict">alr explained pay attention</param>
    /// <returns>take a guess</returns>
    public static bool PIP(Point point, Rectangle rect, bool strict = true)
        => strict
        ? rect.MinMin.X < point.X && point.X < rect.MaxMax.X &&
          rect.MinMin.Y < point.Y && point.Y < rect.MaxMax.Y
        : rect.MinMin.X <= point.X && point.X <= rect.MaxMax.X &&
          rect.MinMin.Y <= point.Y && point.Y <= rect.MaxMax.Y;

    /// <summary>
    /// Returns whether `point` is inside `polygon` using the even-odd rule. Polygon assumed to be simple.
    /// </summary>
    /// <param name="point">Point to be tested for inclusion.</param>
    /// <param name="polygon">Vertices of the polygon.</param>
    /// <param name="strict">Whether to consider points on the boundary as outside or inside.</param>
    /// <returns>hmmm i wonder what...</returns>
    public static bool PIP(Point point, Point[] polygon, bool strict = true)
    {
        if (polygon.Edges().Any(e => PointInLine(point, e)))
            return !strict;

        bool inside = false;

        foreach (var edge in polygon.Edges())
        {
            if (edge.IsHorizontal)
                continue;

            // check if line segment l intersects the horizontal half-line stretching from x = -inf to end

            // half-open interval [Min, Max) to avoid double-counting vertices (since each vertex is shared between two edges)
            bool isLevel = edge.Min.Y <= point.Y && point.Y < edge.Max.Y;

            // Check if the edge is strictly to the left of the point
            bool isToLeft = edge.Min.X < point.X;

            if (isLevel && isToLeft)
                inside = !inside;
        }

        return inside;
    }
}
