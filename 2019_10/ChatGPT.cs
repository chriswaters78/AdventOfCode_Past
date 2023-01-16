using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ChatGPT
{
    public static List<((double x, double y), double angle)> MeasureAngles(List<(double x, double y)> points) =>
        points.Skip(1)
        .Select(point => Math.Atan2(point.y - points[0].y, point.x - points[0].x))
        .Zip(points.Skip(1), (angle, point) => (point, angle)).ToList();
}
