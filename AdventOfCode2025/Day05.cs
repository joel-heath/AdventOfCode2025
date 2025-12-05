using AdventOfCode2025.Utilities;
using System;

namespace AdventOfCode2025;

public class Day05 : IDay
{
    public int Day => 5;
    public Dictionary<string, string> UnitTestsP1 { get; } = new() 
    {
        { "3-5\r\n10-14\r\n16-20\r\n12-18\r\n\r\n1\r\n5\r\n8\r\n11\r\n17\r\n32", "3" }
    };
    public Dictionary<string, string> UnitTestsP2 { get; } = new()
    {
        { "3-5\r\n10-14\r\n16-20\r\n12-18\r\n\r\n1\r\n5\r\n8\r\n11\r\n17\r\n32", "14" }
    };

    public static (int Increment, long End) ExtendRange(List<(long Start, long End)> ranges, int i, long currentStart, long currentEnd)
    {
        if (i >= ranges.Count)
            return (0, currentEnd);

        (long start, long end) = ranges[i];

        if (start > currentEnd)
            return (0, currentEnd);

        (int inc, long newEnd) = ExtendRange(ranges, i + 1, currentStart, Math.Max(currentEnd, end));

        return (inc + 1, newEnd);
    }

    public static IEnumerable<(long Start, long End)> SimplifyRanges(List<(long Start, long End)> ranges, int i = 0)
    {
        if (i >= ranges.Count)
            return [];

        (long start, long end) = ranges[i];
        (int increment, long newEnd) = ExtendRange(ranges, i + 1, start, end);

        return [(start, newEnd), .. SimplifyRanges(ranges, i + increment + 1)];
    }

    public string SolvePart1(string input)
    {
        var (freshIDs, availableIDs) = input.Split(Environment.NewLine + Environment.NewLine).Select(h => h.Lines());
        var ranges = freshIDs.Select(line => line.Split('-'))
                             .Select(tokens => (Start: long.Parse(tokens[0]), End: long.Parse(tokens[1])))
                             .ToList();

        return $"{availableIDs.Select(long.Parse).Count(num => ranges.Any(range => range.Start <= num && num <= range.End))}";
    }

    public string SolvePart2(string input)
        => $"{SimplifyRanges([..input.Split(Environment.NewLine + Environment.NewLine)[0]
                .Lines()
                .Select(line => line.Split('-'))
                .Select(tokens => (Start: long.Parse(tokens[0]), End: long.Parse(tokens[1])))
                .OrderBy(range => range.Start)])
            .Sum(r => r.End - r.Start + 1)}";
}