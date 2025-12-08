using AdventOfCode2025.Utilities;

namespace AdventOfCode2025;

public class Day07 : IDay
{
    public int Day => 7;
    public Dictionary<string, string> UnitTestsP1 { get; } = new() 
    {
        { ".......S.......\r\n...............\r\n.......^.......\r\n...............\r\n......^.^......\r\n...............\r\n.....^.^.^.....\r\n...............\r\n....^.^...^....\r\n...............\r\n...^.^...^.^...\r\n...............\r\n..^...^.....^..\r\n...............\r\n.^.^.^.^.^...^.\r\n...............", "21" },
    };
    public Dictionary<string, string> UnitTestsP2 { get; } = new()
    {
        { ".......S.......\r\n...............\r\n.......^.......\r\n...............\r\n......^.^......\r\n...............\r\n.....^.^.^.....\r\n...............\r\n....^.^...^....\r\n...............\r\n...^.^...^.^...\r\n...............\r\n..^...^.....^..\r\n...............\r\n.^.^.^.^.^...^.\r\n...............", "40" },
    };

    private static readonly Point[] deltas = [(-1, 1), (1, 1)];

    public string SolvePart1(string input)
    {
        Grid<char> grid = Grid<char>.FromString(input);
        HashSet<long> beams = [grid.AllPositions().Select(p => (c: grid[p], p)).First(x => x.c == 'S').p.X];
        int splits = 0;

        for (int y = 1; y < grid.Height - 1; y++)
        {
            HashSet<long> currentBeams = [..beams];
            foreach (long x in currentBeams.Where(x => grid[x, y] == '^'))
            {
                if (grid.Contains((x - 1, y + 1)))
                    beams.Add(x - 1);
                if (grid.Contains((x + 1, y + 1)))
                    beams.Add(x + 1);
                beams.Remove(x);
                splits++;
            }
        }

        return $"{splits}";
    }


    public string SolvePart2(string input)
    {
        Grid<char> grid = Grid<char>.FromString(input);
        Dictionary<long, long> beams = new() { { grid.AllPositions().Select(p => (c: grid[p], p)).First(x => x.c == 'S').p.X, 1 } };

        for (int y = 1; y < grid.Height - 1; y++)
        {
            foreach ((long x, long paths) in new Dictionary<long, long>(beams.Where(b => grid[b.Key, y] == '^')))
            {
                foreach (var child in deltas.Select(p => p + (x, y)).Where(grid.Contains))
                    beams[child.X] = beams.GetValueOrDefault(child.X, 0) + paths;

                beams.Remove(x);
            }
        }

        return $"{beams.Values.Sum()}";
    }
}