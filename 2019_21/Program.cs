using IntCode;

var input = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();

const char NL = (char)10;

var instructions1 = String.Join(NL.ToString(), new[] {
    "NOT D T",
    "NOT T T",
    "NOT C J",
    "AND T J",
    "NOT A T",
    "OR T J",
    "WALK"})
    .Append(NL);

//var interactive = ConsoleStringEnumerable.GetConsoleEnumerable().GetEnumerator();
var spring1 = new Computer("SPRINGBOT", input.ToArray(), instructions1.Select(ch => (long) ch).GetEnumerator(), false);

while (spring1.MoveNext())
{
}

var instructions2 = String.Join(NL.ToString(), new[] {
    "NOT D T",
    "NOT T T",
    "NOT C J",
    "AND T J",
    "NOT A T",
    "OR T J",
    "RUN"})
    .Append(NL);

//var interactive = ConsoleStringEnumerable.GetConsoleEnumerable().GetEnumerator();
var spring2 = new Computer("SPRINGBOT", input.ToArray(), instructions2.Select(ch => (long)ch).GetEnumerator(), false);
while (spring2.MoveNext())
{
    Console.Write((char)spring2.Current);
}

Console.Write(spring2.Current);
Console.WriteLine($"*** STOP ***");


