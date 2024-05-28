namespace Algorithms.Helpers;

public struct Coordinate(double x, double y)
{
    public double X { get; set; } = x;
    public double Y { get; set; } = y;

    public static double Distance_Between_Two_Points(Coordinate a, Coordinate b)
        => Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    public static double Distance_Between_Two_Points(double x, double y, double x1, double y1)
    => Math.Sqrt(Math.Pow(x - x1, 2) + Math.Pow(y - y1, 2));
}