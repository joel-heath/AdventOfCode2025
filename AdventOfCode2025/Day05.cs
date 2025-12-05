using AdventOfCode2025.Utilities;

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

    public string SolvePart1(string input)
    {
        var parts = input.Split(Environment.NewLine + Environment.NewLine);
        var ranges = parts[0].Lines()
                             .Select(line => line.Split('-'))
                             .Select(tokens => (start: long.Parse(tokens[0]), end: long.Parse(tokens[1])))
                             .ToList();

        var available = parts[1].Lines()
                             .Select(long.Parse)
                             .ToList();

        return $"{available.Count(num => ranges.Any(range => range.start <= num && num <= range.end))}";
    }

    public static IEnumerable<(long Start, long End)> MergeRanges(List<(long Start, long End)> ranges)
    {
        for (int i = 0; i < ranges.Count; i++)
        {
            (long start, long end) = ranges[i];
            
            while (i+1 < ranges.Count && ranges[i+1].Start <= end)
            {
                i++;
                end = Math.Max(end, ranges[i].End);
            }

            yield return (start, end);
        }
    }

    public string SolvePart2(string input)
    {
        var parts = input.Split(Environment.NewLine + Environment.NewLine);
        var ranges = parts[0].Lines()
                             .Select(line => line.Split('-'))
                             .Select(tokens => (Start: long.Parse(tokens[0]), End: long.Parse(tokens[1])))
                             .OrderBy(range => range.Start)
                             .ToList();

        return $"{MergeRanges(ranges).Sum(r => r.End - r.Start + 1)}";
    }
}