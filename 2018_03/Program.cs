var inputs = File.ReadAllLines("test.txt").Select(str =>
    {
        var (n, rest) = (str.Split(" @ ")[0].Trim('#'), str.Split(" @ ")[1]);
        var (xy, wh) = (rest.Split(": ")[0], rest.Split(": ")[1]);  
        var (x, y) = (xy.Split(",")[0], xy.Split(",")[1]);
        var (w, h) = (wh.Split("x")[0], wh.Split("x")[1]);
        return new { 
            No = int.Parse(n), 
            x = int.Parse(x),
            y = int.Parse(y), 
            w = int.Parse(w),
            h = int.Parse(h) 
        };
    });

var results = (from x in Enumerable.Range(0,1000)
              from y in Enumerable.Range(0, 1000)
              select (x, y))
              .ToDictionary(tp => tp, tp => 0);

foreach (var input in inputs)
{
    for (int x = input.x; x < input.x + input.w; x++)
    {
        for (int y = input.y; y < input.y + input.h; y++)
        {
            results[(x, y)]++;
        }
    }
}

var part1 = results.Values.Count(v => v > 1);
Console.WriteLine($"Part 1: {part1}");

foreach (var input in inputs)
{
    var doesntOverlap = true;
    for (int x = input.x; x < input.x + input.w; x++)
    {
        for (int y = input.y; y < input.y + input.h; y++)
        {
            if (results[(x, y)] > 1)
            {
                doesntOverlap = false;
            }
        }
    }
    if (doesntOverlap)
    {
        Console.WriteLine($"Part 2: {input.No}");
    }
}
Console.WriteLine($"Finished");
