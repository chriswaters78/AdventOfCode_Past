// See https://aka.ms/new-console-template for more information

var input = File.ReadAllLines("input.txt").Select(int.Parse).ToArray();

var answer = (from n1 in input
              from n2 in input
              from n3 in input
              where n1 + n2 + n3 == 2020
              select n1 * n2 * n3).First();

Console.WriteLine(answer);
