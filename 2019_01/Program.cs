var part1 = File.ReadAllLines("input.txt").Select(int.Parse).Select(i => i / 3 - 2).Sum();
Console.WriteLine(part1);

var test1 = getFuel(12);
var test2 = getFuel(14);
var test3 = getFuel(1969);

var part2 = File.ReadAllLines("input.txt").Select(int.Parse).Select(getFuel).Sum();
Console.WriteLine(part2);

static int getFuel(int mass)
{
    var fuel = mass / 3 - 2;
    return (fuel <= 0 ? 0 : fuel + getFuel(fuel));
}
