using System.Diagnostics;

namespace _2019_18
{
    internal class Program
    {
        record struct State(uint keys, (int r, int c) pos1, (int r, int c) pos2, (int r, int c) pos3, (int r, int c) pos4);
        
        static Dictionary<State, int> cache = new Dictionary<State, int>();
        static Dictionary<(int r, int c), char> maze;

        static bool IsBitSet(int bit, uint state) => (state & (1U << bit)) != 0;
        static uint SetBit(int bit, uint state) => state |= 1U << bit;
        static uint ClearBit(int bit, uint state) => state &= ~(1U << bit);

        static uint finalKeys = 0;

        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            maze = File.ReadAllLines("part2.txt").SelectMany((line, r) => line.Select((ch, c) => (r, c, ch)))
                .Where(tp => tp.ch != '#')
                .ToDictionary(tp => (tp.r, tp.c), tp => tp.ch);

            var starts = maze.Where(kvp => kvp.Value == '@').Select(kvp => kvp.Key).ToArray();

            var keyCount = maze.Count(kvp => Char.IsLower(kvp.Value));
            var allKeys = maze.Values.Where(ch => Char.IsLower(ch)).OrderBy(ch => ch).ToArray();
            foreach (var ch in allKeys)
            {
                finalKeys = SetBit(ch - 'a', finalKeys);
            }

            solve(new State(0, starts[0], starts[1], starts[2], starts[3]));

            var best = cache.Where(kvp => kvp.Key.keys == finalKeys).Min(kvp => kvp.Value);
            Console.WriteLine($"Part 1: {best} in {watch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Cache entries: {cache.Count}");

        }

        static (int or, int oc)[] offsets = new [] { (0, 1), (0, -1), (1, 0), (-1, 0) };

        static void solve(State states)
        {
            LinkedList<(State states, int lastMoved, (int r, int c) cameFrom, int time)> queue = new LinkedList<(State states, int lastMoved, (int r, int c) cameFrom, int time)>();
            queue.AddLast((states, 0, (-1, -1), 0));
            queue.AddLast((states, 1, (-1, -1), 0));
            queue.AddLast((states, 2, (-1, -1), 0));
            queue.AddLast((states, 3, (-1, -1), 0));

            while (queue.Count > 0)
            {
                (states, var lastRobotToMove, var cameFrom, var time) = queue.First.Value;
                queue.RemoveFirst();

                var position = lastRobotToMove switch { 0 => states.pos1, 1 => states.pos2, 2 => states.pos3, 3 => states.pos4 };

                var neighbours = offsets.Select(offset => (position.r + offset.or, position.c + offset.oc)).Where(edge => maze.ContainsKey(edge)).ToArray();
                var isJunction = neighbours.Length > 2;

                foreach (var edge in neighbours)
                {
                    if (cameFrom == edge)
                    {
                        //never backtrack
                        continue;
                    }
                    if (Char.IsUpper(maze[edge]) && !IsBitSet(Char.ToLower(maze[edge]) - 'a', states.keys))
                    {
                        //locked door
                        continue;
                    }

                    var isNewKey = Char.IsLower(maze[edge]) && !IsBitSet(maze[edge] - 'a', states.keys);
                    State newStates = states with
                    {
                        pos1 = lastRobotToMove == 0 ? edge : states.pos1,
                        pos2 = lastRobotToMove == 1 ? edge : states.pos2,
                        pos3 = lastRobotToMove == 2 ? edge : states.pos3,
                        pos4 = lastRobotToMove == 3 ? edge : states.pos4,
                        keys = isNewKey
                            ? SetBit(maze[edge] - 'a', states.keys)
                            : states.keys
                    };

                    if (isJunction || isNewKey)
                    {
                        if (cache.ContainsKey(newStates) && cache[newStates] <= time + 1)
                        {
                            //been there before but better
                            continue;
                        }
                        cache[newStates] = time + 1;
                    }

                    if (isNewKey)
                    {
                        //try the other robots after this series of moves has finished
                        queue.AddLast((newStates, (lastRobotToMove + 1) % 4, (-1, -1), time + 1));
                        queue.AddLast((newStates, (lastRobotToMove + 2) % 4, (-1, -1), time + 1));
                        queue.AddLast((newStates, (lastRobotToMove + 3) % 4, (-1, -1), time + 1));

                        //we have to add this oen again because it might actually be the one one that can move again
                        queue.AddLast((newStates, (lastRobotToMove + 4) % 4, (-1, -1), time + 1));
                    }
                    else
                    {
                        //carry on moving this robot down this path
                        queue.AddFirst((newStates, lastRobotToMove, position, time + 1));
                    }
                }
            }
        }
    }
}