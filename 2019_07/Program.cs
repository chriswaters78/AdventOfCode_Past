using IntCode;

//var test = new[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 };

//var input = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();
var input = new long[] {3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5};

Console.WriteLine($"*** START ***");
//Console.WriteLine($"Part 1: {part1(input)}");
Console.WriteLine($"Part 2: {part2(input)}");
Console.WriteLine($"*** STOP ***");

static long part2(long[] input)
{
    var output = long.MinValue;
    for (long p1 = 5; p1 < 10; p1++)
    {
        for (long p2 = 5; p2 < 10; p2++)
        {
            for (long p3 = 5; p3 < 10; p3++)
            {
                for (long p4 = 5; p4 < 10; p4++)
                {
                    for (long p5 = 5; p5 < 10; p5++)
                    {
                        if (new[] { p1, p2, p3, p4, p5 }.Distinct().Count() != 5)
                        {
                            continue;
                        }
                        if (!new[] { p1, p2, p3, p4, p5 }.SequenceEqual(new long[] { 9, 8, 7, 6, 5 }))
                        {
                            continue;
                        }

                        IEnumerable<long> amp5 = null;
                        var amp1 = new Computer("AMP1", input.ToArray(), lazyConcat(new[] { p1, 0 }, new Lazy<IEnumerable<long>>(() => amp5)).GetEnumerator()).Run();
                        var amp2 = new Computer("AMP2", input.ToArray(), lazyConcat(new[] { p2 }, new Lazy<IEnumerable<long>>(() => amp1)).GetEnumerator()).Run();
                        var amp3 = new Computer("AMP3", input.ToArray(), lazyConcat(new[] { p3 }, new Lazy<IEnumerable<long>>(() => amp2)).GetEnumerator()).Run();
                        var amp4 = new Computer("AMP4", input.ToArray(), lazyConcat(new[] { p4 }, new Lazy<IEnumerable<long>>(() => amp3)).GetEnumerator()).Run();
                        amp5 = new Computer("AMP5", input.ToArray(), lazyConcat(new[] { p5 }, new Lazy<IEnumerable<long>>(() => amp4)).GetEnumerator()).Run();

                        //var results = amp1.ToArray();
                        //var results = amp2.ToArray();
                        //var results = amp3.ToArray();
                        //var results = amp4.ToArray();
                        var results = amp5.ToArray();
                        output = Math.Max(output, results[results.Length - 1]);
                    }
                }
            }
        }
    }

    return output;
}

static IEnumerable<long> lazyConcat(IEnumerable<long> head, Lazy<IEnumerable<long>> lazyTail)
{
    foreach (var i in head)
    {
        yield return i;
    }

    foreach (var i in lazyTail.Value)
    {
        yield return i;
    }
}


//static long part1(long[] input)
//{
//    var output = long.MinValue;
//    for (int p1 = 0; p1 < 5; p1++)
//    {
//        for (int p2 = 0; p2 < 5; p2++)
//        {
//            for (int p3 = 0; p3 < 5; p3++)
//            {
//                for (int p4 = 0; p4 < 5; p4++)
//                {
//                    for (int p5 = 0; p5 < 5; p5++)
//                    {
//                        if (new[] { p1, p2, p3, p4, p5 }.Distinct().Count() != 5)
//                        {
//                            continue;
//                        }
//                        var amp1 = new Computer("AMP1", input.ToArray(), new long[] { p1, 0 }).Run().Last();
//                        var amp2 = new Computer("AMP2", input.ToArray(), new long[] { p2, amp1 }).Run().Last();
//                        var amp3 = new Computer("AMP3", input.ToArray(), new long[] { p3, amp2 }).Run().Last();
//                        var amp4 = new Computer("AMP4", input.ToArray(), new long[] { p4, amp3 }).Run().Last();
//                        var amp5 = new Computer("AMP5", input.ToArray(), new long[] { p5, amp4 }).Run().Last();

//                        output = Math.Max(output, amp5);
//                    }
//                }
//            }
//        }
//    }

//    return output;
//}
