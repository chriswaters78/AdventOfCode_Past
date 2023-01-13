using System;
using System.Diagnostics;

public class Program
{
    public static void Main(string[] args)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        var input = args.Length > 0 ? args[0] : "input";
        
        var grid = File.ReadAllLines($"{input}.txt").SelectMany((line, y) => line.Select((ch, x) => (y, x, ch)))
            .Where(tp => tp.ch == '#')
            .ToDictionary(tp => (tp.y, tp.x), tp => tp.ch);

        int maxY = grid.Keys.Max(pos => pos.y);
        int maxX = grid.Keys.Max(pos => pos.x);

        //directions
        var dirs = new List<(int oy, int ox)>();
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

        Console.WriteLine($"Part 1: {station.Value} in {watch.ElapsedMilliseconds}ms");
        watch.Restart();

        //find the angle to each other station
        //measured clockwise from the -'ve y-axis
        var sortedAsteroids = grid.Keys
            .Where(key => key != station.Key)
            .Select(asteroid => new {
                 Angle = pseudoAngle(asteroid.x - station.Key.x, asteroid.y - station.Key.y),
                 Mag = Math.Pow(asteroid.x - station.Key.x, 2) + Math.Pow(asteroid.y - station.Key.y, 2),
                 Pos = (asteroid.x, asteroid.y)
             })
            .OrderBy(a => a.Angle)
            .ThenBy(a => a.Mag);

        var allAngles = sortedAsteroids.Select(a => a.Angle).Distinct();
        var byAngle = sortedAsteroids.GroupBy(a => a.Angle)
                .ToDictionary(grp => grp.Key, grp => grp.Select(a => a.Pos).ToList());

        int removed = 0;
        while (true)
        {
            foreach (var angle in allAngles)
            {
                if (byAngle[angle].Any())
                {
                    removed++;

                    if (removed == 200)
                    {
                        var lastRemoved = byAngle[angle][0];
                        Console.WriteLine($"Part 2: {lastRemoved.x * 100 + lastRemoved.y} in  {watch.ElapsedMilliseconds}ms");
                        return;
                    }
                    byAngle[angle].RemoveAt(0);
                }
            }
        }
    }

    static double pseudoAngle(double dx, double dy)
    {
        bool diag = dy > dx;
        bool adiag = dy > -dx;

        double r = !adiag ? 4 : 0;

        if (dx == 0)
            return (12 - r) % 8;

        if (diag ^ adiag)
            r += 2 - dy / dx;
        else
            r += dx / dy;

        return (12 - r) % 8;
    }
}
