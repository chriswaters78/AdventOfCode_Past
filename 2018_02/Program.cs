using System.Linq;

var input = "input.txt";
var counts = File.ReadAllLines(input).Select(id => id.GroupBy(ch => ch).Select(grp => grp.Count()));
var part1 = counts.Where(counts => counts.Contains(3)).Count() * counts.Where(counts => counts.Contains(2)).Count();
Console.WriteLine($"Part 1: {part1}");


var correct = from id1 in File.ReadAllLines(input)
from id2 in File.ReadAllLines(input)
where id1.Zip(id2).Count(tp => tp.First == tp.Second) == id1.Length - 1
select (id1, id2);

var part2 = correct.First().id1.Intersect(correct.First().id2).ToList();
Console.WriteLine($"Part 2: {String.Join("",part2)}");