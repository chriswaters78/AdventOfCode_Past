using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntCode
{
    public static class ConsoleNumericEnumerable
    {
        public static IEnumerable<long> GetConsoleEnumerable()
        {
            while (true)
            {
                long input;
                Console.WriteLine($"Please enter a integer:");
                while (!long.TryParse(Console.ReadLine(), out input)) 
                {
                    Console.WriteLine($"Not an integer!");
                }

                yield return input;
            }
        }
    }
    public static class ConsoleStringEnumerable
    {
        public static IEnumerable<long> GetConsoleEnumerable()
        {
            while (true)
            {
                var input = Console.ReadLine();
                foreach (var ch in input)
                {
                    yield return (long) ch;
                }
                yield return 10;                
            }
        }
    }
}
