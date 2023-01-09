using IntCode;
using System.Collections;
using System.Reflection;
using System.Text;

var input = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();

Console.WriteLine($"*** START ***");
Console.WriteLine($"Part 2: {part1(input)}");

Console.WriteLine($"*** STOP ***");

static long part1(long[] input)
{
    var joystick = new ValueEnumerator();

    var repairBot = new Computer("REPAIR", input.ToArray(), joystick, false);

    var maze = new Dictionary<(int x, int y), char>();
    dfs(maze, (0, 0), repairBot, joystick);

    var minSteps = bfs(maze, maze.Single(kvp => kvp.Value == 'G').Key);
    Console.WriteLine(Printer.PrintGridMap(maze));
    return -1;
}
static int bfs(Dictionary<(int x, int y), char> graph, (int x, int y) startPos)
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
        //if (graph[pos.pos] == 'G')
        //{
        //    return pos.steps;
        //}
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