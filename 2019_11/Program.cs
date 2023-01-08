using IntCode;
using System.Collections;
using System.Text;

var input = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();

Console.WriteLine($"*** START ***");
Console.WriteLine($"Part 1: {part1(input)}");
Console.WriteLine($"*** STOP ***");

static long part1(long[] input)
{
    var value = new ValueEnumerator();
    var paint = new Computer("PAINT", input.ToArray(), value);

    HashSet<(int x, int y)> painted = new HashSet<(int x, int y)>();
    var hull = new Dictionary<(int x, int y), long>();
    (int x, int y) pos = (0, 0);
    var dir = 0;
    (int x, int y)[] dirs = new[] { (0, 1), (1, 0), (0, -1), (-1, 0)};
    bool cont = true;

    value.Value = 1;
    while (cont)
    {
        paint.MoveNext();
        var color = paint.Current;
        hull[pos] = color;
        painted.Add(pos);
        cont = paint.MoveNext();
        var turn = paint.Current;
        dir += turn == 0 ? 3 : 1;
        dir %= 4;
        pos = (pos.x + dirs[dir].x, pos.y + dirs[dir].y);
        hull.TryAdd(pos, 0);
        value.Value = hull[pos];
    }

    Console.WriteLine(print(hull));
    return painted.Count;
}

static string print(Dictionary<(int x, int y), long> hull)
{
    var minX = hull.Keys.Min(tp => tp.x);
    var maxX = hull.Keys.Max(tp => tp.x);
    var minY = hull.Keys.Min(tp => tp.y);
    var maxY = hull.Keys.Max(tp => tp.y);

    List<StringBuilder> sbs = new List<StringBuilder>();
    for (int y = minY; y <= maxY; y++)
    {
        sbs.Add(new StringBuilder((String.Join("", Enumerable.Repeat(' ', maxX - minX + 1)))));
    }

    foreach (var kvp in hull)
    {
        var y = kvp.Key.y - minY;
        sbs[y][kvp.Key.x - minX] = kvp.Value == 1 ? '#' : '.';
    }

    sbs.Reverse();
    return String.Join(Environment.NewLine, sbs);
}
