using IntCode;

var input = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();

Console.WriteLine($"*** START ***");
Console.WriteLine($"Part 1: {part1(input)}");

var tractorBeam = part2(input);

Console.WriteLine($"Part 2: {part2(input)}");

Console.WriteLine($"*** STOP ***");

static int part2(long[] input)
{
    long count = 0;
    var tractorBeam = new HashSet<(int x, int y)>();
    for (int y = 0; y < 1100; y++)
    {
        for (int x = y/2; x < y; x++)
        {
            var probe = new Computer("REPAIR", input.ToArray(), new long[] { x, y }.AsEnumerable().GetEnumerator(), false);
            probe.MoveNext();
            if (probe.Current == 1)
            {
                tractorBeam.Add((x, y));
            }
            count += probe.Current;
        }
    }

    var part2A = (from y in Enumerable.Range(0, 1000)
              from x in Enumerable.Range(0, 1000)
              select new (int x, int y)[] { (x, y), (x + 99, y), (x, y + 99), (x + 99, y + 99) })
            .First(arr => arr.All(corner => tractorBeam.Contains(corner)))[0];

    return part2A.x * 10000 + part2A.y;
}

static long part1(long[] input)
{
    long count = 0;
    for (int x = 0; x < 50; x++)
    {
        for (int y = 0; y < 50; y++)
        {
            var probe = new Computer("REPAIR", input.ToArray(), new long[] { x, y }.AsEnumerable().GetEnumerator(), false);
            probe.MoveNext();
            count += probe.Current;
        }
    }

    return count;
}