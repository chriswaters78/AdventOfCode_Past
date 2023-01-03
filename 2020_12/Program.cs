class Program
{
    public static void Main(string[] args)
    {
        var instructions = File.ReadAllLines("input.txt").Select(str => (str[0], int.Parse(new String(str.Skip(1).ToArray())))).ToArray<(char op, int value)>();

        Console.WriteLine($"Part1: {part1(instructions)}");
        Console.WriteLine($"Part2: {part2(instructions)}");
    }

    static int part2((char op, int value)[] instructions)
    {

        //r
        //1,0 goes to 0, 1
        //0,1 goes to -1,0
        //l
        //1,0 goes to 0, -1
        //0,1 goes to 1,0

        var rots = new int[][][] {
            new[] {
                new[] { 0, -1 },
                new[] { 1, 0 }
            },
            new[] {
                new[] { 0, 1 },
                new[] { -1, 0 }
            },
        };

        var ps = (y: 0, x: 0);
        var pw = (y: 1, x: 10);

        foreach (var instr in instructions)
        {
            switch (instr.op)
            {
                case 'N':
                    pw.y += instr.value;
                    break;
                case 'S':
                    pw.y -= instr.value;
                    break;
                case 'E':
                    pw.x += instr.value;
                    break;
                case 'W':
                    pw.x -= instr.value;
                    break;
                case 'L':
                    for (int a = 0; a < instr.value; a += 90)
                    {
                        pw = (rots[1][0][0] * pw.y + rots[1][0][1] * pw.x, rots[1][1][0] * pw.y + rots[1][1][1] * pw.x);
                    }
                    break;
                case 'R':
                    for (int a = 0; a < instr.value; a += 90)
                    {
                        pw = (rots[0][0][0] * pw.y + rots[0][0][1] * pw.x, rots[0][1][0] * pw.y + rots[0][1][1] * pw.x);
                    }
                    break;
                case 'F':
                    ps.y += pw.y * instr.value;
                    ps.x += pw.x * instr.value;
                    break;
            }
        }

        return Math.Abs(ps.x) + Math.Abs(ps.y);
    }


    static int part1((char op, int value)[] instructions)
    {
        var dirs = new (int y, int x)[] { (1, 0), (0, 1), (-1, 0), (0, -1) };
        var dir = 1;
        var pos = (y: 0, x: 0);

        foreach (var instr in instructions)
        {
            switch (instr.op)
            {
                case 'N':
                    pos.y += instr.value;
                    break;
                case 'S':
                    pos.y -= instr.value;
                    break;
                case 'E':
                    pos.x += instr.value;
                    break;
                case 'W':
                    pos.x -= instr.value;
                    break;
                case 'L':
                    dir = dir - instr.value / 90;
                    dir = (dir + 4) % 4;
                    break;
                case 'R':
                    dir = dir + instr.value / 90;
                    dir = (dir + 4) % 4;
                    break;
                case 'F':
                    pos.y += dirs[dir].y * instr.value;
                    pos.x += dirs[dir].x * instr.value;
                    break;
            }
        }

        return Math.Abs(pos.x) + Math.Abs(pos.y);
    }
}