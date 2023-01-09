using IntCode;
using System.Collections;
using System.Reflection;
using System.Text;

var input = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();

Console.WriteLine($"*** START ***");
Console.WriteLine($"Part 2: {part2(input)}");

Console.WriteLine($"*** STOP ***");

static long part2(long[] input)
{
    var movement = "A,B,B,A,C,A,C,A,C,B";
    var a = "L,6,R,12,R,8";
    var b = "R,8,R,12,L,12";
    var c = "R,12,L,12,L,4,L,4";

    input[0] = 2;
    var console = movement.Append((char)10).Concat(a).Append((char)10).Concat(b).Append((char)10).Concat(c).Append((char)10).Append('n').Append((char)10);
    var vacuum = new Computer("REPAIR", input.ToArray(), console.Select(ch => (long) ch).GetEnumerator(), false);

    while (vacuum.MoveNext())
    {
        Console.Write((char)vacuum.Current);
    }

    return vacuum.Current;
}

static long part1(long[] input)
{
    var vacuum = new Computer("REPAIR", input.ToArray(), null, false);

    Dictionary<(int r, int c), char> hull = new Dictionary<(int r, int c), char>();
    (int r, int c) pos = (0, 0);
    while (vacuum.MoveNext())
    {
        if (vacuum.Current == 10)
        {
            pos = (pos.r + 1, 0);
        }
        else
        {
            hull[pos] = (char)vacuum.Current;
            pos = (pos.r, pos.c + 1);
        }

        Console.Write((char)vacuum.Current);
    }

    //find all intersections
    var offsets = new (int r, int c)[] { (0, 0), (0, 1), (-1, 0), (0, -1), (1, 0) };
    int count = hull.Keys.Where(kvp => offsets.Select(os => (kvp.r + os.r, kvp.c + os.c)).All(point => hull.ContainsKey(point) && hull[point] == '#'))
        .Sum(point => point.r * point.c);

    Printer.PrintGridMap(hull);

    return count;
}