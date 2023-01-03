class Program
{
    public static void Main(string[] args)
    {
        var sp1 = File.ReadAllText("input.txt").Split($"{Environment.NewLine}{Environment.NewLine}");

        var myTicket = sp1[1].Split(Environment.NewLine)[1].Split(',').Select(int.Parse).ToArray();
        var nearbyTickets = sp1[2].Split(Environment.NewLine).Skip(1).Select(str => str.Split(',').Select(int.Parse).ToArray()).ToList();
        nearbyTickets.Add(myTicket);
  

        var filters = sp1[0].Split(Environment.NewLine).Select(str =>
        {
            var sp2 = str.Split(": ");
            var ranges = sp2[1].Split(" or ");
            var low = ranges[0].Split('-').Select(int.Parse).ToArray();
            var high = ranges[1].Split('-').Select(int.Parse).ToArray();

            return (sp2[0], new int[][] { new[] { low[0], low[1] }, new[] { high[0], high[1] } });
        }).ToArray<(string attr, int[][] conditions)>();

        int invalidCount = 0;
        var valid = new List<int[]>();
        foreach (var nearby in nearbyTickets)
        {
            bool invalid = false;
            foreach (var i in nearby)
            {
                if (filters.All(filter => filter.conditions.All(arr => i < arr[0] || i > arr[1])))
                {
                    invalidCount += i;
                    invalid = true;
                }
            }
            if (!invalid)
            {
                valid.Add(nearby);
            }
        }

        var validColumnsForAttribute = Enumerable.Range(0, filters.Length).ToDictionary(i => i, _ => new HashSet<int>());
        var solvedColumns = new Dictionary<int,int>();

        //for each attribute filter
        for (int ai = 0; ai < filters.Length; ai++)
        {
            //find the unique column that is valid for this set of filters
            for (int c = 0; c < valid.First().Length; c++)
            {

                if (valid.Select(arr => arr[c]).All(columnValue => filters[ai].conditions.Any(condition => columnValue >= condition[0] && columnValue <= condition[1])))
                {
                    validColumnsForAttribute[ai].Add(c);
                }
            }
        }

        while (validColumnsForAttribute.Any(kvp => kvp.Value.Count > 1))
        {
            var solvedKeys = validColumnsForAttribute.Where(kvp => kvp.Value.Count == 1).Select(kvp => (kvp.Key, kvp.Value.First())).ToArray();
            foreach ((int key, int column) in solvedKeys)
            {
                solvedColumns.Add(key, column);
                foreach (var attribute in validColumnsForAttribute.Keys.ToArray())
                {
                    if (validColumnsForAttribute[attribute].Contains(column))
                    {
                        validColumnsForAttribute[attribute].Remove(column);
                    }
                }
            }
        }

        var part2 = Enumerable.Range(0, 6).Aggregate(1L, (acc, i) => acc * myTicket[solvedColumns[i]]);

        Console.WriteLine($"Part1: {invalidCount}");
        Console.WriteLine($"Part2: {0}");
    }
}