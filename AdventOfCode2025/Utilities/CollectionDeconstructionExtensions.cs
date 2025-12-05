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
}
