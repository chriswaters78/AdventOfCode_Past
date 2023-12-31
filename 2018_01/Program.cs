Console.WriteLine(File.ReadAllLines("input.txt").Sum(int.Parse));

var input = File.ReadAllLines("input.txt").Select(int.Parse).ToArray();
var seen = new HashSet<int>();
int f = 0, i = 0;
do
{
    f += input[i];
    i = (i + 1) % input.Length;
}
while (seen.Add(f));

Console.WriteLine(f);