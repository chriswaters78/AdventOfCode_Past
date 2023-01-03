class Program
{
    public static void Main(string[] args)
    {
        (string op, int val)[]? memory = File.ReadAllLines("input.txt").Select(str => str.Split(' ')).Select(sp => (sp[0], int.Parse(sp[1]))).ToArray<(string op, int val)>();

        var part1 = check(memory.ToArray());
        var results = new List<(int acc, bool looped)>();
        foreach (var (instr, pc) in memory.Select((instr, i) => (instr,i)))
        {
            if (instr.op != "acc")
            {
                var clone = memory.ToArray();
                clone[pc] = (instr.op == "jmp" ? "nop" : "jmp", instr.val);
                results.Add(check(clone));
            }
        }

        var part2 = results.Single(result => !result.looped).acc;
        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    private static (int acc, bool looped) check((string op, int val)[] memory)
    {
        var pc = 0;
        var acc = 0;
        HashSet<int> visited = new HashSet<int>();
        while (true)
        {
            if (!visited.Add(pc))
            {
                return (acc, true);
            }
            if (pc == memory.Length)
            {
                return (acc, false);
            }
            var instr = memory[pc];
            switch (instr.op)
            {
                case "nop":
                    break;
                case "acc":
                    acc += instr.val;
                    break;
                case "jmp":
                    pc += instr.val - 1;
                    break;
                default:
                    throw new Exception();
            }
            pc++;
        }

        throw new Exception("Must terminate or overflow");
    }
}