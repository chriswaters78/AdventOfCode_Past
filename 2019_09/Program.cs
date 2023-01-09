using IntCode;
using System.Collections;

var test1 = new long[] { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99 };
var test2 = new long[] { 1102, 34915192, 34915192, 7, 4, 7, 99, 0 };
var test3 = new long[] { 104, 1125899906842624, 99 };

var input = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();

Console.WriteLine($"*** START ***");
Console.WriteLine($"Part 1: {part1(input)}");
Console.WriteLine($"*** STOP ***");

static long part1(long[] input)
{
    var boost = new Computer("BOOST", input.ToArray(), ConsoleNumericEnumerable.GetConsoleEnumerable().GetEnumerator());
    while (boost.MoveNext())
    {
    }
    return boost.Current;
}
