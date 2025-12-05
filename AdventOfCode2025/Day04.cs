using AdventOfCode2025.Utilities;
using System.Numerics;

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
        => Utils.EnumerateForever()
            .AggregateWhile(
                (grid, changed: 0, total: 0),
                (state, _) =>
                {
                    (int rollsMoved, List<char> newGrid) = state.grid
                        .AllPositions()
                        .Select(p => (p, c: state.grid[p]))
                        .SelectAggregate(0,
                            (acc, t) =>
                            {
                                bool moveable = t.c == '@' && state.grid.Adjacents(t.p, includeDiagonals: true).Count(adj => state.grid[adj] == '@') < 4;

                                return moveable ? (acc + 1, '.') : (acc, t.c);
                            });

                    return (new Grid<char>(grid.Width, grid.Height, newGrid), rollsMoved, state.total + rollsMoved);
                },
                state => state.changed > 0
            ).total;

    public string SolvePart1(string input)
        => $"{CountAccessible(Grid<char>.FromString(input))}";

    public string SolvePart2(string input)
        => $"{RemoveAll(Grid<char>.FromString(input))}";

}