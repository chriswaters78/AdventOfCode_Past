class Program
{
    private static int intyr;

    public static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");

        var board = new HashSet<(int x, int y, int z, int w)>();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines.First().Length; x++)
            {
                if (lines[y][x] == '#')
                {
                    board.Add((x, y, 0, 0));
                }                
            }
        }

        for (int step = 0; step < 6; step++)
        {
            (int minx, int miny, int minz, int minw) = (board.Min(tp => tp.x), board.Min(tp => tp.y), board.Min(tp => tp.z), board.Min(tp => tp.w));
            (int maxx, int maxy, int maxz, int maxw) = (board.Max(tp => tp.x), board.Max(tp => tp.y), board.Max(tp => tp.z), board.Max(tp => tp.w));

            var newBoard = new  HashSet<(int x, int y, int z, int w)>();
            for (int x = minx - 1; x <= maxx + 1; x++)
            {
                for (int y = miny - 1; y <= maxy + 1; y++)
                {
                    for (int z = minz - 1; z <= maxz + 1; z++)
                    {
                        for (int w = minw - 1; w <= maxw + 1; w++)
                        {
                            var neighbours =
                                from dx in Enumerable.Range(-1, 3)
                                from dy in Enumerable.Range(-1, 3)
                                from dz in Enumerable.Range(-1, 3)
                                from dw in Enumerable.Range(-1, 3)
                                where !(dx == 0 && dy == 0 && dz == 0 && dw == 0)
                                select (x + dx, y + dy, z + dz, w + dw);

                            var neighboursActive = neighbours.Count(tp => board.Contains(tp));


                            if (board.Contains((x, y, z, w)))
                            {
                                //if 2 or 3 neighbours are alive
                                if (neighboursActive == 2 || neighboursActive == 3)
                                {
                                    //remains active
                                    newBoard.Add((x, y, z, w));
                                }
                            }
                            else
                            {
                                if (neighboursActive == 3)
                                {
                                    //exactly 3, becomes active
                                    newBoard.Add((x, y, z, w));
                                }
                            }
                        }
                    }
                }
            }

            board = newBoard;
        }

        var part1 = board.Count;
    }
}