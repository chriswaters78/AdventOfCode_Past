using System.Collections;

namespace IntCode
{
    public class Computer : IEnumerator<long>
    {
        public readonly long[] Memory;
        private long ip;
        private IEnumerator<long> inputEnumerator;
        private string name;

        public Computer(long[] initialMemory, IEnumerator<long> inputEnumerator) : this("COMP", initialMemory, inputEnumerator)
        {
        }

        public Computer(string name, long[] initialMemory, IEnumerator<long> inputEnumerator)
        {
            Memory = initialMemory.ToArray();
            ip = 0;
            this.inputEnumerator = inputEnumerator;
            this.name = name;
        }

        private long current;
        public long Current => current;
        object IEnumerator.Current => current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            while (true)
            {
                var opCodeAndParameters = Memory[ip];
                var opCode = opCodeAndParameters % 100;
                var parameterMode1 = opCodeAndParameters / 100 % 10;
                var parameterMode2 = opCodeAndParameters / 1000 % 10;
                var parameterMode3 = opCodeAndParameters / 10000 % 10;

                var p1 = new Lazy<long>(() => getValue(Memory[ip + 1], parameterMode1));
                var p2 = new Lazy<long>(() => getValue(Memory[ip + 2], parameterMode2));
                var p3 = new Lazy<long>(() => getValue(Memory[ip + 3], parameterMode3));

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

                        Memory[Memory[ip + 3]] = opCode == 1 ? p1.Value + p2.Value : p1.Value * p2.Value;
                        ip += 4;
                        break;
                    case 3:
                        if (parameterMode1 == 1)
                        {
                            throw new Exception($"Invalid immediate parameter mode for parameter 1 opcode 3");
                        }
                        inputEnumerator.MoveNext();
                        var input = inputEnumerator.Current;
                        Console.WriteLine($"Computer {name} received {input}");
                        Memory[Memory[ip + 1]] = input;
                        ip += 2;
                        break;
                    case 4:
                        current = getValue(Memory[ip + 1], parameterMode1);
                        Console.WriteLine($"Computer {name} output {current}");
                        ip += 2;
                        return true;
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
                        if (parameterMode3 == 1)
                        {
                            throw new Exception($"Invalid immediate parameter mode for parameter 3, opcode {opCode}");
                        }

                        Memory[Memory[ip + 3]] = getValue(Memory[ip + 1], parameterMode1) < getValue(Memory[ip + 2], parameterMode2) ? 1 : 0;
                        ip += 4;
                        break;
                    case 8:
                        if (parameterMode3 == 1)
                        {
                            throw new Exception($"Invalid immediate parameter mode for parameter 3, opcode {opCode}");
                        }

                        Memory[Memory[ip + 3]] = getValue(Memory[ip + 1], parameterMode1) == getValue(Memory[ip + 2], parameterMode2) ? 1 : 0;
                        ip += 4;
                        break;

                    default: throw new Exception($"Unexpected opcode {opCode}");
                }
            }

        halt:;
            Console.WriteLine($"Computer {name} halted");
            return false;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }


        private long getValue(long parameter, long parameterMode)
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