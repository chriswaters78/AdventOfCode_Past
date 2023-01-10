using IntCode;

const char NL = (char)10;

var input = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();

var instructions1 = String.Join(NL.ToString(), new[] {
    "NOT A J",
    "NOT C T",
    "AND D T",
    "OR T J",
    "WALK"})
    .Append(NL);

var spring1 = new Computer("SPRINGBOT1", input.ToArray(), instructions1.Select(ch => (long) ch).GetEnumerator(), false);
while (spring1.MoveNext())
{
    Console.Write((char)spring1.Current);
}

Console.WriteLine($"Part 1: {spring1.Current} in {spring1.Steps} steps");

//Patterns to cross
//XOX
//XOOOX
//XOXOOX
//XOOOXXOX
//XOXOXOOX
//XOOXXOXXOX

var instructions2 = String.Join(NL.ToString(), new[] {
    "NOT C J", //J = !C
    "AND H J", //J = !C && H 
    "NOT A T", //T = !A
    "OR T J",  //J = !A || (!C && H)
    "NOT B T", //T = !B
    "OR T J",  //J = !B || !A || (!C && H)
    "AND D J", //J = D & (!B || !A || (!C && H))
    "RUN"})
    .Append(NL);

var spring2 = new Computer("SPRINGBOT2", input.ToArray(), instructions2.Select(ch => (long)ch).GetEnumerator(), false);
while (spring2.MoveNext())
{
    Console.Write((char)spring2.Current);
}
Console.Write($"Part 2: {spring2.Current} in {spring2.Steps} steps");


