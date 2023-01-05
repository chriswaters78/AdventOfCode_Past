
int[] test1 = new[] { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 };
runProgram(test1);
assert(test1, new[] { 3500,9,10,70, 2,3,11,0, 99, 30, 40, 50 });

int[] test2 = new[] { 1, 0, 0, 0, 99 };
runProgram(test2);
assert(test2, new[] { 2, 0, 0, 0, 99 });

int[] test3 = new[] { 2, 3, 0, 3, 99 };
runProgram(test3);
assert(test3, new[] { 2, 3, 0, 6, 99 });

int[] test4 = new[] { 2, 4, 4, 5, 99, 0 };
runProgram(test4);
assert(test4, new[] { 2, 4, 4, 5, 99, 9801 });

int[] test5 = new[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 };
runProgram(test5);
assert(test5, new[] { 30, 1, 1, 4, 2, 5, 6, 0, 99 });

int[] initalMemory = File.ReadAllText("input.txt").Split(",").Select(int.Parse).ToArray();

{
    var memory = initalMemory.ToArray();
    memory[1] = 12;
    memory[2] = 2;
    runProgram(memory);
    Console.WriteLine($"Part 1: {memory[0]}");
}

for (int i1 = 0; i1 <= 99; i1++)
{
    for (int i2 = 0; i2 <= 99; i2++)
    {
        var memory = initalMemory.ToArray();
        memory[1] = i1;
        memory[2] = i2;
        runProgram(memory);
        if (memory[0] == 19690720)
        {
            Console.WriteLine($"Part 2: {100 * i1 + i2}");
        }
    }
}

static void runProgram(int[] memory)
{
    var ip = 0;
    while (memory[ip] != 99)
    {
        var opCode = memory[ip];
        switch (opCode)
        {
            case 1:
                memory[memory[ip + 3]] = memory[memory[ip + 1]] + memory[memory[ip + 2]];
                ip += 4;
                break;
            case 2:
                memory[memory[ip + 3]] = memory[memory[ip + 1]] * memory[memory[ip + 2]];
                ip += 4;
                break;

        }
    }
}

static void assert(int[] actual, int[] expected)
{
    if (!actual.SequenceEqual(expected))
    {
        throw new Exception($"Expected: {String.Join(", ", expected)}, Actual: {String.Join(", ", actual)}");
    }
}
