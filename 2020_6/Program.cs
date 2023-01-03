var input = File.ReadAllText("input.txt").Split($"{Environment.NewLine}{Environment.NewLine}").Select(str => str.Split(Environment.NewLine)).ToArray();

var counts = input.Select(arr => arr.SelectMany(str => str).Distinct().Count()).ToArray();

var counts2 = input.Select(arr => arr.Select(arr => arr.ToArray()).Aggregate((arr1, arr2) => arr1.Intersect(arr2).ToArray())).Select(arr => arr.Count()).ToArray();

var part1 = counts.Sum();
var part2 = counts2.Sum();
Console.WriteLine($"{part1}");
Console.WriteLine($"{part2}");