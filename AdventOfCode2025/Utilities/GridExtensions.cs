
namespace AdventOfCode2025.Utilities;

public static class GridExtensions
{
    public static T[][] ToArray2<T>(this IEnumerable<IEnumerable<T>> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return [..source.Select(row => row.ToArray())];
    }

    public static T[][][] ToArray3<T>(this IEnumerable<IEnumerable<IEnumerable<T>>> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return [.. source.Select(x => x.Select(y => y.ToArray()).ToArray())];
    }

    /// <summary>
    /// Swaps rows and columns. Truncates to the smallest dimension.
    /// </summary>
    public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        List<IEnumerator<T>> enumerators = [.. source.Select(x => x.GetEnumerator())];

        try
        {
            while (enumerators.All(e => e.MoveNext()))
                yield return enumerators.Select(e => e.Current); // might need to materialise here...
        }
        finally
        {
            foreach (var e in enumerators)
                e.Dispose();
        }
    }

    /// <summary>
    /// Flips the grid Top-to-Bottom (Mirror over X-axis).
    /// </summary>
    public static IEnumerable<IEnumerable<T>> RotateVertically<T>(this IEnumerable<IEnumerable<T>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.Reverse();
    }

    /// <summary>
    /// Flips the grid Left-to-Right (Mirror over Y-axis).
    /// </summary>
    public static IEnumerable<IEnumerable<T>> RotateHorizontally<T>(this IEnumerable<IEnumerable<T>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.Select(row => row.Reverse());
    }

    /// <summary>
    /// Rotates the grid 90 degrees Clockwise.
    /// </summary>
    public static IEnumerable<IEnumerable<T>> RotateClockwise<T>(this IEnumerable<IEnumerable<T>> source)
        => source.Transpose().RotateHorizontally();

    /// <summary>
    /// Rotates the grid 90 degrees Anti-Clockwise.
    /// </summary>
    public static IEnumerable<IEnumerable<T>> RotateAnticlockwise<T>(this IEnumerable<IEnumerable<T>> source)
        => source.Transpose().RotateVertically();
}
