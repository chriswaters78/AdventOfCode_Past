using IntCode;

var input = File.ReadAllText("input.txt").Split(",").Select(int.Parse).ToArray();
//var test = new[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 };

var output = int.MinValue;
for (int p1 = 0; p1 < 5; p1++)
{
    for (int p2 = 0; p2 < 5; p2++)
    {
        for (int p3 = 0; p3 < 5; p3++)
        {
            for (int p4 = 0; p4 < 5; p4++)
            {
                for (int p5 = 0; p5 < 5; p5++)
                {
                    if (new[] { p1, p2, p3, p4, p5 }.Distinct().Count() != 5)
                    {
                        continue;
                    }
                    var amp1 = new Computer(input.ToArray(), new [] { p1, 0 }).Run().Last();
                    var amp2 = new Computer(input.ToArray(), new [] { p2, amp1 }).Run().Last();
                    var amp3 = new Computer(input.ToArray(), new [] { p3, amp2 }).Run().Last();
                    var amp4 = new Computer(input.ToArray(), new [] { p4, amp3}).Run().Last();
                    var amp5 = new Computer(input.ToArray(), new [] { p5, amp4 }).Run().Last();

                    output = Math.Max(output, amp5);
                }
            }
        }
    }
}

Console.WriteLine($"Part 1: {output}");