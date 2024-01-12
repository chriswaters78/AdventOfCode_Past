(int p1, int p2) parse(Queue<int> tokens)
{
    var childResults = new List<(int p1, int p2)>();
    var childCount = tokens.Dequeue();
    var metadataCount = tokens.Dequeue();
    for (int i = 0; i < childCount; i++)
    {
        var childResult = parse(tokens);
        childResults.Add(childResult);
    }
    int mc = 0;
    int p2 = 0;
    for (int i = 0; i < metadataCount; i++)
    {
        var m = tokens.Dequeue();
        mc += m;
        if (childCount != 0 && m > 0 && m <= childCount)
        {
            p2 += childResults[m - 1].p2;
        }
    }

    return (childResults.Sum(tp => tp.p1) + mc, childCount != 0 ? p2 : mc);
}

var queue = new Queue<int>(File.ReadAllText("input.txt").Split(" ").Select(int.Parse));

(var part1, var part2) = parse(queue);
Console.WriteLine($"Part1: {part1}");
Console.WriteLine($"Part2: {part2}");

