using AdventOfCode2025.Utilities;

namespace AdventOfCode2025;

class Graph<T> where T : notnull
{
    private readonly Dictionary<T, List<T>> _adjacencyList = [];

    public IEnumerable<T> Nodes => _adjacencyList.Keys;
    public int Count => _adjacencyList.Count;

    public void Connect(T from, T to)
    {
        if (_adjacencyList.TryGetValue(from, out var listFrom))
            listFrom.Add(to);
        else
            _adjacencyList[from] = [to];
    }

    public void ConnectUndirected(T a, T b)
    {
        Connect(a, b);
        Connect(b, a);
    }

    public IEnumerable<T> GetNeighbours(T node)
        => _adjacencyList.GetValueOrDefault(node, []);

    public IEnumerable<T> this[T node] => GetNeighbours(node);
}

public class Day08 : IDay
{
    public int Day => 8;
    public Dictionary<string, string> UnitTestsP1 { get; } = new() 
    {
        { "162,817,812\r\n57,618,57\r\n906,360,560\r\n592,479,940\r\n352,342,300\r\n466,668,158\r\n542,29,236\r\n431,825,988\r\n739,650,466\r\n52,470,668\r\n216,146,977\r\n819,987,18\r\n117,168,530\r\n805,96,715\r\n346,949,466\r\n970,615,88\r\n941,993,340\r\n862,61,35\r\n984,92,344\r\n425,690,689", "40" },
    };
    public Dictionary<string, string> UnitTestsP2 { get; } = new()
    {
        { "162,817,812\r\n57,618,57\r\n906,360,560\r\n592,479,940\r\n352,342,300\r\n466,668,158\r\n542,29,236\r\n431,825,988\r\n739,650,466\r\n52,470,668\r\n216,146,977\r\n819,987,18\r\n117,168,530\r\n805,96,715\r\n346,949,466\r\n970,615,88\r\n941,993,340\r\n862,61,35\r\n984,92,344\r\n425,690,689", "25272" },
    };

    private static long ExploreGraph(Graph<Coord> graph, HashSet<Coord> visited, Coord start)
    {
        int size = 0;
        Queue<Coord> queue = new([start]);
        while (queue.TryDequeue(out Coord current))
        {
            size++;
            foreach (var neighbour in graph[current])
            {
                if (visited.Add(neighbour))
                    queue.Enqueue(neighbour);
            }
        }

        return size;
    }

    private static long Find3LargestCircuits(Graph<Coord> graph)
    {
        HashSet<Coord> visited = [];
        List<long> circuitSizes = [];

        foreach (var node in graph.Nodes)
        {
            if (!visited.Add(node))
                continue;

            circuitSizes.Add(ExploreGraph(graph, visited, node));
        }

        return circuitSizes
            .OrderDescending()
            .Take(3)
            .Aggregate(1L, (acc, val) => acc * val);
    }

    private static bool Connected(Graph<Coord> graph, Coord[] boxes)
    {
        if (graph.Count < boxes.Length)
            return false;
        HashSet<Coord> visited = [];
        Queue<Coord> queue = new([boxes[0]]);
        visited.Add(boxes[0]);
        while (queue.TryDequeue(out Coord current))
        {
            foreach (var neighbour in graph[current])
            {
                if (visited.Add(neighbour))
                    queue.Enqueue(neighbour);
            }
        }
        return visited.Count == boxes.Length;
    }

    private static IEnumerable<(Coord, Coord)> OrderConnections(Coord[] boxes)
        => Utils.Range(0, boxes.Length)
            .SelectMany(i => Utils.RangeTo(i + 1, boxes.Length)
                .Select(j => (boxes[i], boxes[j])))
            .OrderBy(pair =>
            {
                long dX = pair.Item1.X - pair.Item2.X,
                     dY = pair.Item1.Y - pair.Item2.Y,
                     dZ = pair.Item1.Z - pair.Item2.Z;
                return Math.Sqrt(dX * dX + dY * dY + dZ * dZ);
            });

    public string SolvePart1(string input)
    {
        int connectionsToMake = UnitTestsP1.ContainsKey(input) ? 10 : 1000;

        Coord[] boxes = [..input.Lines()
            .Select(l => l.Split(',').Select(long.Parse).ToArray())
            .Select(xs => new Coord(xs[0], xs[1], xs[2]))];

        Graph<Coord> graph = new();

        foreach ((Coord a, Coord b) in OrderConnections(boxes).Take(connectionsToMake))
            graph.ConnectUndirected(a, b);

        return $"{Find3LargestCircuits(graph)}";
    }

    public string SolvePart2(string input)
    {
        Coord[] boxes = [..input.Lines()
            .Select(l => l.Split(',').Select(long.Parse).ToArray())
            .Select(xs => new Coord(xs[0], xs[1], xs[2]))];

        Graph<Coord> graph = new();

        (Coord a, Coord b) lastConnection = default;
        foreach ((Coord a, Coord b) in OrderConnections(boxes))
        {
            lastConnection = (a, b);
            graph.ConnectUndirected(a, b);

            if (Connected(graph, boxes))
                break;
        }

        return $"{lastConnection.a.X * lastConnection.b.X}";
    }
}