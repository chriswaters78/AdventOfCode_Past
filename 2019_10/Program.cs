var input = args.Length > 0 ? args[0] : "input";
var grid = File.ReadAllLines($"{input}.txt").SelectMany((line, y) => line.Select((ch, x) => (y, x, ch)))
    .Where(tp => tp.ch == '#')
    .ToDictionary(tp => (tp.y, tp.x), tp => tp.ch);

int maxY = grid.Keys.Max(pos => pos.y);
int maxX = grid.Keys.Max(pos => pos.x);

//directions
List<(int oy, int ox)> dirs = new List<(int oy, int ox)>();
for (int y = -maxY; y <= maxY; y++)
{
    for (int x = -maxX; x <= maxX; x++)
    {
        if (y != 0 || x != 0)
            dirs.Add((y, x));
    }
}

var visibledFrom = new Dictionary<(int y, int x), int>();
foreach (var ast in grid.Keys)
{
    var blocked = new HashSet<(int y, int x)>();
    var canSee = new HashSet<(int y, int x)>();
    foreach (var dir in dirs)
    {
        var current = ast;
        current = (current.y + dir.oy, current.x + dir.ox);
        bool seen = false;
        while (current.y <= maxY && current.y >= 0 && current.x >= 0 && current.x <= maxX)
        {
            if (grid.ContainsKey(current))
            {
                canSee.Add(current);
                if (seen)
                    blocked.Add(current);
                else
                    seen = true;
            }
            current = (current.y + dir.oy, current.x + dir.ox);
        }
    }
    visibledFrom[ast] = canSee.Except(blocked).Count();
}

var station = visibledFrom.OrderByDescending(kvp => kvp.Value).First();

Console.WriteLine($"Part 1: {station.Value}");

//find the angle to each other station
// we start looking up, -'ve y
foreach (var asteroid in grid.Keys.Where(pos => pos != station.Key))
{
    int opp;
    int adj;
    //tan goes from -infinity to +infinity from -90 to + 90 degrees
    //where 0 degrees is along the y axis
    if (asteroid.y < station.Key.y)
    {
        opp = asteroid.x - station.Key.x;
        adj = station.Key.y - asteroid.y;
    }
    else
    {
        opp = station.Key.x - asteroid.x;
        adj = asteroid.y - station.Key.y;
    }
    var angle = adj == 0 ? Math.Sign(opp) * 90 : Math.Tan(opp / adj);
}