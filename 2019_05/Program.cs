using IntCode;

var memory = File.ReadAllText("input.txt").Split(",").Select(int.Parse).ToArray();

var computer = new Computer(memory);
computer.Run();


