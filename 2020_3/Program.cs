var map = File.ReadAllLines("input.txt").ToArray();

List<long> results = new List<long>();
var slopes = new[] { (3, 1), (1, 1), (5, 1), (7, 1), (1, 2) };
foreach (var slope in slopes)
{
    (int x, int y) = (slope.Item1, slope.Item2);
    int trees = 0;
    while (y < map.Length)
    {
        if (map[y][x] == '#')
        {
            trees++;
        }
        (x, y) = ((x + slope.Item1) % map.First().Length, y + slope.Item2);
    }

    results.Add(trees);
}

var part1 = results[0];
var part2 = results.Aggregate(1L, (acc, t) => acc * t);

Console.WriteLine($"{part1}, {part2}");