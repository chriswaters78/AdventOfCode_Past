var input = File.ReadAllLines("input.txt").Select(line => line.ToArray()).ToArray();

uint state = 0;
for (int r = 0; r < 5; r++)
{
    for (int c = 0; c < 5; c++)
    {
        if (input[r][c] == '#')
        {
            state = SetBit(r * 5 + c, state);
        }
    }
}

HashSet<uint> seen = new HashSet<uint>();
var offsets =
    (from r in Enumerable.Range(0, 5)
     from c in Enumerable.Range(0, 5)
     select (r, c))
    .ToDictionary(key => key.r * 5 + key.c,
    key => new[] { (key.r - 1, key.c), (key.r + 1, key.c), (key.r, key.c - 1), (key.r, key.c + 1) }
        .Where(key => key.Item1 >= 0 && key.Item1 < 5 && key.Item2 >= 0 && key.Item2 < 5)
        .Select(key => key.Item1 * 5 + key.Item2).ToArray());
do
{
    seen.Add(state);
    uint newState = 0;
    for (int bit = 0; bit < 25; bit++)
    {
        var count = offsets[bit].Count(b => IsBitSet(b, state));
        if (IsBitSet(bit, state))
        {
            if (count == 1)
            {
                newState = SetBit(bit, newState);
            }
        }
        else
        {
            if (count == 1 || count == 2)
            {
                newState = SetBit(bit, newState);
            }
        }
    }
    state = newState;
}
while (!seen.Contains(state));

Console.WriteLine($"Part 1: {state}");

static bool IsBitSet(int bit, uint state) => (state & (1U << bit)) != 0;
static uint SetBit(int bit, uint state) => state |= 1U << bit;
static uint ClearBit(int bit, uint state) => state &= ~(1U << bit);
