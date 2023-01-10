using IntCode;
using System.Diagnostics;

Stopwatch watch = new Stopwatch();
watch.Start();

var input = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();

var commands = new CommandEnumerable(new[] {
        "north", "north", "take space heater",
        "east", "take semiconductor",
        "west", "south", "south", "east", "take ornament",
        "south", "take festive hat",
        "north", "west", "west", "north", "north", "north", "west",});

var advent = new Computer("ADVENTURE OF INTCODE", input.ToArray(), 
    args.Any() && args[0] == "solve" 
    ? commands.GetEnumerator() 
    : ConsoleStringEnumerable.GetConsoleEnumerable().GetEnumerator());

while (advent.MoveNext())
{
    Console.Write((char) advent.Current);
}

Console.WriteLine($"Computer {advent.Name} halted after {advent.Steps} steps. Runtime {watch.ElapsedMilliseconds}ms");