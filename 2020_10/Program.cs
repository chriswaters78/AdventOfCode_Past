class Program
{
    static Dictionary<int, long> pathCount = new Dictionary<int, long>();
    public static void Main(string[] args)
    {
        var adapters = File.ReadAllLines("input.txt").Select(int.Parse).OrderBy(v => v).ToArray();
        pathCount[adapters.Last()] = 1;

        var part2 = pathsToDevice(adapters, 0);
        Console.WriteLine($"Part1 {part1(adapters)}");
        Console.WriteLine($"Part2 {part1(adapters)}");
    }

    static long pathsToDevice(int[] remainingAdapters, int voltage)
    {
        if (pathCount.ContainsKey(voltage))
        {
            return pathCount[voltage];
        }

        if (!remainingAdapters.Any())
        {
            pathCount[voltage] = 0L;
            return 0L;
        }

        var paths = remainingAdapters
                        .TakeWhile(adapter => adapter <= voltage + 3)
                        .Select(adapter => 
                            pathsToDevice(
                                remainingAdapters.Where(a => a > adapter).ToArray(), 
                                adapter))
                        .ToArray();

        var count = paths.Sum();
        pathCount[voltage] = count;
        return count;
    }

    static int part1(int[] adapters)
    {
        Dictionary<int, int> diffCount = new Dictionary<int, int>();

        int voltage = 0;
        diffCount[3] = 1;
        foreach (var adapter in adapters)
        {
            var diff = adapter - voltage;
            if (!diffCount.ContainsKey(diff))
            {
                diffCount[diff] = 0;
            }
            diffCount[diff]++;
            voltage = adapter;
        }
        voltage += 3;

        return diffCount[1] * diffCount[3];

    }

}