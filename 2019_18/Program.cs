using System.Diagnostics;

namespace _2019_18
{
    internal class Program
    {
        record struct State((uint keys, int keyCount) s, (int r, int c) pos);
        
        static Dictionary<State, int> cache = new Dictionary<State, int>();
        static Dictionary<(int r, int c), char> maze;

        static bool IsBitSet(int bit, uint state) => (state & (1U << bit)) != 0;
        static uint SetBit(int bit, uint state) => state |= 1U << bit;
        static uint ClearBit(int bit, uint state) => state &= ~(1U << bit);

        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            maze = File.ReadAllLines("input.txt").SelectMany((line, r) => line.Select((ch, c) => (r, c, ch)))
                .Where(tp => tp.ch != '#')
                .ToDictionary(tp => (tp.r, tp.c), tp => tp.ch);

            var start = maze.Single(kvp => kvp.Value == '@').Key;
            var keyCount = maze.Count(kvp => Char.IsLower(kvp.Value));
            var allKeys = maze.Values.Where(ch => Char.IsLower(ch)).OrderBy(ch => ch).ToArray();
            uint finalKeys = 0;
            foreach (var ch in allKeys)
            {
                finalKeys = SetBit(ch - 'a', finalKeys);
            }

            solve(new State((0, 0), start));

            var best = cache.Where(kvp => kvp.Key.s.keys == finalKeys).Min(kvp => kvp.Value);
            Console.WriteLine($"Part 1: {best} in {watch.ElapsedMilliseconds}ms");

        }

        static void solve(State state)
        {
            cache.Add(state, 0);
            Queue<State> queue = new Queue<State>();
            queue.Enqueue(state);

            while (queue.Count > 0)
            {
                state = queue.Dequeue();
                var t = cache[state];

                //we can move to any of the 4 spaces around
                foreach (var edge in new[] { (0, 1), (0, -1), (1, 0), (-1, 0) }.Select(tp => (state.pos.r + tp.Item1, state.pos.c + tp.Item2)))
                {
                    if (!maze.ContainsKey(edge))
                    {
                        continue;
                    }
                    if (Char.IsUpper(maze[edge]) && !IsBitSet(Char.ToLower(maze[edge]) - 'a', state.s.keys))
                    {
                        //locked door
                        continue;
                    }

                    State newState = state with
                    {
                        pos = edge,
                        s = Char.IsLower(maze[edge]) && !IsBitSet(maze[edge] - 'a', state.s.keys)
                            ? (SetBit(maze[edge] - 'a', state.s.keys), state.s.keyCount + 1)
                            : state.s
                    };

                    if (cache.ContainsKey(newState) && cache[newState] <= t + 1)
                    {
                        //been there before but better
                        continue;
                    }
                    cache[newState] = t + 1;
                    queue.Enqueue(newState);
                }
            }
        }
    }
}