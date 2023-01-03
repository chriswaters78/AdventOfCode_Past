public class Program
{
    //mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
    //mem[8] = 11
    //mem[7] = 101
    //mem[8] = 0
    static void Main(string[] args)
    {
        var lines = File.ReadAllLines(args[0]);
        //List<(ulong, List<(int, ulong)>)> steps = new List<(ulong, List<(int, ulong)>)>();

        //or mask1 to set 1s 00010000 || 101011010
        string mask = null;
        ulong mask1 = 0;
        //and mask 2 to clear 1s
        // 111101111 || 101011010
        ulong mask0 = 0;
        var memoryPart1 = new Dictionary<int, ulong>();
        var memoryPart2 = new Dictionary<ulong, ulong>();
        foreach (var line in lines)
        {
            Console.WriteLine($"Memory count: {memoryPart2.Count}");
            if (line.StartsWith("mask"))
            {
                mask = line.Substring(7);
                mask1 = Convert.ToUInt64(new String(line.Substring(7).Select(ch => ch == '1' ? '1' : '0').ToArray()).PadLeft(64, '0'), 2);
                mask0 = Convert.ToUInt64(new String(line.Substring(7).Select(ch => ch == '0' ? '0' : '1').ToArray()).PadLeft(64, '1'), 2);      
            }
            else
            {
                var sp = line.Split(' ').ToArray();
                int address = int.Parse(new String(sp[0].Skip(4).TakeWhile(ch => ch != ']').ToArray()));
                var value = ulong.Parse(sp[2]);
                List<ulong> generatedAddresses = new List<ulong>();
                generatedAddresses.Add(0);
                int count = 0;
                foreach (var ch in mask)
                {
                    switch (ch)
                    {
                        case '1':
                            for (int i = 0; i < generatedAddresses.Count; i++)
                            {
                                generatedAddresses[i] = (generatedAddresses[i] << 1) + 1;
                            }
                            break;
                        case '0':
                            for (int i = 0; i < generatedAddresses.Count; i++)
                            {
                                generatedAddresses[i] = (generatedAddresses[i] << 1) + ((((ulong) address) & ((ulong) 1 << (36 - count - 1))) != 0 ? (ulong) 1 : (ulong) 0);
                            }
                            break;
                        case 'X':
                            var originalCount = generatedAddresses.Count;
                            for (int i = 0; i < originalCount; i++)
                            {
                                var original = generatedAddresses[i];
                                generatedAddresses[i] = (original << 1) + 0;
                                generatedAddresses.Add((original << 1) + 1);
                            }
                            break;
                    }
                    count++;
                }

                foreach (var a in generatedAddresses)
                {
                    memoryPart2[a] = value;
                }


                if (!memoryPart1.ContainsKey(address))
                {
                    memoryPart1[address] = 0;
                }
                memoryPart1[address] = (value | mask1) & mask0;
            }
        }


        Console.WriteLine($"Part1: Sum all memory {memoryPart1.Select(kvp => kvp.Value).Aggregate((s1, s2) => s1 + s2)}");
        Console.WriteLine($"Part2: Sum all memory {memoryPart2.Select(kvp => kvp.Value).Aggregate((s1, s2) => s1 + s2)}");
    }
}