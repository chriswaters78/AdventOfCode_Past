﻿namespace _2019_16
{
    internal class Program
    {
        const int PHASES = 100;
        static int[] mask = new[] { 0, 1, 0, -1 };
        static void Main(string[] args)
        {
            var input = Enumerable.Repeat(File.ReadAllText("input.txt").Select(ch => int.Parse(ch.ToString())).ToArray(), 10000).SelectMany(arr => arr).ToArray();

            var finalOffset = int.Parse(File.ReadAllText("input.txt").Substring(0, 7));
            Console.WriteLine($"Skipping {finalOffset} characters.");
            //no value ever depends on a value before it
            //so we can skip all values before the value we want to read
            input = input.Skip(finalOffset).ToArray();

            Console.WriteLine($"Applying FFT to {input.Length} values");

            for (int phase = 0; phase < PHASES; phase++)
            {
                var output = new int[input.Length];
                int last = 0;
                for (int i = output.Length - 1; i >= 0; i--)
                {
                    output[i] = (input[i] + last) % 10;
                    last = output[i];
                }
                //Parallel.For(0, input.Length, i =>
                //{
                    //output[i] = Math.Abs(input.Zip(GetMask(i)).Sum(tp => tp.First * tp.Second)) % 10;
                    //output[i] = input.Skip(i).Sum() % 10;
                //});
                input = output;
                Console.WriteLine($"Phase {phase} done");
            }

            Console.WriteLine($"Part 1: {String.Join("", input.Take(8))}");
        }

        static IEnumerable<int> GetMask(int repeats)
        {
            int repeated = 1;
            int pos = 0;
            while (true)
            {
                while (repeated <= repeats)
                {
                    yield return mask[pos % mask.Length];
                    repeated++;
                }
                repeated = 0;
                pos++;
            }
        }
    }
}