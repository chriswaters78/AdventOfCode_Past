//position=<-20620, -41485> velocity=< 2,  4>
var points = File.ReadAllLines("test.txt").Select(line => line.Replace("position=<", "").Trim().Replace(",", "").Replace("> velocity=<", " ").Replace("  ", " ").Replace(">", "").Split(" ").Select(int.Parse).ToArray()).ToArray();

var T = (points[0][0] - points[1][0]) / (points[1][2] - points[0][2]);

for (int t = T - 2; t <= T + 2; t++)
{
    HashSet<(int r, int c)> pointsAtt = points.Select(p => (p[1] + t * p[3], p[0] + t * p[2])).ToHashSet();
    (var minC, var maxC) = (pointsAtt.Min(a => a.c), pointsAtt.Max(a => a.c));
    (var minR, var maxR) = (pointsAtt.Min(a => a.r), pointsAtt.Max(a => a.r));

    Console.WriteLine($"After {t}:");

    for (int r = minR; r <= maxR; r++)
    {
        for (int c = minC; c <= maxC; c++)
        {
            if (pointsAtt.Contains((r, c)))
            {
                Console.Write('#');
            }
            else
            {
                Console.Write(' ');
            }
        }
        Console.WriteLine();
    }
}