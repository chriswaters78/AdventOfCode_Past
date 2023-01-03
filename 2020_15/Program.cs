class Program
{
    static int[] example = new[] { 2, 0, 1, 7, 4, 14, 18 };
    static Dictionary<int, int> lastSeen = new Dictionary<int, int>();
    static Dictionary<int, int> lastButOneSeen = new Dictionary<int, int>();
    static void Main(string[] args)
    {
        int round = 1;
        foreach (var i in example.Take(example.Length - 1))
        {
            lastSeen[i] = round;
            Console.WriteLine(i);
            round++;
        }
        var last = example.Last();

        for (round = round; round < 30000000; round++)
        {
            //Console.WriteLine(last);
            if (!lastSeen.ContainsKey(last))
            {
                lastSeen[last] = round;
                last = 0;
            }
            else
            {
                lastButOneSeen[last] = lastSeen[last];
                lastSeen[last] = round;
                last = lastSeen[last] - lastButOneSeen[last];
            }
        }

        Console.WriteLine(last);
    }
}
