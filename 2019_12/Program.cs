using System.Numerics;
using System.Collections.Generic;
using System.Linq;

namespace _2019_20
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //uint state = 0;
            //state = SetValue(0, 3, state);
            //unchecked
            //{
            //    state = SetValue(1, (byte)-1, state);

            //    var t3 = (sbyte)GetValue(2, state);
            //}

            //var t5 = GetValue(1, state);

            //part1();

            var x = simulateAxis(0);
            var y = simulateAxis(1);
            var z = simulateAxis(2);

            if (x.Item2 != y.Item2 || y.Item2 != z.Item2)
            {
                throw new Exception($"Chinese Remainder problem!");
            }
            var dx = new BigInteger(x.Item1 - x.Item2);
            var dy = new BigInteger(y.Item1 - y.Item2);
            var dz = new BigInteger(z.Item1 - z.Item2);

            var _gcd = BigInteger.GreatestCommonDivisor(dx, dy);
            var _lcm = dx * dy / _gcd;
            var gcd = BigInteger.GreatestCommonDivisor(_lcm, dz);

            var lcm = _lcm * dz / gcd;



            //xRepeat = 505
            //yRepeat = 2415
            //zRepeat = 1314
            //LCM = 1602521550
            if (true)
            {
            }
        }
        static (int, int) simulateAxis(int axis)
        {
            sbyte[] positions = File.ReadAllLines("input.txt").Select(line =>
            {
                var arr = line.Trim('<').Trim('>').Split(", ");
                return sbyte.Parse(arr[axis][2..]);
            }).ToArray();

            sbyte[] velocities = new sbyte[positions.Length];

            (uint pos, uint vel) state = (0, 0);
            for (int i = 0; i < positions.Length; i++)
            {
                state.pos = SetValue(i, (byte) positions[i], state.pos);
                state.vel = SetValue(i, (byte) velocities[i], state.vel);
            }

            var previous = new Dictionary<(uint positions, uint velocities), int>();
            int steps = 0;
            while (!previous.ContainsKey(state))
            {
                previous.Add(state, steps);
                steps++;
                for (int m1 = 0; m1 < positions.Length; m1++)
                {
                    for (int m2 = 0; m2 < positions.Length; m2++)
                    {
                        if (m1 == m2)
                        {
                            continue;
                        }

                        if (positions[m1] == positions[m2])
                        {
                            continue;
                        }

                        var m2Pos = GetValue(m2, state.pos);
                        var m1Pos = GetValue(m1, state.pos);
                        var m1Vel = GetValue(m1, state.vel);
                        var dv = (sbyte) (m1Vel + Math.Sign(m2Pos - m1Pos));
                        state.vel = SetValue(m1, (byte) dv, state.vel);
                    }
                }

                for (int m = 0; m < positions.Length; m++)
                {
                    var v = GetValue(m, state.vel);
                    var p = GetValue(m, state.pos);
                    state.pos = SetValue(m, (byte) (p + v), state.pos);
                }
            }

            return (steps, previous[state]);
        }

        static sbyte GetValue(int index, uint state) => (sbyte) ((state & (0xFF << (index * 8))) >> (index * 8));
        static uint SetValue(int index, byte value, uint state)
        {
            state &= ~((uint)0xFF << (index * 8));
            var mask = value << (index * 8);
            return (uint)(state | mask);
        }

        static int part1()
        {
            int[][] positions = File.ReadAllLines("input.txt").Select(line =>
            {
                var arr = line.Trim('<').Trim('>').Split(", ");
                return new[] { int.Parse(arr[0][2..]), int.Parse(arr[1][2..]), int.Parse(arr[2][2..]) };
            }).ToArray();

            int[][] velocities = new int[positions.Length][];

            for (int i = 0; i < velocities.Length; i++)
            {
                velocities[i] = new int[3];
            }

            for (int steps = 0; steps < 505; steps++)
            {
                for (int m1 = 0; m1 < positions.Length; m1++)
                {
                    for (int m2 = 0; m2 < positions.Length; m2++)
                    {
                        if (m1 == m2)
                        {
                            continue;
                        }

                        for (int axis = 0; axis < 3; axis++)
                        {
                            if (positions[m1][axis] == positions[m2][axis])
                            {
                                continue;
                            }

                            velocities[m1][axis] = velocities[m1][axis] + Math.Sign(positions[m2][axis] - positions[m1][axis]);
                        }
                    }
                }

                for (int m = 0; m < positions.Length; m++)
                {
                    for (int axis = 0; axis < 3; axis++)
                    {
                        positions[m][axis] += velocities[m][axis];
                    }
                }
            }

            int totalEnergy = 0;
            for (int m = 0; m < positions.Length; m++)
            {
                int pot = 0;
                int kin = 0;
                for (int axis = 0; axis < 3; axis++)
                {
                    pot += Math.Abs(positions[m][axis]);
                    kin += Math.Abs(velocities[m][axis]);
                }

                totalEnergy += pot * kin;
            }

            return totalEnergy;
        }
    }
}


