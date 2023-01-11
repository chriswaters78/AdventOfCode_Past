using System.Numerics;

Part1.Run();
//BigInteger LENGTH = 119315717514047;
BigInteger ITERATIONS = 101741582076661;
BigInteger LENGTH = 10;

var example1 = StackDeck(StackDeck(Increment(0, 7)));
var example2 = StackDeck(Increment(Cut(0, 6), 7));
var example3 = Cut(Increment(Increment(0, 7), 9), -2);
var example4 = Cut(Increment(Increment(0, 7), 9), -2);
var example5 = Cut(Increment(Increment(Cut(Increment(Cut(Cut(Increment(Cut(StackDeck(0),-2), 7),8), -4), 7), 3),9),3),-1);


var cut1 = getCut(6);
var stack1 = getStack();
var comb1 = combine(cut1, stack1);

var comb = combine(combine(getIncrement(7), getStack()), getStack());
var test2 = Enumerable.Range(0, (int)LENGTH).Select(i => apply(comb, i)).ToArray();

//ok we have all the parts for a solution I think
//1. represent the shuffle by Ax+B
//2. keep combining to get 2, 4, 8 shuffles etc
//3. break down the ITERATIONS into a binary number and combine all the relevant shuffles to make ITERATIONS in total
//4. Solve for Ax + B = 2020 % LENGTH

//do we need to stop A and B from getting too large?

Console.WriteLine("STOP");

//x' = Ax + B
//x'' = Cx' + D
//    = ACx + BC + D

//gets the position that pos is mapped to
BigInteger apply((BigInteger a, BigInteger b) op, BigInteger pos)
{
    var res = (op.a * pos + op.b) % LENGTH;
    if (res < 0)
    {
        res += LENGTH;
    }
    return res;
}

(BigInteger a, BigInteger b) combine((BigInteger a, BigInteger b) op1, (BigInteger a, BigInteger b) op2)
{
    return (op1.a * op2.a, op1.b * op2.a + op2.b);
}

(BigInteger a, BigInteger b) getCut(BigInteger cut)
{
    return (1, LENGTH - cut);
}
(BigInteger a, BigInteger b) getStack()
{
    return (-1, LENGTH - 1);
}
(BigInteger a, BigInteger b) getIncrement(BigInteger increment)
{
    return (increment, 0);
}

BigInteger Cut(BigInteger pos, BigInteger cut)
{
    //x' = x + (LENGTH - cut)
    //A = 1, B = (LENGTH - cut)
    return (pos + (LENGTH - cut)) % LENGTH;
}

BigInteger StackDeck(BigInteger pos)
{
    //x = -1*x + (LENGTH - 1)
    //A = -1, B = (LENGTH - 1)
    return (LENGTH - 1 - pos) % LENGTH;
}
BigInteger Increment(BigInteger pos, BigInteger increment)
{
    //A = increment, B = 0
    return (increment * pos) % LENGTH;
}

class Part1
{
    public static void Run()
    {
        var instructions = File.ReadLines("input.txt").Select(line =>
        {
            switch (line)
            {
                case string increment when increment.Contains("increment"):
                    return ("INC", int.Parse(increment.Split(' ').Last()));
                case string cut when cut.Contains("cut"):
                    return ("CUT", int.Parse(cut.Split(' ').Last()));
                default:
                    return ("STACK", int.MinValue);
            }
        });

        var deck = Enumerable.Range(0, 10007).ToArray();
        foreach (var instr in instructions)
        {
            switch (instr)
            {
                case ("INC", int inc):
                    deck = incrementDeck(deck, inc);
                    break;
                case ("CUT", int cut):
                    deck = cutDeck(deck, cut);
                    break;
                case ("STACK", _):
                    deck = stackDeck(deck);
                    break;
            }
        }

        var part1 = Array.IndexOf(deck, 2019);

        Console.WriteLine($"Part 1: {part1}");
    }

    static int[] stackDeck(int[] deck)
    {
        return deck.Reverse().ToArray();
    }

    static int[] cutDeck(int[] deck, int cut)
    {
        if (cut < 0)
        {
            cut = deck.Length + cut;
        }

        return deck[cut..].Concat(deck[0..cut]).ToArray();
    }

    static int[] incrementDeck(int[] deck, int inc)
    {

        int[] newDeck = Enumerable.Range(-1, deck.Length).ToArray();

        int currPos = 0;
        newDeck[currPos] = deck[0];
        foreach (var card in deck.Skip(1))
        {
            for (int i = 0; i < inc;)
            {
                currPos = (currPos + 1) % deck.Length;

                if (newDeck[currPos] != -1)
                {
                    i++;
                }
                else
                {
                    //we hit something, should never happen!
                    throw new Exception($"BOOM!");
                }
            }
            newDeck[currPos] = card;
        }

        return newDeck;
    }
}
