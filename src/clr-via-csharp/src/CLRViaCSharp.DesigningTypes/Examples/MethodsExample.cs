using System.Text;
using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.DesigningTypes.Examples;

public class MethodsExample : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
        Console.WriteLine("Methods example start");

        // Implicit cast
        Rational r1 = 5;
        Rational r2 = 2.5f;
        // Explicit cast
        var x = (Int32)r1;
        var s = (Single)r2;

        // Extension methods
        var sb = new StringBuilder("Hello. My name is Nick.");
        var index = sb.Replace('.', '!').IndexOf('!');

        Console.WriteLine("Methods example start");
    }
}

file sealed class SomeType
{
    private Int32 x = 5;

    public Int32 X => x;
}

file struct Point
{
    private Int32 x = 5;
    private Int32 y;

    public Point()
    {
        y = 0;
    }

    public Int32 X => x;
    public Int32 Y => y;
}

file sealed class SomeRefType
{
    static SomeRefType()
    {
        // this executes the first time a SomeRefType is accessed
    }
}

file struct SomeValType
{
    // C# allows to define parameterless type ctors for value types
    static SomeValType()
    {
        // this executes the first time a SomeValType is accessed
    }
}

// Operator overload
file sealed class Complex
{
    public Int32 X { get; set; }

    public static Complex operator +(Complex c1, Complex c2)
    {
        return new Complex() { X = c1.X + c2.X };
    }
}

// Conversion operator
file sealed class Rational
{
    private readonly float _num;

    public Rational(Int32 num)
    {
        _num = num;
    }

    public Rational(Single num)
    {
        _num = num;
    }

    public Single ToSingle()
    {
        return _num;
    }

    public Int32 ToInt32()
    {
        return (int)_num;
    }

    public static implicit operator Rational(Int32 num)
    {
        return new Rational(num);
    }

    public static implicit operator Rational(Single num)
    {
        return new Rational(num);
    }

    public static explicit operator Int32(Rational r)
    {
        return r.ToInt32();
    }

    public static explicit operator Single(Rational r)
    {
        return r.ToSingle();
    }
}

// Extension methods
file static class StringBuilderExtensions
{
    public static Int32 IndexOf(this StringBuilder sb, Char value)
    {
        for (var index = 0; index < sb.Length; index++)
        {
            if (sb[index] == value)
            {
                return index;
            }
        }

        return -1;
    }
}

// Partial methods
file sealed partial class Base
{
    private String _name;

    partial void OnNameChanging(String value);

    public String Name
    {
        get => _name;
        set
        {
            OnNameChanging(value.ToUpper());
            _name = value;
        }
    }
}

file sealed partial class Base
{
    partial void OnNameChanging(string value)
    {
        if (String.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }
    }
}
