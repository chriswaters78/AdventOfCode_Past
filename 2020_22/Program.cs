using System.Diagnostics;

namespace _2022_22
{
    internal class Program
    {
        const bool DEBUG = false;
        static int gamesPlayed = 1;
        static HashSet<string> cache = new HashSet<string>();
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var queue1 = new Queue<int>(File.ReadAllLines($"{args[0]}.txt").TakeWhile(line => line != "").Skip(1).Select(int.Parse));
            var queue2 = new Queue<int>(File.ReadAllLines($"{args[0]}.txt").SkipWhile(line => line != "Player 2:").Skip(1).Select(int.Parse));

            var p1Queue1 = new Queue<int>(queue1);
            var p1Queue2 = new Queue<int>(queue2);

            var winningQueueP1 = solvePart1(p1Queue1, p1Queue2) ? p1Queue1 : p1Queue2;
            var part1 = winningQueueP1.Reverse().Select((card, i) => card * (i + 1)).Sum();
            Console.WriteLine($"Part 1: {part1} in {watch.ElapsedMilliseconds}ms");

            var p2Queue1 = new Queue<int>(queue1);
            var p2Queue2 = new Queue<int>(queue2);

            var winningQueueP2 = solvePart2(p2Queue1, p2Queue2, 1) ? p2Queue1 : p2Queue2;
            var part2 = winningQueueP2.Reverse().Select((card, i) => card * (i + 1)).Sum();
            Console.WriteLine($"Part 2: {part2} in {watch.ElapsedMilliseconds}ms");
        }


        private static bool solvePart1(Queue<int> queue1, Queue<int> queue2)
        {
            while (queue1.Any() && queue2.Any())
            {
                var p1 = queue1.Dequeue();
                var p2 = queue2.Dequeue();

                if (p1 > p2)
                {
                    queue1.Enqueue(p1);
                    queue1.Enqueue(p2);
                }
                else
                {
                    queue2.Enqueue(p2);
                    queue2.Enqueue(p1);
                }
            }

            return queue1.Any();
        }

        private static bool solvePart2(Queue<int> queue1, Queue<int> queue2, int game)
        {
            if (DEBUG)
            {
                Console.WriteLine();
                Console.WriteLine($"=== Game {game} ===");
            }

            int rounds = 1;
            while (queue1.Any() && queue2.Any())
            {
                var key = getKey(game, queue1, queue2);
                if (cache.Contains(key))
                {
                    if (DEBUG)
                    {
                        Console.WriteLine($"The winner of game {game} is player 1!");
                    }
                    return true;
                }

                cache.Add(key);

                if (DEBUG)
                {
                    Console.WriteLine();
                    Console.WriteLine($"-- Round {rounds} (Game {game}) --");
                    Console.WriteLine($"Player 1's deck {String.Join(", ", queue1)}");
                    Console.WriteLine($"Player 2's deck {String.Join(", ", queue2)}");
                }

                var p1 = queue1.Dequeue();
                var p2 = queue2.Dequeue();
                if (DEBUG)
                {
                    Console.WriteLine($"Player 1 plays {p1}");
                    Console.WriteLine($"Player 2 plays {p2}");
                }

                bool winner;
                if (queue1.Count >= p1 && queue2.Count >= p2)
                {
                    gamesPlayed++;
                    winner = solvePart2(new Queue<int>(queue1.Take(p1)), new Queue<int>(queue2.Take(p2)), gamesPlayed);
                    if (DEBUG)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"...anyway, back to game {game}");
                    }
                }
                else
                {
                    winner = p1 > p2;
                }

                if (DEBUG)
                {
                    Console.WriteLine($"Player {(winner ? 1 : 2)} wins round {rounds} of game {game}!");
                }

                if (winner)
                {
                    queue1.Enqueue(p1);
                    queue1.Enqueue(p2);
                }
                else
                {
                    queue2.Enqueue(p2);
                    queue2.Enqueue(p1);
                }
                rounds++;
            }

            if (DEBUG)
            {
                if (queue1.Any())
                {
                    Console.WriteLine($"The winner of game {game} is player 1!");
                }
                else
                {
                    Console.WriteLine($"The winner of game {game} is player 2!");
                }
            }

            return queue1.Any();
        }


        private static string getKey(int game, Queue<int> queue1, Queue<int> queue2)
        {
            return $"G{game}P1:{string.Join("", queue1.Select(i => i.ToString("D2")))}P2:{string.Join("", queue2.Select(i => i.ToString("D2")))}";
        }
    }
}