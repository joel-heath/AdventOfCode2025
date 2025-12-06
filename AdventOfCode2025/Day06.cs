using AdventOfCode2025.Utilities;

namespace AdventOfCode2025;

public class Day06 : IDay
{
    public int Day => 6;
    public Dictionary<string, string> UnitTestsP1 { get; } = new() 
    {
        { "123 328  51 64 \r\n 45 64  387 23 \r\n  6 98  215 314\r\n*   +   *   +  ", "4277556" },
    };
    public Dictionary<string, string> UnitTestsP2 { get; } = new()
    {
        { "123 328  51 64 \r\n 45 64  387 23 \r\n  6 98  215 314\r\n*   +   *   +  ", "3263827" },
    };

    private static long EvaluateProblem(bool add, IEnumerable<long> nums)
        => add ? nums.Sum() : nums.Aggregate(1L, (a, b) => a * b);

    public string SolvePart1(string input)
        => $"{input.Split(Environment.NewLine)
                .Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                .Transpose().Select(equation =>
                    EvaluateProblem(equation[^1] == "+", equation[..^1].Select(long.Parse)))
                .Sum()}";

    public string SolvePart2(string input)
        => $"{string.Join(Environment.NewLine,
                input.Split(Environment.NewLine)
                    .RotateAntiClockwise()
                    .Select(line => new string(line).Trim()))
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(problem => problem.Split(Environment.NewLine))
            .Select(equation =>
                EvaluateProblem(equation[^1][^1] == '+', equation.Select(c => long.Parse(c.Trim('+', '*')))))
            .Sum()}";
}