var low = 272091;
var high = 815432;

var t1 = test(111111);
var t2 = test(223450);
var t3 = test(123789);

int count1 = 0;
int count2 = 0;
for (int i = low; i <= high; i++)
{
    var a = test(i);
    if (a.p1)
    {
        count1++;
    }
    if (a.p2)
    {
        count2++;
    }
}

Console.WriteLine($"Part 1: {count1}");
Console.WriteLine($"Part 2: {count2}");

static (bool p1, bool p2) test(int i)
{
    var str = i.ToString();
    bool adjacent = false;
    bool leftToRight = true;
    for (int c = 0; c < str.Length - 1; c++)
    {
        adjacent |= str[c + 1] == str[c];
        leftToRight &= str[c + 1] >= str[c];
    }

    var p2Adjacent = str.GroupBy(ch => ch).Any(grp => grp.Count() == 2);

    return (adjacent && leftToRight, adjacent && leftToRight && p2Adjacent);
}

