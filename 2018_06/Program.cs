//if a point is within a a diagonal quadrant from the point:
//            B   #E
//             # #
//              P
//then B would block P upwards, E wouldn't as there will always be a point directly above P that is closer to P than E

//so for all our points find any that are blocked in all directions
List<(int r, int c)> points = File.ReadAllLines("input.txt").Select(line => line.Split(", ")).Select(arr => (int.Parse(arr[1]), int.Parse(arr[0]))).ToList();

(var minR, var maxR) = (points.Min(p => p.r), points.Max(p => p.r));
(var minC, var maxC) = (points.Min(p => p.c), points.Max(p => p.c));

int best = -1;
foreach (var point in points)
{
    var upwards = points.Where(p => Math.Abs(p.c - point.c) <= Math.Abs(p.r - point.r) && p.r < point.r).ToList();
    var downwards = points.Where(p => Math.Abs(p.c - point.c) <= Math.Abs(p.r - point.r) && p.r > point.r).ToList();
    var leftwards = points.Where(p => Math.Abs(p.r - point.r) <= Math.Abs(p.c - point.c) && p.c < point.c).ToList();
    var righwards = points.Where(p => Math.Abs(p.r - point.r) <= Math.Abs(p.c - point.c) && p.c > point.c).ToList();

    if (upwards.Any() && downwards.Any() && leftwards.Any() && righwards.Any())
    {
        var closest = new List<(int r, int c)>();
        //this is not an infinite point
        for (int r = minR; r <= maxR; r++)
        {
            for (int c = minC; c <= maxC; c++)
            {
                var distFromPoint = Math.Abs(point.r - r) + Math.Abs(point.c - c);
                var distanceFromOtherPoints = points.Where(p => p != point).Select(p => (p, Math.Abs(p.r - r) + Math.Abs(p.c - c))).ToList();
                if (distanceFromOtherPoints.All(tp => tp.Item2 > distFromPoint))
                {
                    closest.Add((r, c));
                }
            }
        }
        if (closest.Count > best)
        {
            best = closest.Count;
        }
    }
}

Console.WriteLine($"Part1: {best}");

int part2 = 0;
const int LIMIT = 10000;
for (int r = minR; r <= maxR; r++)
{
    for (int c = minC; c <= maxC; c++)
    {
        var totalDistance = points.Sum(p => Math.Abs(p.r - r) + Math.Abs(p.c - c));
        if (totalDistance < LIMIT)
        {
            part2++;
        }
    }
}

Console.WriteLine($"Part2: {part2}");
