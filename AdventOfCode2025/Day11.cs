using AdventOfCode2025.Utilities;

namespace AdventOfCode2025;

public class Day11 : IDay
{
    public int Day => 11;
    public Dictionary<string, string> UnitTestsP1 { get; } = new() 
    {
        { "aaa: you hhh\r\nyou: bbb ccc\r\nbbb: ddd eee\r\nccc: ddd eee fff\r\nddd: ggg\r\neee: out\r\nfff: out\r\nggg: out\r\nhhh: ccc fff iii\r\niii: out", "5" },
    };
    public Dictionary<string, string> UnitTestsP2 { get; } = new()
    {
        { "svr: aaa bbb\r\naaa: fft\r\nfft: ccc\r\nbbb: tty\r\ntty: ccc\r\nccc: ddd eee\r\nddd: hub\r\nhub: fff\r\neee: dac\r\ndac: fff\r\nfff: ggg hhh\r\nggg: out\r\nhhh: out", "2" },
    };

    private static readonly Dictionary<(int curr, int end), long> memo = [];

    private static long CountAllPaths(List<(string name, Dictionary<int, int> edges)> graph, int currentIndex, int endIndex)
    {
        if (currentIndex == endIndex)
            return 1;

        if (memo.TryGetValue((currentIndex, endIndex), out long cached))
            return cached;

        long paths = 0;

        foreach ((int dest, int _) in graph[currentIndex].edges)
            paths += CountAllPaths(graph, dest, endIndex);
        
        return memo[(currentIndex, endIndex)] = paths;
    }

    private static (List<(string name, Dictionary<int, int> edges)> graph, Dictionary<string, int> nameToIndexMap) ParseInput(string input)
        =>  Utils.GenerateGraph(
                input.Lines()
                .Select(line => line.Split(": "))
                .Select(parts => (source: parts[0], destinations: parts[1].Split(' ')))
                .SelectMany(parts => parts.destinations.Select(d => (parts.source, destination: d, weight: 1))),
            directed: true);

    public string SolvePart1(string input)
    {
        var (graph, nameToIndexMap) = ParseInput(input);

        return $"{CountAllPaths(graph, nameToIndexMap["you"], nameToIndexMap["out"])}";
    }

    public string SolvePart2(string input)
    {
        var (graph, nameToIndexMap) = ParseInput(input);

        return $"{CountAllPaths(graph, nameToIndexMap["svr"], nameToIndexMap["fft"])
                * CountAllPaths(graph, nameToIndexMap["fft"], nameToIndexMap["dac"])
                * CountAllPaths(graph, nameToIndexMap["dac"], nameToIndexMap["out"])

                + CountAllPaths(graph, nameToIndexMap["svr"], nameToIndexMap["dac"])
                * CountAllPaths(graph, nameToIndexMap["dac"], nameToIndexMap["fft"])
                * CountAllPaths(graph, nameToIndexMap["fft"], nameToIndexMap["out"])}";
    }
}