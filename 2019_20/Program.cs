using System.Collections.Generic;
using System.Linq;

namespace _2019_20
{
    internal class Program
    {
        static Dictionary<(int r, int c), List<((int r, int c) pos, int levelChange)>> graph;        
        static HashSet<(int r, int c)> portalKeys;

        static void Main(string[] args)
        {
            var maze = File.ReadAllLines("input.txt").SelectMany((line, r) => line.Select((ch, c) => (r, c, ch)))               
               .ToDictionary(tp => (tp.r, tp.c), tp => tp.ch);

            int maxR = maze.Keys.Max(tp => tp.r);
            int maxC = maze.Keys.Max(tp => tp.c);
            

            var portals = new Dictionary<string, List<((int r, int c) pos, int level)>>();
            Action<string, ((int r, int c), int level)> addOrUpdate = (key, value) =>
            {
                if (!portals.ContainsKey(key))
                {
                    portals[key] = new List<((int r, int c) pos, int level)>();
                }
                portals[key].Add(value);
            };

            for (int r = 0; r < maxR; r++)
            {
                for (int c = 0; c < maxC; c++)
                {
                    var patterns = new[] {
                        new { Letter1 = (r, c), Letter2 = (r, c + 1), Portal = (r, c + 2) },
                        new { Letter1 = (r, c + 1), Letter2 = (r, c + 2), Portal = (r, c) },
                        new { Letter1 = (r, c ), Letter2 = (r + 1, c), Portal = (r + 2, c) },
                        new { Letter1 = (r + 1, c ), Letter2 = (r + 2, c), Portal = (r, c) },
                    };

                    foreach (var pattern in patterns)
                    {
                        if (maze.ContainsKey(pattern.Letter1) && maze.ContainsKey(pattern.Letter2) && maze.ContainsKey(pattern.Portal))
                        {
                            if (Char.IsUpper(maze[pattern.Letter1]) && Char.IsUpper(maze[pattern.Letter2]) && maze[pattern.Portal] == '.')
                            {
                                if (pattern.Portal.r < 5 || pattern.Portal.r > maxR - 5 || pattern.Portal.Item2 < 5 || pattern.Portal.Item2 > maxC - 5)
                                {
                                    addOrUpdate($"{maze[pattern.Letter1]}{maze[pattern.Letter2]}", (pattern.Portal, -1));
                                }
                                else
                                {
                                    addOrUpdate($"{maze[pattern.Letter1]}{maze[pattern.Letter2]}", (pattern.Portal, 1));
                                }
                            }
                        }
                    }
                }
            }

            portalKeys = new HashSet<(int r, int c)>(portals.Values.SelectMany(arr => arr).Select(tp => tp.pos).Distinct());
            maze = new Dictionary<(int r, int c), char>(maze.Where(kvp => kvp.Value == '.'));
            graph = maze.Keys.ToDictionary(key => key, _ => new List<((int r, int c) pos, int level)>());

            foreach (var key in maze.Keys)
            {
                var possibleConnections = new[] { (key.r - 1, key.c), (key.r + 1, key.c), (key.r, key.c - 1), (key.r, key.c + 1) };
                foreach (var possibleConnection in possibleConnections)
                {
                    if (maze.ContainsKey(possibleConnection))
                    {
                        graph[key].Add((possibleConnection, 0));
                    }
                }
            }

            foreach (var portal in portals.Values.Where(list => list.Count == 2))
            {
                graph[portal[0].pos].Add(portal[1]);
                graph[portal[1].pos].Add(portal[0]);
            }

            var start = portals["AA"][0];
            var end = portals["ZZ"][0];

            var part1 = solve(end.pos, start.pos, true);
            var part2 = solve(end.pos, start.pos, false);
        }

        private static int solve((int r, int c) end, (int r, int c) start, bool part1)
        {
            var cache = new Dictionary<((int r, int c) pos, int level), int>();
            var queue = new Queue<(((int r, int c) pos, int level) state, int time)>();
            queue.Enqueue(((start, 0), 0));


            while (queue.Any())
            {
                var current = queue.Dequeue();
                if (cache.ContainsKey(current.state) && cache[current.state] <= current.time)
                {
                    continue;
                }

                if (portalKeys.Contains(current.state.pos))
                {
                }

                cache[current.state] = current.time;

                if (current.state == (end,0))
                {
                    return current.time;
                }

                foreach (var edge in graph[current.state.pos])
                {
                    var newLevel = current.state.level + (part1 ? 0 : edge.levelChange);
                    if (newLevel > 0)
                    {
                        continue;
                    }
                    queue.Enqueue(((edge.pos, newLevel), current.time + 1));
                }
            }

            return int.MaxValue;
        }
    }
}