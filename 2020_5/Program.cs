var input = File.ReadAllLines("input.txt").Select(str => Convert.ToInt32(str.Replace('F', '0').Replace('B', '1').Replace('R', '1').Replace('L', '0'), 2)).ToArray();

var part1 = input.Max();

var ord = input.OrderBy(i => i).ToArray();
int part2;
for (int i = 1; i < ord.Length; i++)
{
    if (ord[i] != ord[i-1] + 1)
    {
        part2 = ord[i] - 1;
        break;
    }
}

Console.WriteLine(part1);
