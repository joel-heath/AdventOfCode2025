using AdventOfCode2025.Utilities;

namespace AdventOfCode2025;

using Present = string[];
using Region = (int Width, int Height, int[] Presents);

public class PresentComparer : IEqualityComparer<Present>
{
    public bool Equals(Present? x, Present? y)
    {
        if (x is null || y is null)
            return false;
        if (x.Length != y.Length)
            return false;
        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] != y[i])
                return false;
        }
        return true;
    }
    public int GetHashCode(Present obj)
    {
        var hash = new HashCode();
        foreach (var line in obj)
            hash.Add(line);
        return hash.ToHashCode();
    }
}

// you could use an array for length generalisation but creating new arrays is slow
public readonly record struct MemoKey
{
    private readonly string grid;
    private readonly int r0, r1, r2, r3, r4;

    private MemoKey(string grid, int r0, int r1, int r2, int r3, int r4)
    {
        this.grid = grid;
        this.r0 = r0;
        this.r1 = r1;
        this.r2 = r2;
        this.r3 = r3;
        this.r4 = r4;
    }

    public MemoKey(char[][] grid, int[] requirements)
        : this(CanonicalKey(grid), requirements[0], requirements[1], requirements[2], requirements[3], requirements[4]) { }

    public bool Equals(MemoKey other)
        => StringComparer.Ordinal.Equals(grid, other.grid)
              && r0 == other.r0
              && r1 == other.r1
              && r2 == other.r2
              && r3 == other.r3
              && r4 == other.r4;

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(StringComparer.Ordinal.GetHashCode(grid));
        hash.Add(r0);
        hash.Add(r1);
        hash.Add(r2);
        hash.Add(r3);
        hash.Add(r4);
        return hash.ToHashCode();
    }

    private static string CanonicalKey(char[][] g)
    {
        string canonical = Serialise0(g);

        string s90 = Serialise90(g);
        if (string.CompareOrdinal(s90, canonical) < 0) canonical = s90;

        string s180 = Serialise180(g);
        if (string.CompareOrdinal(s180, canonical) < 0) canonical = s180;

        string s270 = Serialise270(g);
        if (string.CompareOrdinal(s270, canonical) < 0) canonical = s270;

        string sy = SerialiseY(g);
        if (string.CompareOrdinal(sy, canonical) < 0) canonical = sy;

        string sx = SerialiseX(g);
        if (string.CompareOrdinal(sx, canonical) < 0) canonical = sx;

        return canonical;
    }

    private static string Serialise0(char[][] g)
    {
        int h = g.Length;
        int w = g[0].Length;
        int len = (w + 1) * h - 1;  // lines joined by '\n', no trailing newline

        return string.Create(len, g, static (span, grid) =>
        {
            int pos = 0;
            for (int y = 0; y < grid.Length; y++)
            {
                var row = grid[y];
                row.AsSpan().CopyTo(span[pos..]);
                pos += row.Length;
                if (y != grid.Length - 1)
                    span[pos++] = '\n';
            }
        });
    }

    private static string Serialise90(char[][] g)
    {
        int h = g.Length;
        int w = g[0].Length;
        int len = (h + 1) * w - 1;

        return string.Create(len, g, static (span, grid) =>
        {
            int h = grid.Length;
            int w = grid[0].Length;
            int pos = 0;

            for (int x = 0; x < w; x++)
            {
                for (int y = h - 1; y >= 0; y--)
                    span[pos++] = grid[y][x];

                if (x != w - 1)
                    span[pos++] = '\n';
            }
        });
    }

    private static string Serialise180(char[][] g)
    {
        int h = g.Length;
        int w = g[0].Length;
        int len = (w + 1) * h - 1;

        return string.Create(len, g, static (span, grid) =>
        {
            int h = grid.Length;
            int w = grid[0].Length;
            int pos = 0;

            for (int y = h - 1; y >= 0; y--)
            {
                var row = grid[y];
                for (int x = w - 1; x >= 0; x--)
                    span[pos++] = row[x];

                if (y != 0)
                    span[pos++] = '\n';
            }
        });
    }

    private static string Serialise270(char[][] g)
    {
        int h = g.Length;
        int w = g[0].Length;
        int len = (h + 1) * w - 1;

        return string.Create(len, g, static (span, grid) =>
        {
            int h = grid.Length;
            int w = grid[0].Length;
            int pos = 0;

            for (int x = w - 1; x >= 0; x--)
            {
                for (int y = 0; y < h; y++)
                    span[pos++] = grid[y][x];

                if (x != 0)
                    span[pos++] = '\n';
            }
        });
    }

    private static string SerialiseY(char[][] g)
    {
        int h = g.Length;
        int w = g[0].Length;
        int len = (w + 1) * h - 1;
        return string.Create(len, g, static (span, grid) =>
        {
            int h = grid.Length;
            int w = grid[0].Length;
            int pos = 0;
            for (int y = 0; y < grid.Length; y++)
            {
                var row = grid[y];
                for (int x = w - 1; x >= 0; x--)
                    span[pos++] = row[x];
                if (y != grid.Length - 1)
                    span[pos++] = '\n';
            }
        });
    }

    private static string SerialiseX(char[][] g)
    {
        int h = g.Length;
        int w = g[0].Length;
        int len = (w + 1) * h - 1;
        return string.Create(len, g, static (span, grid) =>
        {
            int h = grid.Length;
            int w = grid[0].Length;
            int pos = 0;
            for (int y = h - 1; y >= 0; y--)
            {
                var row = grid[y];
                row.AsSpan().CopyTo(span[pos..]);
                pos += row.Length;
                if (y != 0)
                    span[pos++] = '\n';
            }
        });
    }
}


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

    private static Present[] GetAllOrientations(Present present)
    {
        HashSet<Present> orientations = new(new PresentComparer()) { present };
        int width = present[0].Length;
        int height = present.Length;

        // 90° anticlockwise
        Present orientation = new string[width];
        for (int x = 0; x < width; x++)
        {
            char[] row = new char[height];
            for (int y = 0; y < height; y++)
                row[y] = present[y][width - 1 - x];
            orientation[x] = new string(row);
        }
        orientations.Add(orientation);
        
        // 180°
        orientation = new string[height];
        for (int y = 0; y < height; y++)
        {
            char[] row = new char[width];
            for (int x = 0; x < width; x++)
                row[x] = present[height - 1 - y][width - 1 - x];
            orientation[y] = new string(row);
        }
        orientations.Add(orientation);

        // 90° clockwise
        orientation = new string[width];
        for (int x = 0; x < width; x++)
        {
            char[] row = new char[height];
            for (int y = 0; y < height; y++)
                row[y] = present[height - 1 - y][x];
            orientation[x] = new string(row);
        }
        orientations.Add(orientation);

        // Reflect in Y axis
        orientation = new string[height];
        for (int y = 0; y < height; y++)
        {
            char[] row = new char[width];
            for (int x = 0; x < width; x++)
                row[x] = present[y][width - 1 - x];
            orientation[y] = new string(row);
        }
        orientations.Add(orientation);

        // Reflect in X axis
        orientation = new string[width];
        for (int x = 0; x < width; x++)
        {
            char[] row = new char[height];
            for (int y = 0; y < height; y++)
                row[y] = present[height - 1 - y][x];
            orientation[x] = new string(row);
        }
        orientations.Add(orientation);

        return [..orientations];
    }

    private static Dictionary<MemoKey, bool> memo = [];

    private static bool PresentsFit(Present[][] presents, Grid<char> grid, int[] requirements)
    {
        int nextRequirement = Array.FindIndex(requirements, r => r > 0);

        if (nextRequirement == -1)
            return true;

        MemoKey key = new(grid.ToJaggedArray(), requirements);

        if (memo.TryGetValue(key, out bool result))
            return result;

        Present[] present = presents[nextRequirement];
        int pWidth = present[0][0].Length;
        int pHeight = present[0].Length;

        for (int y = 0; y <= grid.Height - pHeight; y++)
        {
            for (int x = 0; x <= grid.Width - pWidth; x++)
            {
                foreach (var orientation in present.Where(p => CanPlace(grid, p, pWidth, pHeight, y, x)))
                {
                    PlacePresent(grid, orientation, pWidth, pHeight, y, x);
                    requirements[nextRequirement]--;

                    if (PresentsFit(presents, grid, requirements))
                        return memo[key] = true;

                    requirements[nextRequirement]++;
                    PlacePresent(grid, orientation, pWidth, pHeight, y, x, unplace: true);
                }
            }
        }

        return memo[key] = false;
    }

    private static void PlacePresent(Grid<char> grid, string[] present, int pWidth, int pHeight, int y, int x, bool unplace = false)
    {
        for (int py = 0; py < pHeight; py++)
            for (int px = 0; px < pWidth; px++)
                if (present[py][px] == '#')
                    grid[x + px, y + py] = unplace ? '.' : '#';
    }

    private static bool CanPlace(Grid<char> grid, string[] present, int pWidth, int pHeight, int y, int x)
    {
        for (int py = 0; py < pHeight; py++)
            for (int px = 0; px < pWidth; px++)
                if (present[py][px] == '#' && grid[x + px, y + py] != '.')
                    return false;
        return true;
    }

    public string SolvePart1(string input)
    {
        (Present[] presents, Region[] regions) = Parse(input);

        int[] areas = [.. presents.Select(p => p.SelectMany(l => l).Count(c => c == '#'))];

        return $"{regions.Count(r =>
        {
            memo = [];

            int area = r.Width * r.Height;
            int requiredArea = r.Presents.Select((p, i) => p * areas[i]).Sum();

            // Console.WriteLine($"Region {r.Width}x{r.Height} requires {requiredArea} of {area} area.");

            if (requiredArea > area)
                return false;

            return false;
        })}";
    }

    public string SolvePart2(string input)
    {
        return $"{string.Empty}";
    }
}