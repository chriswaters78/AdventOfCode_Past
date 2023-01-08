using IntCode;
using System.Collections;
using System.Reflection;
using System.Text;

var input = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();

Console.WriteLine($"*** START ***");
Console.WriteLine($"Part 1: {part1(input)}");
Console.WriteLine($"Part 2: {part2(input)}");

Console.WriteLine($"*** STOP ***");

static long part2(long[] input)
{
    long count = 0;
    HashSet<(int x, int y)> tractor = new HashSet<(int x, int y)>();
    for (int x = 0; x < 50; x++)
    {
        for (int y = 0; y < 50; y++)
        {
            var probe = new Computer("REPAIR", input.ToArray(), new long[] { x, y }.AsEnumerable().GetEnumerator(), false);
            probe.MoveNext();
            if (probe.Current == 1)
            {
                tractor.Add((x, y));
            }
            count += probe.Current;
        }
    }

    Console.WriteLine(Printer.PrintGridMap(tractor.ToDictionary(key => key, _ => '#')));
    return count;
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