﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntCode
{
    public static class ConsoleEnumerable
    {
        public static IEnumerable<int> GetConsoleEnumerable()
        {
            while (true)
            {
                int input;
                Console.WriteLine($"Please enter a integer:");
                while (!int.TryParse(Console.ReadLine(), out input))
                {
                    Console.WriteLine($"Not an integer!");
                }

                yield return input;
            }
        }
    }
}