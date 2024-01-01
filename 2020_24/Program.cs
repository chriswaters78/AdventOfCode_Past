using System.Numerics;

var offsets = new List<(string prefix, Complex offset)>()
{
    ("ne", new Complex(0, 1)),
    ("e", new Complex(1, 0)),
    ("se", new Complex(1, -1)),
    ("sw", new Complex(0, -1)),
    ("w", new Complex(-1, 0)),
    ("nw", new Complex(-1, 1)),
};

var file = File.ReadAllLines("input.txt");
var instructions = file.Select(_ => new List<Complex>()).ToList();
foreach ((var line, var index) in file.Select((str,i) => (str,i)))
{
    var curr = line;
    while (curr.Length > 0)
    {
        var offset = offsets.Where(os => curr.StartsWith(os.prefix)).Single();
        instructions[index].Add(offset.offset);
        curr = curr[offset.prefix.Length..];
    }
}

var flipped = new HashSet<Complex>();
foreach (var instruction in instructions)
{
    var position = new Complex(0, 0);
    foreach (var move in instruction)
        position += move;

    if (flipped.Contains(position))
        flipped.Remove(position);
    else
        flipped.Add(position);
}


Console.WriteLine($"Part1: {flipped.Count}");

for (int days = 1; days <= 100; days++)
{
    //need to check one bigger than the max
    (int minX, int maxX) = ((int)flipped.Min(c => c.Real) - 1, (int)flipped.Max(c => c.Real) + 1);
    (int minY, int maxY) = ((int)flipped.Min(c => c.Imaginary) - 1, (int)flipped.Max(c => c.Imaginary) + 1);

    var newFlipped = new HashSet<Complex>();

    for (int x = minX; x <= maxX; x++)
    {
        for (int y = maxY; y >= minY; y--)
        {
            var curr = new Complex(x, y);
            var blackCount = offsets.Where(os => flipped.Contains(curr + os.offset)).Count();
            if (flipped.Contains(curr))
            {
                if (blackCount != 0 && blackCount <= 2)
                    newFlipped.Add(curr);
            }
            else if (blackCount == 2)
            {
                newFlipped.Add(curr);
            }
        }
    }
    flipped = newFlipped;
}

Console.WriteLine($"Part2: {flipped.Count}");