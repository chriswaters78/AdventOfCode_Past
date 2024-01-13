using System.Text;

//initial state: ##.#..########..##..#..##.....##..###.####.###.##.###...###.##..#.##...#.#.#...###..###.###.#.#
//
//####. => #
var lines = File.ReadAllText("input.txt").Split($"{Environment.NewLine}{Environment.NewLine}");

var state = lines[0].Replace("initial state: ","").Select((ch,i) => (ch,i)).ToDictionary(tp => tp.i, tp => tp.ch);
var maps = lines[1].Split(Environment.NewLine).Select(line => line.Split(" => ")).Select(arr => (arr[0], arr[1][0])).ToArray();

Console.WriteLine($"*** Generation 0 ***");
Console.WriteLine(print(state));

for (int g = 1; g <= 20; g++)
{
    var min = state.Min(kvp => kvp.Key);
    var max = state.Max(kvp => kvp.Key);

    var newState = new Dictionary<int, char>();
    for (int c = min - 3; c <= max + 3; c++)
    {
        foreach (var map in maps)
        {
            bool matches = true;
            for (int i = -2; i < 3; i++)
            {
                if (map.Item1[i + 2] == '#' && (!state.ContainsKey(c + i) || state[c + i] == '.')
                    || map.Item1[i + 2] == '.' && state.ContainsKey(c + i) && state[c + i] == '#')
                {
                    //doesnt match
                    matches = false;
                    break;
                }
            }

            if (matches)
            {
                newState.Add(c, map.Item2);
                break;
            }
        }
    }
    state = newState;
    Console.WriteLine($"*** Generation {g} ***");
    
    Console.WriteLine($"Min: {min}, Max: {max}");
    Console.WriteLine(print(state));
    Console.WriteLine($"Score: {state.Where(tp => tp.Value == '#').Sum(tp => tp.Key)}");
}

var part1 = state.Where(tp => tp.Value == '#').Sum(tp => tp.Key);
Console.WriteLine($"Part1: {part1}");

//becomes stable and steadily increases 45 plants per generation after a while
var part2 = (50000000000L - 4988) * 45 + 224580L;
Console.WriteLine($"Part2: {part2}");

string print(Dictionary<int,char> map)
{
    var min = state.Min(kvp => kvp.Key);
    var max = state.Max(kvp => kvp.Key);
    var sb = new StringBuilder();
    for (int c = min; c <= max; c++)
    {
        sb.Append(state.ContainsKey(c) ? state[c] : '.');
    }
    return sb.ToString();
}