var input = File.ReadAllLines("input.txt").Select(line => line.Split(",").Select(str => (str[0], int.Parse(str[1..]))).ToArray()).ToArray();

var p1s = getPositions(input[0]);
var p2s = getPositions(input[1]);

var crosses = p1s.Select(tp => tp.position).Intersect(p2s.Select(tp => tp.position)).ToList();

var closest = crosses.Min(tp => Math.Abs(tp.x) + Math.Abs(tp.y));
Console.WriteLine($"Part 1: {closest}");

var steps1 = p1s.GroupBy(tp => tp.position).ToDictionary(grp => grp.Key, grp => grp.Min(tp => tp.steps));
var steps2 = p2s.GroupBy(tp => tp.position).ToDictionary(grp => grp.Key, grp => grp.Min(tp => tp.steps));

var closestSteps = crosses.Min(pos => steps1[pos] + steps2[pos]);
Console.WriteLine($"Part 2: {closestSteps}");

static List<(int steps, (int x,int y) position)> getPositions((char, int)[] moves)
{
    var positions = new List<(int steps, (int x, int y))>();
    var (steps, (x,y)) = (0, (0, 0));
    foreach (var move in moves)
    {
        var offset = (0, 0);
        switch (move.Item1)
        {
            case 'U':
                offset = (0, 1);
                break;
            case 'D':
                offset = (0, -1);
                break;
            case 'L':
                offset = (-1, 0);
                break;
            case 'R':
                offset = (1, 0);
                break;
        }
        for (int i = 1; i <= move.Item2; i++)
        {
            steps++;
            (x, y) = (x + offset.Item1, y + offset.Item2);
            positions.Add((steps, (x, y)));
        }
    }

    return positions;
}
