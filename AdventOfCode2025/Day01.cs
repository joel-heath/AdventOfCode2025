using AdventOfCode2025.Utilities;
using System.Text.RegularExpressions;

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
    {
        var lines = input.Split(Environment.NewLine);

        int count = 0;
        int dial = 50;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            
            bool left = line[0] == 'L';
            int dir = int.Parse(line[1..]);

            if (left)
                dial -= dir;
            else
                dial += dir;


            dial = (int)Utils.Mod(dial, 100);

            if (dial == 0)
                count++;
        }


        return $"{count}";
    }

    public string SolvePart2(string input)
    {
        var lines = input.Split(Environment.NewLine);

        int count = 0;
        int dial = 50;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            bool left = line[0] == 'L';
            int dir = int.Parse(line[1..]);

            bool dialStartedAtZero = dial == 0;
            

            if (left)
            {
                dial -= dir;
                if (dial <= 0)
                    count += (dialStartedAtZero ? 0 : 1) + -dial / 100;
            }
            else
            {
                dial += dir;
                if (dial >= 100)
                    count += dial / 100;
            }

            dial = (int)Utils.Mod(dial, 100);
        }

        return $"{count}";
    }
}