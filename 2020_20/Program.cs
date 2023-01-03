using System.Diagnostics;
using System.Text;

class Program
{
    public static void Main(string[] args)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        var tiles = File.ReadAllText($"{args[0]}.txt").Split($"{Environment.NewLine}{Environment.NewLine}").Select(str => str.Split(Environment.NewLine))
            .ToDictionary(sp => long.Parse(sp[0][5..^1]), sp => sp.Skip(1).Select(str => str.ToArray()).ToArray());

        var size = (int)Math.Sqrt(tiles.Count);
        var board = new (long tileNo, int orientation)?[size][];
        for (int r = 0; r < size; r++)
        {
            board[r] = new (long tileNo, int orientation)?[size];
        }

        var solve = getSolutions(tiles, 0, 0, board);

        var part1 = solve.First()[0][0].Value.tileNo * solve.First()[0][size-1].Value.tileNo * solve.First()[size - 1][0].Value.tileNo * solve.First()[size - 1][size - 1].Value.tileNo;
        Console.WriteLine($"Found {solve.Count} solutions, first solution:");
        Console.WriteLine(printBoard(tiles, solve.First()));

        Console.WriteLine($"Part1 {part1} in {watch.ElapsedMilliseconds}ms");

        //get full image
        //try all rotations
        //and count sea monsters

        char[][] fullImage = new char[8 * size][];
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                var tileNo = solve.First()[r][c].Value.tileNo;
                var orientation = solve.First()[r][c].Value.orientation;
                var tile = rotateImage(rotations[orientation], tiles[tileNo]);
                for (int ri = 1; ri < 9; ri++)
                {
                    fullImage[8 * r + ri - 1] = new char[8 * size];
                    for (int ci = 1; ci < 9; ci++)
                    {
                        fullImage[8 * r + ri - 1][8 * c + ci - 1] = tile[ri][ci];
                    }
                }
            }
        }

        //01234567890123456789
        //                  # 
        //#    ##    ##    ###
        // #  #  #  #  #  #   
        var seeMonster = new char[3][];
        seeMonster[0] = new char[20];
        seeMonster[1] = new char[20];
        seeMonster[2] = new char[20];
        seeMonster[0][18] = '#';
        seeMonster[1][0] = '#';
        seeMonster[1][5] = '#';
        seeMonster[1][6] = '#';
        seeMonster[1][11] = '#';
        seeMonster[1][12] = '#';
        seeMonster[1][17] = '#';
        seeMonster[1][18] = '#';
        seeMonster[1][19] = '#';
        seeMonster[2][1] = '#';
        seeMonster[2][4] = '#';
        seeMonster[2][7] = '#';
        seeMonster[2][10] = '#';
        seeMonster[2][13] = '#';
        seeMonster[2][16] = '#';

        HashSet<(int r, int c)> foundMonsters = new HashSet<(int r, int c)>();
        for (int rot = 0; rot < rotations.Length; rot++)
        {
            var rotatedImage = rotateImage(rotations[rot], fullImage);
            for (int sr = 0; sr < fullImage.Length - seeMonster.Length; sr++)
            {
                for (int sc = 0; sc < fullImage[0].Length - seeMonster[0].Length; sc++)
                {
                    bool allMatch = true;
                    var partsOfMonster = new List<(int r, int c)>();
                    for (int r = 0; r < seeMonster.Length; r++)
                    {
                        for (int c = 0; c < seeMonster.First().Length; c++)
                        {
                            if (seeMonster[r][c] == '#')
                            {
                                if (rotatedImage[sr + r][sc + c] != '#')
                                {
                                    allMatch = false;
                                }
                                partsOfMonster.Add((sr + r, sc + c));
                            }
                        }
                    }
                    if (allMatch)
                    {
                        foreach (var monster in partsOfMonster)
                        {
                            foundMonsters.Add(monster);
                        }
                    }
                }
            }
            Console.WriteLine($"Found monster count at rot {rot}: {foundMonsters.Count}");
            var part2 = size * size * 8 * 8 - foundMonsters.Count;
            Console.WriteLine($"Part 2: {part2}");
        }

    }

    private static int[][][] rotations = new int[][][] {
        //identity
        new int[][] {
            new int[] {1,0},
            new int[] {0,1},
        },
        //rotate 180
        new int[][] {
            new int[] {-1,0},
            new int[] {0,-1},
        },
        //rotate 90
        new int[][] {
            new int[] {0,-1},
            new int[] {1,0},
        },
        //rotate 270
        new int[][] {
            new int[] {0,1},
            new int[] {-1,0},
        },
        new int[][] {
            new int[] {-1,0},
            new int[] {0,1},
        },
        new int[][] {
            new int[] {1,0},
            new int[] {0,-1},
        },
        new int[][] {
            new int[] {0,1},
            new int[] {1,0},
        },
        new int[][] {
            new int[] {0,-1},
            new int[] {-1,0},
        },
    };

    static char[][] rotateImage(int[][] rotation, char[][] image)
    {
        var rotatedImage = new Dictionary<(int r, int c), (decimal r, decimal c)>();
        decimal yoffset = ((decimal) image.Length - 1)/ 2m;
        decimal xoffset = ((decimal) image.First().Length - 1) / 2m;
        for (int r = 0; r < image.Length; r++)
        {
            for (int c = 0; c < image.First().Length; c++)
            {
                rotatedImage[(r,c)] = (r - yoffset, c - xoffset);
            }
        }

        for (int r = 0; r < image.Length; r++)
        {
            for (int c = 0; c < image.First().Length; c++)
            {
                rotatedImage[(r,c)] = (rotatedImage[(r,c)].r * rotation[0][0] + rotatedImage[(r, c)].c * rotation[0][1], rotatedImage[(r, c)].r * rotation[1][0] + rotatedImage[(r, c)].c * rotation[1][1]);
            }
        }

        var finalImage = new char[image.Length][];
        for (int r = 0; r < image.Length; r++)
        {
            finalImage[r] = new char[image.First().Length];
        }
        for (int r = 0; r < image.Length; r++)
        {
            for (int c = 0; c < image.First().Length; c++)
            {
                finalImage[(int) (rotatedImage[(r, c)].r + yoffset)][(int) (rotatedImage[(r, c)].c + xoffset)] = image[r][c];
            }
        }

        return finalImage;
    }

    private static List<(long tileNo, int orientation)?[][]> getSolutions(Dictionary<long, char[][]> tiles, int r, int c, (long tileNo, int orientation)?[][] board)
    {
        //Console.WriteLine($"Solving r:{r}, c:{c}");

        if (r == board.Length)
        {
            return new List<(long tileNo, int orientation)?[][]>() { board };
        }

        var newBoards = new List<(long tileNo, int orientation)?[][]>();
        var usedTiles = new HashSet<long>(board.SelectMany(i => i).Where(i => i.HasValue).Select(i => i.Value.tileNo));
        foreach (var (tileNo, image) in tiles.Where(t => !usedTiles.Contains(t.Key)))
        {
            //try and fit this tile at r,c in every rotation or flip
            //foreach that fits, clone a new board with the new tile, and call getSolutions recursively
            for (var ri = 0; ri < (r == 0 && c == 0 ? 1 : rotations.Length); ri++)
            {
                bool fits = true;
                //does top edge and left hand edge fit
                var rotatedImage = rotateImage(rotations[ri], tiles[tileNo]);
                if (r != 0)
                {
                    //check top edge
                    var matchingTile = rotateImage(rotations[board[r - 1][c].Value.orientation], tiles[board[r - 1][c].Value.tileNo]);
                    if (!rotatedImage.First().SequenceEqual(matchingTile.Last()))
                    {
                        fits = false;
                    }
                }
                if (c != 0)
                {
                    //check left edge
                    var matchingTile = rotateImage(rotations[board[r][c - 1].Value.orientation], tiles[board[r][c - 1].Value.tileNo]);
                    if (!rotatedImage.Select(arr => arr.First()).SequenceEqual(matchingTile.Select(arr => arr.Last())))
                    {
                        fits = false;
                    }
                }

                if (fits)
                {
                    //Console.WriteLine($"Found tile {tileNo} that fits at r:{r}, c:{c} with rotation {ri}, newBoards count {newBoards.Count}:");
                    //Console.WriteLine(printTile(rotatedImage));
                    var newBoard = board.Select(arr => arr.ToArray()).ToArray();
                    newBoard[r][c] = (tileNo, ri);
                    newBoards.Add(newBoard);
                    //Console.WriteLine(printBoard(tiles, newBoard));
                }
            }
        }

        //Console.WriteLine($"Solved r:{r}, c:{c}. Boards {newBoards.Count}");
        return newBoards.SelectMany(newBoard => getSolutions(tiles, c == board.Length - 1 ? (r + 1) : r, c == board.Length - 1 ? 0 : (c + 1), newBoard)).ToList();
    }

    static string printBoard(Dictionary<long, char[][]> tiles, (long tileNo, int orientation)?[][] board)
    {
        StringBuilder sbBoard = new StringBuilder();
        StringBuilder sbKeys = new StringBuilder();
        for (int rb = 0; rb < board.Length; rb++)
        {
            for (int rt = 0; rt < tiles.First().Value.Length; rt++)
            {
                for (int cb = 0; cb < board.First().Length; cb++)
                {
                    if (board[rb][cb].HasValue)
                    {
                        var tileNo = board[rb][cb].Value.tileNo;
                        var orientation = board[rb][cb].Value.orientation;
                        sbKeys.Append($"({tileNo},{orientation})");
                        var rotatedImage = rotateImage(rotations[orientation], tiles[tileNo]);
                        for (int ct = 0; ct < rotatedImage.First().Length; ct++)
                        {
                            sbBoard.Append(rotatedImage[rt][ct]);
                        }
                        sbBoard.Append(' ');
                    }
                }
                sbBoard.AppendLine();
                sbKeys.AppendLine();
            }
            sbKeys.AppendLine();
        }

        return $"{sbKeys}{Environment.NewLine}{sbBoard}";
    }

    static string printTile(char[][] tile)
    {
        StringBuilder sb = new StringBuilder();
        for (int r = 0; r < tile.Length; r++)
        {
            sb.AppendLine(new String(tile[r]));
        }

        return sb.ToString();
    }
}