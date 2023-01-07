using IntCode;
using System.Collections;

//var test = new[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 };

var input = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();
//var input = new long[] {3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5};

Console.WriteLine($"*** START ***");
Console.WriteLine($"Part 1: {part1(input)}");
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

                        IEnumerator<long> amp5 = null;
                        var amp1 =  new Computer("AMP1", input.ToArray(), new InputEnumerator(new [] { p1, 0 }.AsEnumerable().GetEnumerator(), new Lazy<IEnumerator<long>>(() => amp5), false));
                        var amp2 =  new Computer("AMP2", input.ToArray(), new InputEnumerator(new [] { p2 }.AsEnumerable().GetEnumerator(), new Lazy<IEnumerator<long>>(() => amp1), true));
                        var amp3 =  new Computer("AMP3", input.ToArray(), new InputEnumerator(new [] { p3 }.AsEnumerable().GetEnumerator(), new Lazy<IEnumerator<long>>(() => amp2), true));
                        var amp4 =  new Computer("AMP4", input.ToArray(), new InputEnumerator(new [] { p4 }.AsEnumerable().GetEnumerator(), new Lazy<IEnumerator<long>>(() => amp3), true));
                        amp5 =      new Computer("AMP5", input.ToArray(), new InputEnumerator(new[] { p5 }.AsEnumerable().GetEnumerator(), new Lazy<IEnumerator<long>>(() => amp4), true));

                        while (amp5.MoveNext())
                        {
                        }
                        output = Math.Max(output, amp5.Current);
                    }
                }
            }
        }
    }

    return output;
}

static long part1(long[] input)
{
    var output = long.MinValue;
    for (int p1 = 0; p1 < 5; p1++)
    {
        for (int p2 = 0; p2 < 5; p2++)
        {
            for (int p3 = 0; p3 < 5; p3++)
            {
                for (int p4 = 0; p4 < 5; p4++)
                {
                    for (int p5 = 0; p5 < 5; p5++)
                    {
                        if (new[] { p1, p2, p3, p4, p5 }.Distinct().Count() != 5)
                        {
                            continue;
                        }
                        var amp1 = new Computer("AMP1", input.ToArray(), new long[] { p1, 0 }.AsEnumerable().GetEnumerator());
                        var amp2 = new Computer("AMP2", input.ToArray(), new InputEnumerator(new long[] { p2 }.AsEnumerable().GetEnumerator(), new Lazy<IEnumerator<long>>(() => amp1), true));
                        var amp3 = new Computer("AMP3", input.ToArray(), new InputEnumerator(new long[] { p3 }.AsEnumerable().GetEnumerator(), new Lazy<IEnumerator<long>>(() => amp2), true));
                        var amp4 = new Computer("AMP4", input.ToArray(), new InputEnumerator(new long[] { p4 }.AsEnumerable().GetEnumerator(), new Lazy<IEnumerator<long>>(() => amp3), true));
                        var amp5 = new Computer("AMP5", input.ToArray(), new InputEnumerator(new long[] { p5 }.AsEnumerable().GetEnumerator(), new Lazy<IEnumerator<long>>(() => amp4), true));

                        while (amp5.MoveNext())
                        {
                        }
                        output = Math.Max(output, amp5.Current);
                    }
                }
            }
        }
    }

    return output;
}

public class InputEnumerator : IEnumerator<long>
{
    public long Current => moveHeads ? heads.Current : tails.Value.Current;

    object IEnumerator.Current => Current;

    IEnumerator<long> heads;
    Lazy<IEnumerator<long>> tails;
    bool moveHeads = true;
    bool moveTails = false;
    bool enumerateTail;
    public InputEnumerator(IEnumerator<long> heads, Lazy<IEnumerator<long>> tails, bool shouldEnumerateTail)
    {
        this.heads = heads;
        this.tails = tails;
        this.enumerateTail = shouldEnumerateTail;
    }

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
        if (moveHeads)
        {
            moveHeads = heads.MoveNext();
            if (!moveHeads)
            {
                moveTails = true;
            }
            else
            {
                return true;
            }
        }
        if (!enumerateTail)
        {
            return true;
        }
        if (moveTails)
        {
            return moveTails = tails.Value.MoveNext();
        }
        throw new Exception($"No more values");
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }
}



