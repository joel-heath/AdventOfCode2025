using AdventOfCode2025.Utilities;

namespace AdventOfCode2025;

public class Day03 : IDay
{
    public int Day => 3;
    public Dictionary<string, string> UnitTestsP1 { get; } = new() 
    {
        { "987654321111111\r\n811111111111119\r\n234234234234278\r\n818181911112111", "357" }
    };
    public Dictionary<string, string> UnitTestsP2 { get; } = new()
    {
        { "987654321111111\r\n811111111111119\r\n234234234234278\r\n818181911112111", "3121910778619" }
    };

    private static long MaxJolts(int[] numbers, int k)
    {
        if (k == 0) return 0;

        int max = numbers.SkipLast(k - 1).Max();
        int firstOccurance = numbers.Select((x, i) => (x, i)).First(t => t.x == max).i;
        
        return max * (long)Math.Pow(10, k - 1) + MaxJolts(numbers[(firstOccurance + 1)..], k - 1);
    }

    private static long Solve(string input, int k)
        => input.Lines()
                .Select(line => line.Select(c => c - '0').ToArray())
                .Sum(nums => MaxJolts(nums, k));

    public string SolvePart1(string input)
        => $"{Solve(input, 2)}";

    public string SolvePart2(string input)
        => $"{Solve(input, 12)}";
}