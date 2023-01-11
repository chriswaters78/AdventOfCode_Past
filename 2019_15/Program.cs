using IntCode;
using System.Collections;
using System.Reflection;
using System.Text;

var input = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();

Console.WriteLine($"*** START ***");

(var part1, var part2) = solve(input);
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

Console.WriteLine($"*** STOP ***");

static (long, long) solve(long[] input)
{
    var joystick = new ValueEnumerator();
    joystick.ValueProducer = l => l + 1;

    var repairBot = new Computer("REPAIR", input.ToArray(), joystick, false);

    var maze = new Dictionary<(int x, int y), char>();
    dfs(maze, (0, 0), repairBot, joystick);

    var part1 = bfs(maze, maze.Single(kvp => kvp.Value == 'S').Key, maze.Single(kvp => kvp.Value == 'G').Key);
    var part2 = bfs(maze, maze.Single(kvp => kvp.Value == 'G').Key, null);
    Console.WriteLine(Printer.PrintGridMap(maze));
    return (part1, part2);
}
static int bfs(Dictionary<(int x, int y), char> graph, (int x, int y) startPos, (int x, int y)? endPos)
{ 
    var furthest = int.MinValue;
    var visited = new HashSet<(int x, int y)>();
    Queue<((int x, int y) pos, int steps)> positions = new Queue<((int x, int y) pos, int steps)> ();
    positions.Enqueue((startPos, 0));
    var dirs = new (int ox, int oy)[] { (0, 1), (0, -1), (-1, 0), (1, 0) };
    while (positions.Count > 0)
    {
        var pos = positions.Dequeue();
        furthest = Math.Max(pos.steps, furthest);
        visited.Add(pos.pos);
        if (pos.pos == endPos)
        {
            return pos.steps;
        }
        for (int d = 0; d < dirs.Length; d++)
        {
            var newPos = (pos.pos.x + dirs[d].ox, pos.pos.y + dirs[d].oy);
            if (graph.ContainsKey(newPos) && !visited.Contains(newPos))
            {
                positions.Enqueue((newPos, pos.steps + 1));
            }
        }
    }

    return furthest;
}


static void dfs(Dictionary<(int x, int y), char> graph, (int x, int y) pos, Computer repairBot, ValueEnumerator joystick)
{
    //always try North first
    joystick.Value = 0;
    //N, S, W, E
    var dirs = new (int ox, int oy)[] { (0, 1), (0, -1), (-1, 0), (1, 0) };

    var invert = new Dictionary<int, int> { { 0, 1 }, { 1, 0}, { 2, 3}, { 3, 2} };

    if (graph.ContainsKey(pos))
    {
        //been here before
        return;
    }

    graph[pos] = pos == (0, 0) ? 'S' : '#';
    do
    {
        //try move in the current dirrection and read out the result
        repairBot.MoveNext();
        switch (repairBot.Current)
        {
            case 0:
                //we didn't move
                break;
            case 1:
            case 2:
                var newPos = (pos.x + dirs[joystick.Value].ox, pos.y + dirs[joystick.Value].oy);
                Console.WriteLine($"Moved {joystick.Value} from {pos} to {newPos}");
                Console.WriteLine(Printer.PrintGridMap(graph));
                if (repairBot.Current == 1)
                {
                    var oldJoystickValue = joystick.Value;
                    dfs(graph, newPos, repairBot, joystick);
                    joystick.Value = oldJoystickValue;
                }
                else
                {
                    graph[newPos] = 'G';
                }

                //move back to where we were
                joystick.Value = invert[(int) joystick.Value];
                repairBot.MoveNext();
                Console.WriteLine($"Moved back from {newPos} to {pos}");
                Console.WriteLine(Printer.PrintGridMap(graph));
                joystick.Value = invert[(int) joystick.Value];
                break;
        }
        //try next direction
        joystick.Value = (joystick.Value + 1) % 4;
    }
    while (joystick.Value != 0);
}