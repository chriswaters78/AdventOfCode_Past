using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntCode
{
    public static class Printer
    {
        public static string PrintGridMap(Dictionary<(int x, int y), char> gridMap)
        {
            var minX = gridMap.Keys.Min(tp => tp.x);
            var maxX = gridMap.Keys.Max(tp => tp.x);
            var minY = gridMap.Keys.Min(tp => tp.y);
            var maxY = gridMap.Keys.Max(tp => tp.y);

            List<StringBuilder> sbs = new List<StringBuilder>();
            for (int y = minY; y <= maxY; y++)
            {
                sbs.Add(new StringBuilder((String.Join("", Enumerable.Repeat(' ', maxX - minX + 1)))));
            }

            foreach (var kvp in gridMap)
            {
                var y = kvp.Key.y - minY;
                sbs[y][kvp.Key.x - minX] = kvp.Value;
            }

            return String.Join(Environment.NewLine, sbs);
        }
        public static string PrintGridMap(Dictionary<(int x, int y), long> gridMap, Func<long, char> printer)
        {
            return PrintGridMap(gridMap.ToDictionary(kvp => kvp.Key, kvp => printer(kvp.Value)));
        }

    }
}
