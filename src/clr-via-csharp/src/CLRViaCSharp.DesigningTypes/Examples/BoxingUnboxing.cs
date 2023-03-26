using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.DesigningTypes.Examples;

internal sealed class BoxingUnboxing : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
        Console.WriteLine("Boxing/Unboxing example start");

        BoxingUnboxingValueTypes();
        ChangingFieldsInABoxedValueType();

        Console.WriteLine("Boxing/Unboxing example end");
    }

    private static void BoxingUnboxingValueTypes()
    {
        Console.WriteLine(nameof(BoxingUnboxingValueTypes));
        // Create point instances
        var p1 = new Point(1, 1);
        var p2 = new Point(2, 2);

        // p1 does not get boxed
        Console.WriteLine(p1.ToString());
        // p1 does not get boxed to call compareTo
        // p2 does not get boxed because overload method is called
        Console.WriteLine(p1.CompareTo(p2));

        // p1 DOES get boxed and the reference is placed in c
        IComparable c = p1;
        Console.WriteLine(c.GetType());

        // p1 does NOT get boxed to call CompareTo
        // Because CompareTo is not being passed a Point variable.
        // CompareTo(Object) is called which requires a reference to a boxed Point.
        // c does NOT get boxed because it already refers to a boxed Point.
        Console.WriteLine(p1.CompareTo(c)); // 0

        // c does not get boxed because it already refers to a boxed Point.
        // p2 does get boxed because CompareTo(Object) is called.
        Console.WriteLine(c.CompareTo(p2)); // -1

        // c is unboxed and fields are copied into p2
        p2 = (Point)c;

        // Proved that the fields got copied into p2.
        Console.WriteLine(p2.ToString()); // "(1, 1)"
    }

    private static void ChangingFieldsInABoxedValueType()
    {
        Console.WriteLine(nameof(ChangingFieldsInABoxedValueType));

        var p = new Point(1, 1);

        // P is boxed before passing to WriteLine method WriteLine(object? value) signature is used
        Console.WriteLine(p); // (1, 1)

        p.Change(2, 2);
        Console.WriteLine(p); // (2, 2)

        Object o = p;
        Console.WriteLine(0); // (2, 2)

        // Casting to a Point unboxes o and copies the fields in the boxed Point to a temp Point on the thread's stack!
        // The values of the boxed Point are not affected by the call of Change method
        ((Point)o).Change(3, 3);
        Console.WriteLine(o); // (2, 2)

        // Value of p is boxed when casted to an Interface
        // Change is called on a boxed value, and changes fields of a boxed value
        // but after Change returns, the boxed object is immediately ready to be garbage collected
        // and the value of 'p' is unchanged
        ((IChangeBoxedPoint)p).Change(4, 4);
        Console.WriteLine(p); // (2, 2);

        // Changes the boxed object and shows it
        // Because o is a reference and casting to an Interface is still a reference
        // the value of boxed object is updated
        ((IChangeBoxedPoint)o).Change(5, 5);
        Console.WriteLine(o); // (5, 5);
    }
}

file struct Point : IComparable, IChangeBoxedPoint
{
    private Int32 m_x, m_y;

    public Point(int mX, int mY)
    {
        m_x = mX;
        m_y = mY;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"({m_x.ToString()}, {m_y.ToString()})";
    }

    /// <inheritdoc />
    public void Change(int x, int y)
    {
        m_x = x;
        m_y = y;
    }

    public int CompareTo(Point other)
    {
        return Math.Sign(Math.Sqrt((m_x * m_x) + (m_y * m_y)) - Math.Sqrt((other.m_x * other.m_x) + (other.m_y * other.m_y)));
    }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        if (GetType() != obj.GetType())
        {
            throw new ArgumentException("obj is not a Point");
        }

        return CompareTo((Point)obj);
    }
}

file interface IChangeBoxedPoint
{
    void Change(Int32 x, Int32 y);
}
