using IntCode;

var memory = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();

Console.WriteLine($"*** START ***");
var computer = new Computer(memory, ConsoleNumericEnumerable.GetConsoleEnumerable().GetEnumerator());

while (computer.MoveNext())
{
}
Console.WriteLine(computer.Current);
Console.WriteLine($"*** STOP ***");

