using System.Numerics;

const int SERIAL = 9435;
const int MAXSIZE = 300;
//const int SERIAL = 42;
//const int MAXSIZE = 3;

Complex[] grid(Complex topLeft, int size) => Enumerable.Range(0, size).SelectMany(r => Enumerable.Range(0, size).Select(c => topLeft + new Complex(r, c))).ToArray();
Complex[] column(Complex start, int size) => Enumerable.Range(0, size).Select(y => start + new Complex(0, y)).ToArray();
Complex[] row(Complex start, int size) => Enumerable.Range(0, size).Select(x => start + new Complex(x, 0)).ToArray();

double power(Complex centre)
{
    var rackId = centre.Real + 10;
    var power = rackId * centre.Imaginary;
    power += SERIAL;
    power *= rackId;
    power %= 1000;
    power = Math.Floor(power / 100);
    power -= 5;
    return power;
}

double maxPower = double.MinValue;
Dictionary<(int x, int y, int size), double> cache = new ();

(int maxx, int maxy, int maxsize) = (-1, -1, -1);
for (int size = 1; size <= MAXSIZE; size++)
{
    for (int y = 1; y <= 300 - size + 1; y++)
    {
        for (int x = 1; x <= 300 - size + 1; x++)
        {
            double total = 0;
            if (size > 1)
            {
                total = cache[(x, y, size - 1)];
            }
            //slow, should really reuse sub-results better
            var cTotal = column(new Complex(x + size - 1, y), size).Sum(power);
            total += cTotal;
            var rTotal = row(new Complex(x, y + size - 1), size).Sum(power);
            total += rTotal;
            var corner = power(new Complex(x + size - 1, y + size - 1));
            total -= corner;
            if (total > maxPower)
            {
                maxPower = total;
                (maxx, maxy, maxsize) = (x, y, size);
            }
            cache[(x, y, size)] = total;
        }
    }
    Console.WriteLine($"Checked size: {size}");
}

(var part1X, var part1Y) = (maxx, maxy);
Console.WriteLine($"Part1: ({part1X},{part1Y},{maxsize})");




