using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.DesigningTypes.Examples;

public class InterfacesExample : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
        Console.WriteLine("Interfaces example start");

        Point[] points = new[] { new Point(3, 3), new (1, 2) };
        if (points[0].CompareTo(points[1]) > 0)
        {
            var tempPoint = points[0];
            points[0] = points[1];
            points[1] = tempPoint;
        }

        Console.WriteLine("Points from closest to (0,0) to farthest:");
        foreach (var point in points)
        {
            Console.WriteLine(point);
        }

        // Base dispose
        var b = new Base();
        b.Dispose();
        ((IDisposable)b).Dispose();

        // Derived dispose
        var d = new Derived();
        d.Dispose();
        ((IDisposable)d).Dispose();

        b = new Derived();
        // Base dispose
        b.Dispose();
        // Derived dispose
        ((IDisposable)b).Dispose();

        Console.WriteLine("Interfaces example end");
    }
}

file sealed class Point : IComparable<Point>
{
    private Int32 _x;
    private Int32 _y;

    public Point(int x, int y)
    {
        _x = x;
        _y = y;
    }

    /// <inheritdoc />
    public int CompareTo(Point? other)
    {
        if (other is null)
        {
            return -1;
        }

        return Math.Sign(Math.Sqrt((_x * _x) + (_y * _y)) - Math.Sqrt((other._x * other._x) + (other._y * other._y)));
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"({_x}, {_y})";
    }
}

// Implements IDisposable
internal class Base : IDisposable
{
    // Is implicitly sealed and cannot be overridden
    public void Dispose()
    {
        Console.WriteLine("Base's Dispose");
    }
}

// Is derived from Base and it re-implements IDisposable
internal class Derived : Base, IDisposable
{
    // Cannot override Base's Dispose. 'new' is used to indicate
    // that this method-re-implements IDisposable's Dispose method
    public new void Dispose()
    {
        Console.WriteLine("Derived's Dispose");
    }
}
