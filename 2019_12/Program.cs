using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace _2019_20
{
    internal class Program
    {
        static void Main(string[] args)
        {
            part1();

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

            Console.WriteLine($"Part 2: {lcm}");
        }
        static (int, int) simulateAxis(int axis)
        {
            int[] positions = File.ReadAllLines("input.txt").Select(line =>
            {
                var arr = line.Trim('<').Trim('>').Split(", ");
                return int.Parse(arr[axis][2..]);
            }).ToArray();

            int[] velocities = new int[positions.Length];

            
            (int[] pos, int[] vel) state = (positions, velocities);
            var previous = new Dictionary<(int[] pos, int[] vel), int>(new ArrayComparer());
            int steps = 0;
            while (!previous.ContainsKey(state))
            {
                previous.Add(state, steps);
                state = (state.pos.ToArray(), state.vel.ToArray());
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

                        var m2Pos = state.pos[m2];
                        var m1Pos = state.pos[m1];
                        var m1Vel = state.vel[m1];
                        var dv = m1Vel + Math.Sign(m2Pos - m1Pos);
                        state.vel[m1] = dv;
                    }
                }

                for (int m = 0; m < positions.Length; m++)
                {
                    var v = state.vel[m];
                    var p = state.pos[m];
                    state.pos[m] = p + v;
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

public class ArrayComparer : IEqualityComparer<(int[] positions, int[] velocities)>
{

    public bool Equals((int[], int[]) x, (int[], int[]) y)
    {
        return x.Item1.SequenceEqual(y.Item1) && x.Item2.SequenceEqual(y.Item2);
    }


    public int GetHashCode([DisallowNull] (int[], int[]) obj)
    {
        int code = 0;
        for (int i = 0; i < obj.Item1.Length; i++)
        {
            code ^= obj.Item1[i].GetHashCode();
            code ^= obj.Item2[i].GetHashCode();
        }

        return code;
    }
}

