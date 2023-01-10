using IntCode;

var input = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();

foreach (var ch in input)
{
    if ((char) ch == 10)
    {
        Console.WriteLine();
    }
    else
    {
        Console.Write((char) ch);
    }
}

//carry these to complete
//ornament
//space heater
//festive hat
//semiconductor

var advent = new Computer("ADVENT", input.ToArray(), ConsoleStringEnumerable.GetConsoleEnumerable().GetEnumerator(), false);
while (advent.MoveNext())
{
    Console.Write((char)advent.Current);
}
