// See https://aka.ms/new-console-template for more information

var input = File.ReadAllLines("input.txt").ToArray();

List<(int min, int max, char c, string str)> parsed = new List<(int, int, char, string)>();
foreach (var line in input)
{

    var min = int.Parse(line.Substring(0, line.IndexOf('-')));
    var max = int.Parse(line.Substring(line.IndexOf('-') + 1, line.IndexOf(' ') - line.IndexOf('-') - 1));
    var c = line[line.IndexOf(' ') + 1];
    var s = line.Substring(line.IndexOf(':') + 2, line.Length - line.IndexOf(':') - 2);
    parsed.Add((min, max, c, s));
}

var valid = parsed.Where(tp =>
{
    var cnt = tp.str.Count(ch => ch == tp.c);
    return cnt >= tp.min && cnt <= tp.max;

}).Count();

var valid2 = parsed.Where(tp =>
{
    return tp.str[tp.min - 1] == tp.c ^ tp.str[tp.max - 1] == tp.c;

}).Count();


Console.WriteLine(valid);
