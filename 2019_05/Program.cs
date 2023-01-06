using IntCode;

var memory = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();

Console.WriteLine($"*** START ***");
var computer = new Computer(memory, ConsoleEnumerable.GetConsoleEnumerable());
Console.WriteLine(computer.Run().Last());
Console.WriteLine($"*** STOP ***");

