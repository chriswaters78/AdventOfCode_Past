using System.Text;

class Program
{
    public static void Main(string[] args)
    {
        var grid = File.ReadAllLines("input.txt").Select(str => str.ToArray()).ToArray();

        var part1A = part1(grid);
        var part2A = part2(grid);
    }

    static int part2(char[][] grid)
    {
        while (true)
        {
            Console.WriteLine(print(grid));
            bool moved = false;
            var clone = grid.Select(arr => arr.ToArray()).ToArray(); 
            for (int r = 0; r < clone.Length; r++)
            {
                for (int c = 0; c < clone.First().Length; c++)
                {
                    var offsets =
                        from or in Enumerable.Range(-1, 3)
                        from oc in Enumerable.Range(-1, 3)
                        where
                            !(or == 0 && oc == 0)
                        select (or, oc);

                    int seenOccupied = 0;
                    foreach (var (or,oc) in offsets)
                    {
                        int step = 1;
                        while (r + or * step >= 0 && r + step * or < clone.Length && c + step * oc >= 0 && c + step * oc < clone.First().Length)
                        {
                            if (grid[r + or * step][c + oc * step] == '#')
                            {
                                seenOccupied++;
                                break;
                            }
                            if (grid[r + or * step][c + oc * step] == 'L')
                            {
                                break;
                            }
                            step++;
                        }
                    }

                    if (grid[r][c] == 'L' && seenOccupied == 0)
                    {
                        moved = true;
                        clone[r][c] = '#';
                    }
                    if (grid[r][c] == '#' && seenOccupied >= 5)
                    {
                        moved = true;
                        clone[r][c] = 'L';
                    }
                }
            }
            if (!moved)
            {
                break;
            }
            grid = clone;
        }
        return grid.Sum(arr => arr.Count(ch => ch == '#'));
    }

    static int part1(char[][] grid)
    {
        while (true)
        {
            Console.WriteLine(print(grid));
            bool moved = false;
            var clone = grid.Select(arr => arr.ToArray()).ToArray();
            for (int r = 0; r < clone.Length; r++)
            {
                for (int c = 0; c < clone.First().Length; c++)
                {
                    var checks =
                        from or in Enumerable.Range(-1, 3)
                        from oc in Enumerable.Range(-1, 3)
                        where
                            !(or == 0 && oc == 0) &&
                            r + or >= 0 && r + or < clone.Length && c + oc >= 0 && c + oc < clone.First().Length
                        select grid[r + or][c + oc];

                    if (grid[r][c] == 'L' && checks.All(ch => ch != '#'))
                    {
                        moved = true;
                        clone[r][c] = '#';
                    }
                    if (grid[r][c] == '#' && checks.Count(ch => ch == '#') >= 4)
                    {
                        moved = true;
                        clone[r][c] = 'L';
                    }
                }
            }
            if (!moved)
            {
                break;
            }
            grid = clone;
        }

        return grid.Sum(arr => arr.Count(ch => ch == '#'));        
    }

    static string print(char[][] grid)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var line in grid)
        {
            sb.Append(line);
            sb.AppendLine();
        }

        return sb.ToString();
    }
}