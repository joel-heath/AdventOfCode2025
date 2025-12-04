using AdventOfCode2025.Utilities;

namespace AdventOfCode2025;

public class Day04 : IDay
{
    public int Day => 4;
    public Dictionary<string, string> UnitTestsP1 { get; } = new() 
    {
        { "..@@.@@@@.\r\n@@@.@.@.@@\r\n@@@@@.@.@@\r\n@.@@@@..@.\r\n@@.@@@@.@@\r\n.@@@@@@@.@\r\n.@.@.@.@@@\r\n@.@@@.@@@@\r\n.@@@@@@@@.\r\n@.@.@@@.@.", "13" },
    };
    public Dictionary<string, string> UnitTestsP2 { get; } = new()
    {
        { "..@@.@@@@.\r\n@@@.@.@.@@\r\n@@@@@.@.@@\r\n@.@@@@..@.\r\n@@.@@@@.@@\r\n.@@@@@@@.@\r\n.@.@.@.@@@\r\n@.@@@.@@@@\r\n.@@@@@@@@.\r\n@.@.@@@.@.", "43" },
    };

    private static int CountAccessible(Grid<char> grid)
        => grid.AllPositions()
               .Select(p => (p, c:grid[p]))
               .Where(t => t.c == '@')
               .Select(t => grid.Adjacents(t.p, includeDiagonals: true))
               .Count(ps => ps.Count(adj => grid[adj] == '@') < 4);

    private static int RemoveAll(Grid<char> grid)
    {
        int changed, total = 0;
        do
        {
            changed = 0;
            var newGrid = grid.ToGrid();

            foreach (Point p in grid.AllPositions())
            {
                char c = grid[p];
                if (c == '@')
                {
                    if (grid.Adjacents(p, includeDiagonals: true)
                    .Count(adj => grid[adj] == '@') < 4)
                    {
                        changed++;
                        newGrid[p] = '.';
                    }
                }
            }

            total += changed;
            grid = newGrid;
        }
        while (changed > 0);

        return total;
    }

    public string SolvePart1(string input)
        => $"{CountAccessible(Grid<char>.FromString(input))}";

    public string SolvePart2(string input)
        => $"{RemoveAll(Grid<char>.FromString(input))}";

}