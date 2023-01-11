using IntCode;
using System.Collections;
using System.Reflection;
using System.Text;

var input = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();

Console.WriteLine($"*** START ***");

//Console.WriteLine($"Part 1: {part1(input)}");

input[0] = 2;
Console.WriteLine($"Part 2: {part2(input)}");

Console.WriteLine($"*** STOP ***");
static long part2(long[] input)
{
    var joystick = new ValueEnumerator();

    joystick.Value = -1;
    var game = new Computer("GAME", input.ToArray(), joystick, false);

    var screen = new Dictionary<(int x, int y),  long>();
    var SCORE = (-1, 0);
    long score = long.MinValue;
    (int x, int y) ball = (-1, -1);
    (int x, int y) paddle = (-1, -1);
    while (game.MoveNext())
    {
        var x = (int)game.Current;
        game.MoveNext();
        var y = (int)game.Current;
        game.MoveNext();
        var tileid = game.Current;

        if ((x, y) == SCORE)
        {
            score = tileid;
            Console.WriteLine($"Score: {score}");
        }
        else
        {
            screen[(x, y)] = tileid;
        }

        paddle = (tileid == 3) ? (x, y) : paddle;
        ball = (tileid == 4) ? (x, y) : ball;


        Console.WriteLine(Printer.PrintGridMap(screen, printer));

        var newValue = -Math.Sign(paddle.x - ball.x);
        if (joystick.Value != newValue)
        {
            joystick.Value = newValue; 
        }
    }

    return score;
}

static char printer(long val)
{
    return val switch 
    {
        0 => ' ',
        1 => '#',
        2 => 'B',
        3 => '_',
        4 => 'o',
    };
};

static long part1(long[] input)
{
    var joystick = new ValueEnumerator();
    var game = new Computer("GAME", input.ToArray(), joystick, false);

    var screen = new Dictionary<(int x, int y), long>();
    while (game.MoveNext())
    {
        var x = (int) game.Current;
        game.MoveNext();
        var y = (int) game.Current;
        game.MoveNext();
        var tileid = game.Current;
        screen[(x, y)] = tileid;

    }
    Console.WriteLine(Printer.PrintGridMap(screen, printer));
    return screen.Values.Count(val => val == 2);
}