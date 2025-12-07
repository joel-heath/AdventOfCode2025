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

    public string SolvePart1(string input)
    {
        Grid<char> grid = Grid<char>.FromString(input);
        HashSet<long> beams = [grid.AllPositions().Select(p => (c: grid[p], p)).First(x => x.c == 'S').p.X];
        int splits = 0;

        for (int y = 1; y < grid.Height - 1; y++)
        {
            HashSet<long> currentBeams = [..beams];
            foreach (long x in currentBeams)
            {
                Point p = new(x, y);
                char cell = grid[p];
                if (cell == '^')
                {

                    if (grid.Contains((x - 1, y + 1)))
                        beams.Add(x - 1);
                    if (grid.Contains((x + 1, y + 1)))
                        beams.Add(x + 1);
                    if (beams.Remove(x))
                        splits++;
                }
            }
        }

        return $"{splits}";
    }

    public string SolvePart2(string input)
    {
        Grid<char> grid = Grid<char>.FromString(input);
        HashSet<(long X, long Paths)> beams = [(grid.AllPositions().Select(p => (c: grid[p], p)).First(x => x.c == 'S').p.X, 1)];

        for (int y = 1; y < grid.Height - 1; y++)
        {
            HashSet<(long X, long Paths)> currentBeams = [..beams];
            foreach ((long x, long paths) in currentBeams)
            {
                Point p = new(x, y);
                char cell = grid[p];
                if (cell == '^')
                {
                    Point[] news = [(x - 1, y + 1), (x + 1, y + 1)];
                    foreach (var np in news)
                    {
                        if (grid.Contains(np))
                        {
                            var existing = beams.FirstOrDefault(b => b.X == np.X);
                            if (existing != default)
                            {
                                beams.Remove(existing);
                                beams.Add((existing.X, existing.Paths + paths));
                            }
                            else
                            {
                                beams.Add((np.X, paths));
                            }
                        }
                    }

                    beams.Remove((x, paths));
                }
            }    
        }

        return $"{beams.Sum(x => x.Paths)}";
    }
}