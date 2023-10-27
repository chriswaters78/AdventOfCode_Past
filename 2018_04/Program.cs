// See https://aka.ms/new-console-template for more information

int? currentGuard = null;
DateTime? currentTime = null;
DateTime? fellAsleep = null;
bool isSleeping = false;
var guardSleeping = new Dictionary<int, int>();
var minutesAsleep = new Dictionary<int, List<int>>();
foreach (var line in File.ReadAllLines("input.txt").OrderBy(l => l))
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
        for (var minute = fellAsleep; minute < currentTime; minute = minute.Value.AddMinutes(1))
        {
            if (!minutesAsleep.ContainsKey(currentGuard.Value))
            {
                minutesAsleep.Add(currentGuard.Value, new List<int>());
            }
            minutesAsleep[currentGuard.Value].Add(minute.Value.Minute);
        }
        fellAsleep = null;
    }
}
var guardSleepsMost = guardSleeping.OrderByDescending(kv => kv.Value).First().Key;
var minuteSleepsMost = minutesAsleep[guardSleepsMost].GroupBy(i => i).OrderByDescending(g => g.Count()).First().Key;
var part1 = guardSleepsMost * minuteSleepsMost;

Console.WriteLine($"Part 1: {part1}");

var guardSleepsMost2 = 
        minutesAsleep.Select(kv => new { Guard = kv.Key, Minute = kv.Value.GroupBy(i => i).OrderByDescending(g => g.Count()).First() })
        .OrderByDescending(kv => kv.Minute.Count()).First();

var part2 = guardSleepsMost2.Guard * guardSleepsMost2.Minute.Key;
Console.WriteLine($"Part 2: {part2}");
