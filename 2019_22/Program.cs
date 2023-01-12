using System.Numerics;

//1. Combine all ops to get a represention of a single pass of the shuffle in the form Ax+B (MOD L)
//2. keep combining these to get 2, 4, 8 shuffles etc
//3. break down the ITERATIONS into a binary number and combine all the relevant shuffles to make ITERATIONS in total
//4. Solve for Ax + B = 2020 % LENGTH

const int LENGTH1 = 10007;
var shuffle1 = getShuffle(LENGTH1);
var part1 = apply(LENGTH1, shuffle1, 2019);

BigInteger LENGTH2 = 119315717514047;
BigInteger ITERATIONS = 101741582076661;

var shufflePowers = new (BigInteger a, BigInteger b)[64];
var powersOfTwo = new BigInteger[64];

shufflePowers[0] = getShuffle(LENGTH2);
powersOfTwo[0] = 1;
for (int i = 1; i < shufflePowers.Length; i++)
{
    shufflePowers[i] = combine(LENGTH2, shufflePowers[i - 1], shufflePowers[i - 1]);
    powersOfTwo[i] = 2 * powersOfTwo[i - 1];
}

var iter = ITERATIONS;
(BigInteger a, BigInteger b) fullShuffle = (1, 0);
for (int i = shufflePowers.Length - 1; i >= 0; i--)
{
    if (iter / powersOfTwo[i] > 0)
    {
        fullShuffle = combine(LENGTH2, fullShuffle, shufflePowers[i]);
        iter -= powersOfTwo[i];
    }
}

//for special case when m is PRIME, which it is :)
//https://en.wikipedia.org/wiki/Modular_multiplicative_inverse#Using_Euler.27s_theorem
var aModInverse = BigInteger.ModPow(fullShuffle.a, LENGTH2 - 2, LENGTH2);

//now solve our equation using the a', the mod inverse of a
//(Ax + B) = 2020 
//x + B.A' = 2020.A'
//X = 2020.A' - B.A'
var part2 = (2020 * aModInverse - fullShuffle.b * aModInverse) % LENGTH2;
if (part2 < 0) part2 += LENGTH2;

//verify we have the correct answer
var verify = (fullShuffle.a * part2 + fullShuffle.b) % LENGTH2;
if (verify != 2020) throw new Exception($"BOOM!");

Console.WriteLine($"Part 2: {part2}");

(BigInteger a, BigInteger b) getShuffle(BigInteger length)
{
    return File.ReadLines("input.txt").Select(line =>
    {
        switch (line)
        {
            case string increment when increment.Contains("increment"):
                return getIncrement(length, BigInteger.Parse(increment.Split(' ').Last()));
            case string cut when cut.Contains("cut"):
                return getCut(length, BigInteger.Parse(cut.Split(' ').Last()));
            default:
                return getStack(length);
        }
    })
    .Aggregate(((BigInteger)1, (BigInteger)0), (acc, next) => combine(length, acc, next), acc => acc);
}

BigInteger apply(BigInteger length, (BigInteger a, BigInteger b) op, BigInteger pos)
{
    var res = (op.a * pos + op.b) % length;
    if (res < 0) res += length;
    return res;
}

(BigInteger a, BigInteger b) combine(BigInteger length, (BigInteger a, BigInteger b) op1, (BigInteger a, BigInteger b) op2)
{
    //x' = Ax + B
    //x'' = Cx' + D
    //    = ACx + BC + D
    var a = (op1.a * op2.a) % length;
    if (a < 0) a += length;
    var b = (op1.b * op2.a + op2.b) % length;
    if (b < 0) b += length;

    return (a, b);
}

//x' = x + (LENGTH - cut)
(BigInteger a, BigInteger b) getCut(BigInteger length, BigInteger cut) => (1, length - cut);
//x = -1*x + (LENGTH - 1)
(BigInteger a, BigInteger b) getStack(BigInteger length) => (-1, length - 1);
//A = increment, B = 0
(BigInteger a, BigInteger b) getIncrement(BigInteger length, BigInteger increment) => (increment, 0);
