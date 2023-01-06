namespace IntCode
{
    public class Computer
    {
        public readonly int[] Memory;
        private int ip;


        public Computer(int[] initialMemory)
        {
            Memory = initialMemory.ToArray();
            ip = 0;
        }

        public void Run()
        {
            while (true)
            {
                var opCodeAndParameters = Memory[ip];
                var opCode = opCodeAndParameters % 100;
                var parameterMode1 = opCodeAndParameters / 100 % 10;
                var parameterMode2 = opCodeAndParameters / 1000 % 10;
                var parameterMode3 = opCodeAndParameters / 10000 % 10;

                switch (opCode)
                {
                    case 99:
                        goto halt;
                    case 1:
                    case 2:
                        if (parameterMode3 == 1)
                        {
                            throw new Exception($"Invalid immediate parameter mode for parameter 3, opcode {opCode}");
                        }

                        var p1 = getValue(Memory[ip + 1], parameterMode1);
                        var p2 = getValue(Memory[ip + 2], parameterMode2);
                        var outputAddress = Memory[ip + 3];
                        Memory[outputAddress] = opCode == 1 ? p1 + p2 : p1 * p2;
                        ip += 4;
                        break;
                    case 3:
                        if (parameterMode1 == 1)
                        {
                            throw new Exception($"Invalid immediate parameter mode for parameter 1 opcode 3");
                        }

                        Console.WriteLine($"Please enter a single integer:");
                        int input;
                        while (!int.TryParse(Console.ReadLine(), out input))
                        {
                            Console.WriteLine($"Please enter a valid integer");
                        }

                        Memory[Memory[ip + 1]] = input;
                        ip += 2;
                        break;
                    case 4:
                        Console.WriteLine(getValue(Memory[ip+1], parameterMode1));
                        ip += 2;
                        break;
                    case 5:
                        if (getValue(Memory[ip + 1], parameterMode1) != 0)
                        {
                            ip = getValue(Memory[ip + 2], parameterMode2);
                        }
                        else
                        {
                            ip += 3;
                        }
                        break;
                    case 6:
                        if (getValue(Memory[ip + 1], parameterMode1) == 0)
                        {
                            ip = getValue(Memory[ip + 2], parameterMode2);
                        }
                        else
                        {
                            ip += 3;
                        }
                        break;
                    case 7:
                        Memory[Memory[ip + 3]] = getValue(Memory[ip + 1], parameterMode1) < getValue(Memory[ip + 2], parameterMode2) ? 1 : 0;
                        ip += 4;
                        break;
                    case 8:
                        Memory[Memory[ip + 3]] = getValue(Memory[ip + 1], parameterMode1) == getValue(Memory[ip + 2], parameterMode2) ? 1 : 0;
                        ip += 4;
                        break;

                    default: throw new Exception($"Unexpected opcode {opCode}");
                }
            }

            halt:;
        }

        private int getValue(int parameter, int parameterMode)
        {
            switch (parameterMode)
            {
                case 0: return Memory[parameter];
                case 1: return parameter;
                default: throw new NotImplementedException();
            }
        }

    }
}