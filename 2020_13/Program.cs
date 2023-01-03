public class Program
{
    static void Main(string[] args)
    {
        var lines = File.ReadAllLines(args[0]);

        var arrived = int.Parse(lines[0]);
        var times = lines[1].Split(',').Where(str => str != "x").Select(int.Parse).ToArray();

        int earliest = int.MaxValue;
        int earliestBusNumber = -1;
        foreach (var time in times)
        {
            var next = (arrived / time) * time;
            if (next != arrived)
            {
                next += time;
            }

            if (next < earliest)
            {
                earliest = next;
                earliestBusNumber = time;
            }
        }

        Console.WriteLine($"Part1: {(earliest - arrived) * earliestBusNumber}");
    }
}