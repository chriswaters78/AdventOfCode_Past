class Program
{
    public static void Main(string[] args)
    {
        Dictionary<string, List<(int,string)>> adj = new Dictionary<string, List<(int,string)>>();

        foreach (var line in File.ReadAllLines("input.txt"))
        {
            var sp = line.Split(' ');
            var container = $"{sp[0]}-{sp[1]}";
            var list = new List<(int, string)>();

            var contains = line.Split("contain")[1].Split(',').Select(str => str.Split(' ')).ToArray();

            if (contains[0][1] != "no")
            {
                foreach (var contained in contains)
                {
                    list.Add((int.Parse(contained[1]), $"{contained[2]}-{contained[3]}"));
                }
            }

            adj[container] = list;
        }

        //DFS starting from each bag type
        int canContainGold = 0;

        foreach (var key in adj.Keys)
        {
            if (key == "shiny-gold")
            {
                continue;
            }

            Queue<string> toCheck = new Queue<string>();
            toCheck.Enqueue(key);
            while (toCheck.Count > 0)
            {
                var node = toCheck.Dequeue();
                if (node == "shiny-gold")
                {
                    canContainGold++;
                    break;
                }
                else
                {
                    foreach (var neighbour in adj[node])
                    {
                        toCheck.Enqueue(neighbour.Item2);
                    }
                }
            }
        }

        var part2 = countBags(adj, "shiny-gold");
        Console.WriteLine(canContainGold);
        
    }

    private static int countBags(Dictionary<string,List<(int,string)>> adj, string key)
    {
        int count = 0;
        var node = adj[key];
        foreach (var neighbour in adj[key])
        {
            count += neighbour.Item1 + neighbour.Item1 * countBags(adj, neighbour.Item2);
        }

        return count;
    }
}
