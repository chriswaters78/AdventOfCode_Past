using System.Text;

class Program
{
    public static void Main(string[] args)
    {
        var tiles = File.ReadAllText("input.txt").Split($"{Environment.NewLine}{Environment.NewLine}").Select(str => str.Split(Environment.NewLine))
            .ToDictionary(sp => long.Parse(sp[0][5..^1]), sp => sp.Skip(1).Select(str => str.ToArray()).ToArray());

        var size = (int)Math.Sqrt(tiles.Count);
        var board = new (long tileNo, int orientation)?[size][];
        for (int r = 0; r < size; r++)
        {
            board[r] = new (long tileNo, int orientation)?[size];
        }

        var solve = getSolutions(tiles, 0, 0, board);

        var part1 = solve.First()[0][0].Value.tileNo * solve.First()[0][2].Value.tileNo * solve.First()[2][0].Value.tileNo * solve.First()[2][2].Value.tileNo;
        Console.WriteLine($"Found {solve.Count} solutions, first solution:");
        Console.WriteLine(printBoard(tiles, solve.First()));

        Console.WriteLine($"Part1 {part1}");

        //get full image
        //try all rotations
        //and count sea monsters
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
        for (int r = 0; r < 10; r++)
        {
            for (int c = 0; c < 10; c++)
            {
                rotatedImage[(r,c)] = (r - yoffset, c - xoffset);
            }
        }

        for (int r = 0; r < 10; r++)
        {
            for (int c = 0; c < 10; c++)
            {
                rotatedImage[(r,c)] = (rotatedImage[(r,c)].r * rotation[0][0] + rotatedImage[(r, c)].c * rotation[0][1], rotatedImage[(r, c)].r * rotation[1][0] + rotatedImage[(r, c)].c * rotation[1][1]);
            }
        }

        var finalImage = new char[10][];
        for (int r = 0; r < 10; r++)
        {
            finalImage[r] = new char[10];
        }
        for (int r = 0; r < 10; r++)
        {
            for (int c = 0; c < 10; c++)
            {
                finalImage[(int) (rotatedImage[(r, c)].r + yoffset)][(int) (rotatedImage[(r, c)].c + xoffset)] = image[r][c];
            }
        }

        return finalImage;
    }

    private static List<(long tileNo, int orientation)?[][]> getSolutions(Dictionary<long, char[][]> tiles, int r, int c, (long tileNo, int orientation)?[][] board)
    {
        Console.WriteLine($"Solving r:{r}, c:{c}");

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

        Console.WriteLine($"Solved r:{r}, c:{c}. Boards {newBoards.Count}");
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