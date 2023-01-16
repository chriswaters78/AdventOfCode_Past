using System.Diagnostics;

namespace _2019_18
{
    internal class Program
    {
        record struct State(uint keys, (int r, int c) pos);
        
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
            maze = File.ReadAllLines("input.txt").SelectMany((line, r) => line.Select((ch, c) => (r, c, ch)))
                .Where(tp => tp.ch != '#')
                .ToDictionary(tp => (tp.r, tp.c), tp => tp.ch);

            var start = maze.First(kvp => kvp.Value == '@').Key;
            var keyCount = maze.Count(kvp => Char.IsLower(kvp.Value));
            var allKeys = maze.Values.Where(ch => Char.IsLower(ch)).OrderBy(ch => ch).ToArray();
            foreach (var ch in allKeys)
            {
                finalKeys = SetBit(ch - 'a', finalKeys);
            }

            solve(new State(0, start));

            var best = cache.Where(kvp => kvp.Key.keys == finalKeys).Min(kvp => kvp.Value);
            Console.WriteLine($"Part 1: {best} in {watch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Cache entries: {cache.Count}");

        }

        static (int or, int oc)[] offsets = new [] { (0, 1), (0, -1), (1, 0), (-1, 0) };

        static void solve(State state)
        {
            var queue = new Queue<(State state, (int r, int c) lastPosition, int time)>();
            queue.Enqueue((state, (-1,-1), 0));

            while (queue.Count > 0)
            {
                (state, var lastPosition, var time) = queue.Dequeue();

                var neighbours = offsets.Select(offset => (state.pos.r + offset.or, state.pos.c + offset.oc)).Where(edge => maze.ContainsKey(edge)).ToArray();
                var isJunction = neighbours.Length > 2;
                //we can move to any of the 4 spaces around
                foreach (var edge in neighbours)
                {
                    if (lastPosition == edge)
                    {
                        //never backtrack, clear lastPosition if we find a key
                        continue;
                    }
                    if (Char.IsUpper(maze[edge]) && !IsBitSet(Char.ToLower(maze[edge]) - 'a', state.keys))
                    {
                        //locked door
                        continue;
                    }

                    var isNewKey = Char.IsLower(maze[edge]) && !IsBitSet(maze[edge] - 'a', state.keys);
                    State newState = state with
                    {
                        pos = edge,
                        keys = isNewKey
                            ? SetBit(maze[edge] - 'a', state.keys)
                            : state.keys
                    };

                    if (isJunction || isNewKey)
                    {
                        if (cache.ContainsKey(newState) && cache[newState] <= time + 1)
                        {
                            //been there before but better
                            continue;
                        }
                        cache[newState] = time + 1;
                    }

                    queue.Enqueue((newState, isNewKey ? (-1,-1) : state.pos, time + 1));
                }
            }
        }
    }
}