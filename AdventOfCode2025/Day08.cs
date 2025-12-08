using AdventOfCode2025.Utilities;

namespace AdventOfCode2025;

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

    private static long Find3LargestCircuits(Dictionary<Coord, List<Coord>> graph)
    {
        HashSet<Coord> visited = [];
        List<long> circuitSizes = [];
        foreach (var node in graph.Keys)
        {
            if (!visited.Add(node))
                continue;
            
            long size = 0;
            Queue<Coord> queue = new([node]);
            while (queue.TryDequeue(out Coord current))
            {
                size++;
                foreach (var neighbour in graph[current])
                {
                    if (visited.Add(neighbour))
                        queue.Enqueue(neighbour);
                }
            }
            circuitSizes.Add(size);
        }

        return circuitSizes
            .OrderDescending()
            .Take(3)
            .Aggregate(1L, (acc, val) => acc * val);
    }

    private static void Connect(Dictionary<Coord, List<Coord>> graph, Coord a, Coord b)
    {
        if (graph.TryGetValue(a, out var listA))
            listA.Add(b);
        else
            graph[a] = [b];

        if (graph.TryGetValue(b, out var listB))
            listB.Add(a);
        else
            graph[b] = [a];
    }

    private static bool Connected(Dictionary<Coord, List<Coord>> graph, Coord[] boxes)
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

    private static PriorityQueue<(Coord, Coord), double> OrderConnections(Coord[] boxes)
    {
        PriorityQueue<(Coord, Coord), double> connections = new();

        for (int i = 0; i < boxes.Length; i++)
        {
            Coord iBox = boxes[i];
            for (int j = i + 1; j < boxes.Length; j++)
            {
                Coord jBox = boxes[j];

                long dX = iBox.X - jBox.X,
                     dY = iBox.Y - jBox.Y,
                     dZ = iBox.Z - jBox.Z;

                double distance = Math.Sqrt(dX * dX + dY * dY + dZ * dZ);

                connections.Enqueue((iBox, jBox), distance);
            }
        }

        return connections;
    }

    public string SolvePart1(string input)
    {
        int connectionsToMake = UnitTestsP1.ContainsKey(input) ? 10 : 1000;

        Coord[] boxes = [..input.Lines()
            .Select(l => l.Split(',').Select(long.Parse).ToArray())
            .Select(xs => new Coord(xs[0], xs[1], xs[2]))];

        PriorityQueue<(Coord, Coord), double> ordered = OrderConnections(boxes);
        Dictionary<Coord, List<Coord>> graph = [];

        for (int c = 0; c < connectionsToMake; c++)
        {
            var (a, b) = ordered.Dequeue();
            Connect(graph, a, b);
        }

        return $"{Find3LargestCircuits(graph)}";
    }

    public string SolvePart2(string input)
    {
        Coord[] boxes = [..input.Lines()
            .Select(l => l.Split(',').Select(long.Parse).ToArray())
            .Select(xs => new Coord(xs[0], xs[1], xs[2]))];

        PriorityQueue<(Coord, Coord), double> connections = OrderConnections(boxes);

        (Coord a, Coord b) lastConnection = default;
        Dictionary<Coord, List<Coord>> graph = [];
        while (!Connected(graph, boxes))
        {
            lastConnection = connections.Dequeue();
            Connect(graph, lastConnection.a, lastConnection.b);
        }

        return $"{lastConnection.a.X * lastConnection.b.X}";
    }

}