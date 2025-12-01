using AdventOfCode2025.Utilities;

namespace AdventOfCode2025;

public class Day01 : IDay
{
    public int Day => 1;
    public Dictionary<string, string> UnitTestsP1 { get; } = new() 
    {
        { "L68\r\nL30\r\nR48\r\nL5\r\nR60\r\nL55\r\nL1\r\nL99\r\nR14\r\nL82", "3" },
    };
    public Dictionary<string, string> UnitTestsP2 { get; } = new()
    {
        { "L68\r\nL30\r\nR48\r\nL5\r\nR60\r\nL55\r\nL1\r\nL99\r\nR14\r\nL82", "6" },
        { "R1000", "10" },
        { "R1049", "10" },
        { "R1050", "11" },
        { "L1000", "10" },
        { "L1049", "10" },
        { "L1050", "11" },
        { "L50\r\nL100\r\nR100\r\nL99\r\nL1\r\nR99\r\nR1", "5" }
    };

    public string SolvePart1(string input)
        => $"{input.Split(Environment.NewLine)
                .Select(l => (Polarity: l[0] == 'L' ? -1 : 1, Distance: int.Parse(l[1..])))
                .Aggregate(
                    (Dial: 50, Count: 0),
                    (acc, curr) =>
                    {
                        int newDial = ((int)Utils.Mod(acc.Dial + curr.Polarity * curr.Distance, 100));
                        return (newDial, acc.Count + (newDial == 0 ? 1 : 0));
                    })
                .Count}";

    public string SolvePart2(string input)
        => $"{input.Split(Environment.NewLine)
                .Select(l => (Polarity: l[0] == 'L' ? -1 : 1, Distance: int.Parse(l[1..])))
                .Aggregate(
                    (Dial: 50, Count: 0),
                    (acc, curr) =>
                    {
                        int newDial = acc.Dial + curr.Polarity * curr.Distance;
                        int increment = 
                            curr.Polarity == 1 && newDial >= 100
                                ? newDial / 100
                                : curr.Polarity == -1 && newDial <= 0
                                ? (acc.Dial == 0 ? 0 : 1) + -newDial / 100
                                : 0;

                        return ((int)Utils.Mod(newDial, 100), acc.Count + increment);
                    })
                .Count}";
}