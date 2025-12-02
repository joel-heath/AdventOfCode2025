using AdventOfCode2025.Utilities;

namespace AdventOfCode2025;

public class Day02 : IDay
{
    public int Day => 2;
    public Dictionary<string, string> UnitTestsP1 { get; } = new() 
    {
        { "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124", "1227775554" },
    };
    public Dictionary<string, string> UnitTestsP2 { get; } = new()
    {
        { "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124", "4174379265" },
    };

    private bool IsInvalid1(long number)
    {
        string n = number.ToString();
        if (n.Length % 2 == 1) return false;
        int half = n.Length / 2;

        return n[..half] == n[half..];
    }

    private bool IsInvalid2(long number)
    {
        string n = number.ToString();
        for (int chunkSize = 1; chunkSize <= n.Length / 2; chunkSize++)
        {
            var stuff = n.Chunk(chunkSize)
                .Select(x => new string(x))
                .Distinct().ToList();

            if (stuff.Count == 1)
                return true;

        }
        return false;
    }

    public string SolvePart1(string input)
       => $"{input.Split(',')
            .Select(line => line.Split('-').Select(long.Parse).ToArray())
            .Select(list => (Start: list[0], End: list[1]))
            .SelectMany(r => Utils.Range(r.Start, r.End - r.Start + 1).Where(IsInvalid1))
            .Sum()}";

    public string SolvePart2(string input)
       => $"{input.Split(',')
            .Select(line => line.Split('-').Select(long.Parse).ToArray())
            .Select(list => (Start: list[0], End: list[1]))
            .SelectMany(r => Utils.Range(r.Start, r.End - r.Start + 1).Where(IsInvalid2))
            .Sum()}";
}