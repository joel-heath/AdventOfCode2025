namespace AdventOfCode2025.Utilities;

public static class CollectionDeconstructionExtensions
{
    // clearly C# is lacking in this area

    public static void Deconstruct<T>(this IEnumerable<T> array, out T el0, out T el1)
    {
        using var enumerator = array.GetEnumerator();
        enumerator.MoveNext();
        el0 = enumerator.Current;
        enumerator.MoveNext();
        el1 = enumerator.Current;
    }

    public static void Deconstruct<T>(this IEnumerable<T> array, out T el0, out T el1, out T el2)
    {
        using var enumerator = array.GetEnumerator();
        enumerator.MoveNext();
        el0 = enumerator.Current;
        enumerator.MoveNext();
        el1 = enumerator.Current;
        enumerator.MoveNext();
        el2 = enumerator.Current;
    }

    public static void Deconstruct<T>(this IEnumerable<T> array, out T el0, out T el1, out T el2, out T el3)
    {
        using var enumerator = array.GetEnumerator();
        enumerator.MoveNext();
        el0 = enumerator.Current;
        enumerator.MoveNext();
        el1 = enumerator.Current;
        enumerator.MoveNext();
        el2 = enumerator.Current;
        enumerator.MoveNext();
        el3 = enumerator.Current;
    }

    public static void Deconstruct<T>(this IEnumerable<T> array, out T el0, out T el1, out T el2, out T el3, out T el4)
    {
        using var enumerator = array.GetEnumerator();
        enumerator.MoveNext();
        el0 = enumerator.Current;
        enumerator.MoveNext();
        el1 = enumerator.Current;
        enumerator.MoveNext();
        el2 = enumerator.Current;
        enumerator.MoveNext();
        el3 = enumerator.Current;
        enumerator.MoveNext();
        el4 = enumerator.Current;
    }

    public static (T A, T B) ToTuple<T>(this IEnumerable<T> array)
    {
        using var enumerator = array.GetEnumerator();
        enumerator.MoveNext();
        var el0 = enumerator.Current;
        enumerator.MoveNext();
        var el1 = enumerator.Current;
        return (el0, el1);
    }

    public static (T A, T B, T C) ToTriple<T>(this IEnumerable<T> array)
    {
        using var enumerator = array.GetEnumerator();
        enumerator.MoveNext();
        var el0 = enumerator.Current;
        enumerator.MoveNext();
        var el1 = enumerator.Current;
        enumerator.MoveNext();
        var el2 = enumerator.Current;
        return (el0, el1, el2);
    }

    public static (T A, T B, T C, T D) ToQuadruple<T>(this IEnumerable<T> array)
    {
        using var enumerator = array.GetEnumerator();
        enumerator.MoveNext();
        var el0 = enumerator.Current;
        enumerator.MoveNext();
        var el1 = enumerator.Current;
        enumerator.MoveNext();
        var el2 = enumerator.Current;
        enumerator.MoveNext();
        var el3 = enumerator.Current;
        return (el0, el1, el2, el3);
    }

    public static (T A, T B, T C, T D, T E) ToQuintuple<T>(this IEnumerable<T> array)
    {
        using var enumerator = array.GetEnumerator();
        enumerator.MoveNext();
        var el0 = enumerator.Current;
        enumerator.MoveNext();
        var el1 = enumerator.Current;
        enumerator.MoveNext();
        var el2 = enumerator.Current;
        enumerator.MoveNext();
        var el3 = enumerator.Current;
        enumerator.MoveNext();
        var el4 = enumerator.Current;
        return (el0, el1, el2, el3, el4);
    }


    public static Point ToPoint(this IEnumerable<long> array)
    {
        using var enumerator = array.GetEnumerator();
        enumerator.MoveNext();
        var x = enumerator.Current;
        enumerator.MoveNext();
        var y = enumerator.Current;
        return new Point(x, y);
    }

    public static Coord ToCoord(this IEnumerable<long> array)
    {
        using var enumerator = array.GetEnumerator();
        enumerator.MoveNext();
        var x = enumerator.Current;
        enumerator.MoveNext();
        var y = enumerator.Current;
        enumerator.MoveNext();
        var z = enumerator.Current;
        return new Coord(x, y, z);
    }
}
