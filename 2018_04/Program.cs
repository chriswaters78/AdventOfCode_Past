// See https://aka.ms/new-console-template for more information

int? currentGuard = null;
DateTime? currentTime = null;
DateTime? fellAsleep = null;
bool isSleeping = false;
Dictionary<int,int> guardSleeping = new Dictionary<int, int>();
foreach (var line in File.ReadAllLines("test.txt"))
{
    var sp = line.Split("] ");
    currentTime = DateTime.Parse(sp[0].Trim('['));
    if (sp[1].StartsWith("Guard #"))
    {
        currentGuard = int.Parse(sp[1].Split(" ")[1].Trim('#'));
    }
    if (sp[1].StartsWith("falls"))
    {
        isSleeping = true;
        fellAsleep = currentTime;
    }
    if (sp[1].StartsWith("wakes"))
    {
        isSleeping = false;
        if (!guardSleeping.ContainsKey(currentGuard.Value))
        {
            guardSleeping.Add(currentGuard.Value, 0);
        }
        guardSleeping[currentGuard.Value] += (currentTime.Value - fellAsleep.Value).Minutes;
        fellAsleep = null;
    }
}
var part1 = guardSleeping.OrderByDescending(kv => kv.Value).First();
