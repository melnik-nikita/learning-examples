using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.DesigningTypes.Examples;

public class GenericsExample : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
        Console.WriteLine("Generics example start");

        Node head = new TypedNode<Char>('.');
        head = new TypedNode<DateTime>(DateTime.Now, head);
        head = new TypedNode<String>("Today is ", head);
        Console.WriteLine(head.ToString());

        Console.WriteLine("Generics example end");
    }

    private static List<TBase> ConvertIList<T, TBase>(IList<T> list) where T : TBase
    {
        List<TBase> baseList = new (list.Count);
        for (var index = 0; index < list.Count; index++)
        {
            baseList.Add(list[index]);
        }

        return baseList;
    }
}

// Primary constraint
internal sealed class PrimaryConstraintOfStream<T> where T : Stream
{
    public void M(T stream)
    {
        stream.Close();
    }
}

internal sealed class PrimaryConstraintOfClass<T> where T : class
{
    public void M(T @class)
    {
        @class = null; // Allowed as T is a ref type
    }
}

// Constructor constraint
internal sealed class ConstructorConstraint<T> where T : new()
{
    public static T Factory()
    {
        return new T();
    }
}

// Example how to store different generic types in a same type
internal class Node
{
    protected Node _next;

    protected Node(Node next)
    {
        _next = next;
    }
}

internal sealed class TypedNode<T> : Node
{
    private T _data;

    public TypedNode(T data)
        : this(data, null)
    {
    }

    public TypedNode(T data, Node next)
        : base(next)
    {
        _data = data;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return _data.ToString() + (_next != null ? _next.ToString() : String.Empty);
    }
}
