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

var offsets =
    (from r in Enumerable.Range(0, 5)
     from c in Enumerable.Range(0, 5)
     select (r, c))
    .ToDictionary(key => key.r * 5 + key.c,
    key => new[] { (key.r - 1, key.c), (key.r + 1, key.c), (key.r, key.c - 1), (key.r, key.c + 1) }
        .Where(key => key.Item1 >= 0 && key.Item1 < 5 && key.Item2 >= 0 && key.Item2 < 5
                && !(key.Item1 == 2 && key.Item2 == 2))
        .Select(key => (key.Item1 * 5 + key.Item2, 0)).ToList());

//for recursive, these are modified
offsets[0].Add((11, -1));
offsets[0].Add((7, -1));
offsets[1].Add((7, -1));
offsets[2].Add((7, -1));
offsets[3].Add((7, -1));
offsets[4].Add((7, -1));
offsets[4].Add((13, -1));

offsets[5].Add((11, -1));
offsets[10].Add((11, -1));
offsets[15].Add((11, -1));
offsets[9].Add((13, -1));
offsets[14].Add((13, -1));
offsets[19].Add((13, -1));

offsets[20].Add((11, -1));
offsets[20].Add((17, -1));
offsets[21].Add((17, -1));
offsets[22].Add((17, -1));
offsets[23].Add((17, -1));
offsets[24].Add((17, -1));
offsets[24].Add((13, -1));

offsets[7].AddRange(new[] { (0, 1), (1, 1), (2, 1), (3, 1), (4, 1) });
offsets[17].AddRange(new[] { (20, 1), (21, 1), (22, 1), (23, 1), (24, 1) });

offsets[11].AddRange(new[] { (0, 1), (5, 1), (10, 1), (15, 1), (20, 1) });
offsets[13].AddRange(new[] { (4, 1), (9, 1), (14, 1), (19, 1), (24, 1) });

var states = new Dictionary<int, uint>();
states[0] = state;

for (int steps = 0; steps < 200; steps++)
{
    //Console.WriteLine($"Step {steps}");
    //print(states);
    //seen.Add(state);
    var minLevel = states.Keys.Min();
    var maxLevel = states.Keys.Max();

    states[minLevel - 1] = 0;
    states[maxLevel + 1] = 0;
    states[minLevel - 2] = 0;
    states[maxLevel + 2] = 0;
    var newStates = states.ToDictionary(kvp => kvp.Key, _ => (uint) 0);
    for (int level = minLevel - 1; level <= maxLevel + 1; level++)
    {
        for (int bit = 0; bit < 25; bit++)
        {
            if (bit == 12)
            {
                continue;
            }

            var count = offsets[bit].Count(os => IsBitSet(os.Item1, states[level + os.Item2]));
            if (IsBitSet(bit, states[level]))
            {
                if (count == 1)
                {
                    newStates[level] = SetBit(bit, newStates[level]);
                }
            }
            else
            {
                if (count == 1 || count == 2)
                {
                    newStates[level] = SetBit(bit, newStates[level]);
                }
            }
        }
    }
    states = newStates;
}
//while (!seen.Contains(state));

Console.WriteLine($"Part 1: {state}");

var part2 = 0;
foreach (var level in states.Keys)
{
    for (int bit = 0; bit < 25; bit++)
    {
        if (bit == 12)
        {
            continue;
        }
        if (IsBitSet(bit, states[level]))
        {
            part2++;
        }
    }
}

//Console.WriteLine($"Final state:");
//print(states);
Console.WriteLine($"Part 2: {part2}");

static bool IsBitSet(int bit, uint state) => (state & (1U << bit)) != 0;
static uint SetBit(int bit, uint state) => state |= 1U << bit;
static uint ClearBit(int bit, uint state) => state &= ~(1U << bit);

static void print(Dictionary<int, uint> states)
{
    foreach (var level in states.Keys.OrderBy(key => key))
    {
        if (states[level] == 0)
        {
            continue;
        }
        Console.WriteLine();
        Console.WriteLine($"Level {level}:");
        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 5; c++)
            {
                var bit = r * 5 + c;
                if (bit == 12)
                {
                    Console.Write('?');
                }
                else
                {
                    Console.Write(IsBitSet(bit, states[level]) ? '#' : '.');
                }
            }
            Console.WriteLine();
        }
    }
}