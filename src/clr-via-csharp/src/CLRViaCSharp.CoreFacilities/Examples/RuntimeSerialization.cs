using System.Runtime.Serialization;
using System.Text.Json;
using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.CoreFacilities.Examples;

internal class RuntimeSerialization : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Runtime Serialization example start");

        var pt = new Point { X = 1, Y = 2 };
        OptInSerialization(pt);

        var type = new MyType(3, 4);
        OptInSerialization(type);

        Console.WriteLine("Runtime Serialization example end");
        Console.WriteLine("---------------------------");
    }

    private static void OptInSerialization<T>(T @object)
    {
        var serializerOptions = new JsonSerializerOptions { IncludeFields = true };
        using var stream = new MemoryStream();
        JsonSerializer.Serialize(stream, @object, serializerOptions);

        stream.Position = 0;

        var obj = JsonSerializer.Deserialize<T>(stream, serializerOptions);
        Console.WriteLine(obj?.ToString());
    }
}

// [Serializable] - seems like all types are serializable by default
public struct Point
{
    public Int32 X { get; set; }
    public Int32 Y; // { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{{ X: {X}, Y: {Y} }}";
    }
}

[Serializable]
public class MyType
{
    public Int32 x, y;

    // NonSerialized - Obsolete?
    [NonSerialized] public Int32 sum;

    public MyType()
    {
    }

    public MyType(Int32 x, Int32 y)
    {
        this.x = x;
        this.y = y;
        sum = x + y;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{{ X: {x}, Y: {y}, Sum: {sum} }}";
    }

    // Below methods does not work with JsonSerializer
    [OnDeserializing]
    private void OnDeserializing(StreamingContext context)
    {
        Console.WriteLine("OnDeserializing");
    }

    [OnDeserialized]
    private void OnDeserialized(StreamingContext context)
    {
        Console.WriteLine("OnDeserialized");

        sum = x + y;
    }

    [OnSerializing]
    private void OnSerializing(StreamingContext context)
    {
        Console.WriteLine("OnSerializing");
    }

    [OnSerialized]
    private void OnSerialized(StreamingContext context)
    {
        Console.WriteLine("OnSerialized");
    }
}
