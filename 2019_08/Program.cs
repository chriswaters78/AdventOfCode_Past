const int WIDTH = 25;
const int HEIGHT = 6;

var layers = File.ReadAllText("input.txt")
    .Select((ch, i) => (ch, i))
    .GroupBy(tp => tp.i / (WIDTH * HEIGHT))
    .Select(grp => grp.GroupBy(tp => tp.ch).ToDictionary(grp2 => grp2.Key, grp2 => grp2.Count())).ToArray();

var fewest0s = layers.Select((dir, i) => (dir, i)).OrderBy(tp => tp.dir['0']).First().i;
var part1 = layers[fewest0s]['1'] * layers[fewest0s]['2'];
Console.WriteLine($"Part 1: {part1}");

char[][,] grid = new char[100][,];
char[,] message = new char[6, 25];
var input2 = File.ReadAllText("input.txt").GetEnumerator();
for (int l = 0; l < 100; l++)
{
    grid[l] = new char[6, 25];
    for (int r = 0; r < HEIGHT; r++)
    {
        for (int c = 0; c < WIDTH; c++)
        {
            input2.MoveNext();
            grid[l][r, c] = input2.Current;
        }
    }
}

for (int r = 0; r < HEIGHT; r++)
{
    for (int c = 0; c < WIDTH; c++)
    {
        var nonBlank = Enumerable.Range(0, layers.Length).Select(i => grid[i][r, c]).Where(ch => ch != '2').First();
        message[r, c] = nonBlank;
    }
}

for (int r = 0; r < HEIGHT; r++)
{
    for (int c = 0; c < WIDTH; c++)
    {
        switch (message[r, c])
        {
            case '1':
                Console.Write("#");
                break;
            default: Console.Write(" ");
                break;
        }
    }
    Console.WriteLine();
}
