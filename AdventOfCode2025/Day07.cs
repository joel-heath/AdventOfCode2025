using AdventOfCode2025.Utilities;
using System.Linq;

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
            Dictionary<long, long> currentBeams = new(beams);
            foreach ((long x, long paths) in currentBeams.Where(b => grid[b.Key, y] == '^'))
            {
                Point[] children = [(x - 1, y + 1), (x + 1, y + 1)];
                foreach (var child in children.Where(grid.Contains))
                {
                    if (beams.TryGetValue(child.X, out long existingPaths))
                    {
                        beams.Remove(child.X);
                        beams.Add(child.X, existingPaths + paths);
                    }
                    else beams.Add(child.X, paths);
                }
                beams.Remove(x);
            }    
        }

        return $"{beams.Values.Sum()}";
    }
}