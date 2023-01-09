using System.Collections;

namespace IntCode
{
    public class Computer : IEnumerator<long>
    {
        public readonly long[] Memory;
        private long rb;
        private long ip;
        private IEnumerator<long> inputEnumerator;
        private string name;
        private readonly bool log;

        public Computer(long[] initialMemory, IEnumerator<long> inputEnumerator) : this("COMP", initialMemory, inputEnumerator)
        {
        }

        public Computer(string name, long[] initialMemory, IEnumerator<long> inputEnumerator, bool log = true)
        {
            Memory = initialMemory.Concat(Enumerable.Repeat((long)0, 10000)).ToArray();
            ip = 0;
            this.inputEnumerator = inputEnumerator;
            this.name = name;
            this.log = log;
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

                var pref1 = new Lazy<long>(() => getRef(Memory[ip + 1], parameterMode1));
                var pref2 = new Lazy<long>(() => getRef(Memory[ip + 2], parameterMode2));
                var pref3 = new Lazy<long>(() => getRef(Memory[ip + 3], parameterMode3));

                var plit1 = new Lazy<long>(() => getLit(Memory[ip + 1], parameterMode1));
                var plit2 = new Lazy<long>(() => getLit(Memory[ip + 2], parameterMode2));
                var plit3 = new Lazy<long>(() => getLit(Memory[ip + 3], parameterMode3));

                switch (opCode)
                {
                    case 99:
                        goto halt;
                    case 1:
                    case 2:
                        if (parameterMode3 == 1)
                            throw new Exception($"Invalid immediate parameter mode for parameter 3, opcode {opCode}");

                        Memory[plit3.Value] = opCode == 1 ? pref1.Value + pref2.Value : pref1.Value * pref2.Value;
                        ip += 4;
                        break;
                    case 3:
                        if (parameterMode1 == 1)
                            throw new Exception($"Invalid immediate parameter mode for parameter 1 opcode 3");
                        
                        inputEnumerator.MoveNext();
                        var input = inputEnumerator.Current;
                        if (log) Console.WriteLine($"Computer {name} received {input}");
                        Memory[plit1.Value] = input;
                        ip += 2;
                        break;
                    case 4:
                        current = pref1.Value;
                        if (log) Console.WriteLine($"Computer {name} output {current}");
                        ip += 2;
                        return true;
                    case 5:
                        if (pref1.Value != 0)
                            ip = pref2.Value;
                        else
                            ip += 3;
                        break;
                    case 6:
                        if (pref1.Value == 0)
                            ip = pref2.Value;
                        else
                            ip += 3;
                        break;
                    case 7:
                        if (parameterMode3 == 1)
                            throw new Exception($"Invalid immediate parameter mode for parameter 3, opcode {opCode}");

                        Memory[plit3.Value] = pref1.Value < pref2.Value ? 1 : 0;
                        ip += 4;
                        break;
                    case 8:
                        if (parameterMode3 == 1)
                            throw new Exception($"Invalid immediate parameter mode for parameter 3, opcode {opCode}");

                        Memory[plit3.Value] = pref1.Value == pref2.Value ? 1 : 0;
                        ip += 4;
                        break;
                    case 9:
                        rb += pref1.Value;
                        ip += 2;
                        break;

                    default: throw new Exception($"Unexpected opcode {opCode}");
                }
            }

        halt:;
            if (log)
            {
                Console.WriteLine($"Computer {name} halted");
            }
            return false;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }


        private long getRef(long parameter, long parameterMode)
        {
            switch (parameterMode)
            {
                case 0: return Memory[parameter];
                case 1: return parameter;
                case 2: return Memory[parameter + rb];
                default: throw new NotImplementedException();
            }
        }

        private long getLit(long parameter, long parameterMode)
        {
            switch (parameterMode)
            {
                case 0: return parameter;
                case 1: return parameter;
                case 2: return parameter + rb;
                default: throw new NotImplementedException();
            }
        }

    }
}