const int MOD = 20201227;
Console.WriteLine($"Part1: {solve(13135480, 8821721)}");

long solve(long cardPK, long doorPK)
{
    (long cardValue, long doorValue) = (1,1);
    (long cardLoopSize, long doorLoopSize) = (-1,-1);
    for (int loop = 1; loop < int.MaxValue; loop++)
    {
        (cardValue, doorValue) = ((cardValue * 7) % 20201227, (doorValue * 7) % MOD);

        if (cardValue == cardPK)
            cardLoopSize = loop;
        if (doorValue == doorPK)
            doorLoopSize = loop;

        long encryption = 1;
        if (cardLoopSize != -1 && doorLoopSize != -1)
        {
            for (int l = 1; l <= cardLoopSize; l++)
                encryption = (encryption * doorPK) % MOD;
            
            return encryption;
        }
    }
    throw new();
}
