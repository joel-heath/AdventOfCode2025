using AdventOfCode2025.Utilities;

namespace AdventOfCode2025;

using Present = string[];
using Region = (int Width, int Height, int[] Presents);

public class Day12 : IDay
{
    public int Day => 12;
    public Dictionary<string, string> UnitTestsP1 { get; } = new() 
    {
        { "0:\r\n###\r\n##.\r\n##.\r\n\r\n1:\r\n###\r\n##.\r\n.##\r\n\r\n2:\r\n.##\r\n###\r\n##.\r\n\r\n3:\r\n##.\r\n###\r\n##.\r\n\r\n4:\r\n###\r\n#..\r\n###\r\n\r\n5:\r\n###\r\n.#.\r\n###\r\n\r\n4x4: 0 0 0 0 2 0\r\n12x5: 1 0 1 0 2 2\r\n12x5: 1 0 1 0 3 2", "2" }
    };
    public Dictionary<string, string> UnitTestsP2 { get; } = new()
    {
        { "TestInput1", "ExpectedOutput1" },
        { "TestInput2", "ExpectedOutput2" }
    };

    private static (Present[] Presents, Region[] Regions) Parse(string input)
    {
        string[][] groups = input.GroupsLines();

        Present[] presents = [.. groups[..^1].Select(p => p[1..])];
        Region[] regions = [..groups[^1]
            .Select(r => r.Split(": "))
            .Select(r => (GridSize: r[0].Split('x').Select(int.Parse).ToTuple(),
                          Presents: r[1].Split(' ').Select(int.Parse).ToArray()))
            .Select(r => (Width: r.GridSize.A, Height: r.GridSize.B, r.Presents))];

        return (presents, regions);
    }

    private static bool PresentsFit(Present[] presents, Grid<char> grid, int[] requirements)
    {
        int nextRequirement = Array.FindIndex(requirements, r => r > 0);

        if (nextRequirement == -1)
            return true;

        Present present = presents[nextRequirement];
        int pWidth = present[0].Length;
        int pHeight = present.Length;

        for (int y = 0; y <= grid.Height - pHeight; y++)
        {
            for (int x = 0; x <= grid.Width - pWidth; x++)
            {
                List<int> orientations = CanPlace(grid, present, pWidth, pHeight, y, x);
                foreach (var orientation in orientations)
                {
                    PlacePresent(grid, present, pWidth, pHeight, y, x, orientation);
                    requirements[nextRequirement]--;

                    if (PresentsFit(presents, grid, requirements))
                        return true;

                    requirements[nextRequirement]++;
                    PlacePresent(grid, present, pWidth, pHeight, y, x, orientation, unplace: true);
                }
            }
        }

        return false;
    }

    private static void PlacePresent(Grid<char> grid, string[] present, int pWidth, int pHeight, int y, int x, int orientation, bool unplace = false)
    {
        for (int py = 0; py < pHeight; py++)
        {
            for (int px = 0; px < pWidth; px++)
            {
                (int nx, int ny) = orientation switch
                {
                    0 => (px, py),
                    1 => (pWidth - 1 - px, py),
                    2 => (pWidth - 1 - px, pHeight - 1 - py),
                    3 => (px, pHeight - 1 - py),
                    _ => throw new ArgumentException("Invalid orientation")
                };

                if (present[nx][ny] == '#')
                    grid[x + px, y + py] = unplace ? '.' : '#';
            }
        }
    }

    private static List<int> CanPlace(Grid<char> grid, string[] present, int pWidth, int pHeight, int y, int x)
    {
        bool can0 = true, can90 = true, can180 = true, can270 = true;

        for (int py = 0; py < pHeight && (can0 || can90 || can180 || can270); py++)
        {
            for (int px = 0; px < pWidth && (can0 || can90 || can180 || can270); px++)
            {
                if (grid[x + px, y + py] != '.')
                {
                    // 0°: (py, px) -> (py, px)
                    if (can0 && present[py][px] == '#')
                        can0 = false;

                    // 90° anticlockwise: (py, px) -> (pWidth-1-px, py)
                    if (can90 && present[pWidth - 1 - px][py] == '#')
                        can90 = false;

                    // 180°: (py, px) -> (pHeight-1-py, pWidth-1-px)
                    if (can180 && present[pHeight - 1 - py][pWidth - 1 - px] == '#')
                        can180 = false;

                    // 90° clockwise: (py, px) -> (px, pHeight-1-py)
                    if (can270 && present[px][pHeight - 1 - py] == '#')
                        can270 = false;
                }
            }
        }

        List<int> orientations = [];
        if (can0) orientations.Add(0);
        if (can90) orientations.Add(1);
        if (can180) orientations.Add(2);
        if (can270) orientations.Add(3);
        return orientations;
    }

    public string SolvePart1(string input)
    {
        (Present[] presents, Region[] regions) = Parse(input);

        return $"{regions.Count(r =>
            PresentsFit(presents, new(r.Width, r.Height, Enumerable.Repeat('.', r.Width * r.Height)), r.Presents))}";
    }

    public string SolvePart2(string input)
    {
        return $"{string.Empty}";
    }
}