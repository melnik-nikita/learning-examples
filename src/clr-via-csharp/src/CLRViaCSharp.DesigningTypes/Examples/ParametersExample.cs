using CLRViaCSharp.DesigningTypes.Interfaces;

namespace CLRViaCSharp.DesigningTypes.Examples;

public class ParametersExample : IExample
{
    private static Int32 _number = 0;

    /// <inheritdoc />
    public static void Run()
    {
        Console.WriteLine("Parameters example start");
        // Same as: OptionalAndNamedParameters(9, "A", new DateTime(), new Guid());
        OptionalAndNamedParameters();
        // Same as: OptionalAndNamedParameters(8, "X", new DateTime(), new Guid());
        OptionalAndNamedParameters(8, "X");
        // Same as:  OptionalAndNamedParameters(5, "A", DateTime.Now, Guid.NewGuid())
        OptionalAndNamedParameters(5, guid: Guid.NewGuid(), dt: DateTime.Now);
        // Same as: OptionalAndNamedParameters(0, "1", DateTime.Now, Guid.NewGuid())
        OptionalAndNamedParameters(_number++, _number++.ToString());
        // Same as:
        // String t1 = "2"; Int32 t2 = 3;
        // OptionalAndNamedParameters(t2, t1, new DateTime(), new Guid())
        OptionalAndNamedParameters(s: (_number++).ToString(), x: _number++);

        ImplicitlyTypedLocalVariables();

        Int32 x;
        GetVal(out x);
        Console.WriteLine(x);
        AddVal(ref x);
        Console.WriteLine(x);

        var result = Add(new Int32[] { 1, 2, 3, 4, 5 });
        Console.WriteLine($"Total is: {result}");

        Console.WriteLine($"Total is: {Add(1, 2, 3, 4, 5)}");

        Console.WriteLine("Parameters example end");
    }

    private static void OptionalAndNamedParameters(Int32 x = 9, String s = "A", DateTime dt = default, Guid guid = new ())
    {
        Console.WriteLine($"x={x}, s={s}, dt={dt}, guid={guid}");
    }

    private static void ImplicitlyTypedLocalVariables()
    {
        var name = "Nick";
        ShowVariableType(name);

        // var n = null; // Error: Cannot assign 'null' to an implicitly-typed local variable
        var x = (string)null;
        ShowVariableType(x);

        var numbers = new Int32[] { 1, 2, 3, 4 };
        ShowVariableType(numbers);

        var collection = new Dictionary<String, Single> { ["Grant"] = 4.0f };

        ShowVariableType(collection);

        foreach (var item in collection)
        {
            ShowVariableType(item);
        }
    }

    private static void ShowVariableType<T>(T t)
    {
        Console.WriteLine($"Variable type is: {typeof(T)}");
    }

    private static void GetVal(out Int32 value)
    {
        value = 10;
    }

    private static void AddVal(ref Int32 value)
    {
        value += 10;
    }

    private static Int32 Add(params Int32[] values)
    {
        var total = 0;

        if (values is not null)
        {
            foreach (var value in values)
            {
                total += value;
            }
        }

        return total;
    }
}
