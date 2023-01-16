using Rationals;

var grid = File.ReadAllLines($"{args[0]}.txt").SelectMany((line, y) => line.Select((ch, x) => (y, x, ch)))
    .Where(tp => tp.ch == '#')
    .ToDictionary(tp => (tp.y, tp.x), tp => tp.ch);

var asteroidMap = grid.Keys.ToDictionary(pos => pos, candidate => 
        grid.Keys
            .Where(key => key != candidate)
            .Select(asteroid => new {
                Angle = pseudoAngle(asteroid.x - candidate.x, asteroid.y - candidate.y),
                Maghattan = asteroid.x - candidate.x + asteroid.y - candidate.y,
                Pos = (asteroid.x, asteroid.y)
            })
            .OrderBy(a => a.Angle).ThenBy(a => a.Maghattan)
            .GroupBy(a => a.Angle)
            .ToDictionary(grp => grp.Key, grp => new Queue<(int x, int y)>(grp.Select(a => a.Pos))));

var station = asteroidMap.OrderByDescending(kvp => kvp.Value.Count).First().Key;
Console.WriteLine($"Part 1: {asteroidMap[station].Count}");

int removed = 0;
while (true)
{
    foreach (var angle in asteroidMap[station].Keys)
    {
        if (asteroidMap[station][angle].TryDequeue(out (int x, int y) lastRemoved))
        {
            removed++;
            if (removed == 200)
            {
                Console.WriteLine($"Part 2: {lastRemoved.x * 100 + lastRemoved.y}");
                return;
            }
        }
    }
}

static Rational pseudoAngle(int dx, int dy)
{
    bool diag = dy > dx;
    bool adiag = dy > -dx;

    int r = !adiag ? 4 : 0;

    if (dx == 0)
        return (12 - r) % 8;

    Rational rr = r;
    if (diag ^ adiag)
        rr += 2 - new Rational(dy, dx);
    else
        rr += new Rational(dx, dy);

    rr = 12 - rr;
    if (rr >= 8)
        rr -= 8;
    return rr;
}
