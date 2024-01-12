var arr = File.ReadAllText("input.txt").Split(" ").ToArray();
(var players, var lastMarble) = (int.Parse(arr[0]), int.Parse(arr[6]));

var part1 = solve(players, lastMarble);
Console.WriteLine($"Part1: {part1}");
var part2 = solve(players, lastMarble * 100); 
Console.WriteLine($"Part2: {part2}");

long solve(int players, int lastMarble)
{
    var played = new LinkedList<int>();
    played.AddLast(0);
    var current = played.First;
    var player = 0;
    var playerScores = new long[players];
    for (int marble = 1; marble <= lastMarble; marble++)
    {
        if (marble % 23 == 0)
        {
            playerScores[player] += marble;
            for (int i = 0; i < 7; i++)
            {
                current = current?.Previous ?? played.Last;
            }
            playerScores[player] += current.Value;
            current = current?.Next ?? played.First;
            played.Remove(current.Previous);
        }
        else
        {
            current = played.AddAfter(current?.Next ?? played.First, marble);
        }
        player = (player + 1) % players;
    }

    return playerScores.Max();
}
