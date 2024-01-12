var edges = File.ReadAllLines($"input.txt").Select(line => line.Split(" must be finished before step ")).Select(arr => (arr[0].Last(), arr[1].First()))
    .GroupBy(tp => tp.Item1).ToDictionary(grp => grp.Key, grp => grp.Select(tp => tp.Item2).ToHashSet());

var reverseEdges = edges.SelectMany(kvp => kvp.Value.Select(to => (kvp.Key, to))).GroupBy(tp => tp.to).ToDictionary(grp => grp.Key, grp => grp.Select(tp => tp.Key).ToHashSet());

void part2()
{
    var sorted = new List<char>();
    var noEdges = new SortedSet<char>(edges.Keys.ToList().Concat<char>(reverseEdges.Keys.ToList()).Distinct().Where(v => !reverseEdges.ContainsKey(v)));

    const int NOWORKERS = 5;
    const int SECONDSOFFSET = 60;
    var workers = new (char n, int finishedBy)?[NOWORKERS];

    int seconds = 0;
    while (true)
    {
        foreach (var i in workers.Select((tp, i) => (tp, i)).Where(a => a.tp?.finishedBy == seconds).Select(a => a.i))
        {
            (var n, var finishedBy) = workers[i].Value;
            workers[i] = null;
            sorted.Add(n);
            if (edges.ContainsKey(n))
            {
                foreach (var v2 in edges[n].ToList())
                {
                    edges[n].Remove(v2);
                    reverseEdges[v2].Remove(n);

                    if (reverseEdges[v2].Count == 0)
                    {
                        noEdges.Add(v2);
                    }
                }
            }
        }

        while (workers.Any(tp => !tp.HasValue) && noEdges.Any())
        {
            var available = noEdges.First();
            noEdges.Remove(available);
            var index = workers.Select((tp, i) => (tp, i)).Where(a => !a.tp.HasValue).Select(a => a.i).First();
            workers[index] = (available, SECONDSOFFSET + seconds + available - 'A' + 1);
        }

        if (!workers.Any(a => a.HasValue))
        {
            Console.WriteLine($"Part2: {seconds}");
            break;
        }

        seconds = workers.Min(tp => tp?.finishedBy ?? int.MaxValue);
    }
}


void part1()
{
    var sorted = new List<char>();
    var noEdges = new SortedSet<char>(edges.Keys.ToList().Concat<char>(reverseEdges.Keys.ToList()).Distinct().Where(v => !reverseEdges.ContainsKey(v)));

    while (noEdges.Count > 0)
    {
        var n = noEdges.First();
        noEdges.Remove(n);

        sorted.Add(n);
        if (edges.ContainsKey(n))
        {
            foreach (var v2 in edges[n].ToList())
            {
                edges[n].Remove(v2);
                reverseEdges[v2].Remove(n);

                if (reverseEdges[v2].Count == 0)
                {
                    noEdges.Add(v2);
                }
            }
        }
    }

    Console.WriteLine($"Part1: {String.Join("", sorted)}");
}
