# [C# 6](../../README.md)

1. [Super-sleek properties and expression-bodied members](#super-sleek-properties-and-expression-bodied-members)

## Super-sleek properties and expression-bodied members

### Automatically implemented properties

Automatically implemented read-write property

```csharp
public class Person
{
    public List<Person> Friends { get; set; } = new ();
}
```

Automatically implemented read-only property

```csharp
public class Person
{
    public List<Person> Friends { get; } = new ();
}
```

Before C#6

```csharp
public struct Point
{
    public doulble X { get; private set; }
    public doulble Y { get; private set; }
    
    public Point(double X, double Y) : this()
    {
        X = x;
        Y = y;
    }
}
```

In C# 6:

- You're allowed to set an automatically implemented property before all the fields are initialized.
- Setting an automatically implemented property counts as initializing the field.
- You're allowed to read an automatically implemented property before other fields are initialized, so long as you've
  set it beforehand.

Another way of thinking of this is that within the constructor, automatically implemented properties are treated as if
they're fields.

```csharp
public struct Point
{
    public doulble X { get; }
    public doulble Y { get; }
    
    public Point(double X, double Y)
    {
        X = x;
        Y = y;
    }
}
```

### Expression-bodied members

```csharp
public struct Point
{
    ...
    
    // Read-only property to compute the distance
    public double DistanceFromOrigin
    {
        get { return Math.sqrt(X * X + Y * Y); }
    }
}
```

#### Expression-bodied member in C#6:

```csharp
public struct Point
{
    ...
        
    // Expression-bodied member
    public double DistanceFromOrigin => Math.sqrt(X * X + Y * Y);
}
```

#### Pass-through or delegation properties

```csharp
public struct LocalDateTime
{
    // Property for the date component
    public LocalDate Date { get; }
    // Properties delegating to date subcomponents
    public int Year => Date.Year;
    public int Month => Date.Month;
    public int Day => Date.Day;
    // Property for the time component
    public LocalTime TimeOfDay { get; }
    // Properties delegating to time subcomponents
    public int Hour => TimeOfDay.Hour;
    public int Minute => TimeOfDay.Minute;
    public int Second => TimeOfDay.Second;
    
    ... // Initialization, other properties, and members
}
```

#### Expression-bodied methods, indexers, and operators

- Before C#6:

```csharp
public static Point Add(Point left, Vector right)
{
    return left + right;
}

public static Point operator+(Point left, Vector right)
{
    return new Point(left.X + right.X, left.Y + right.Y);
}
```

- After C#6 expression-bodied methods and operators:

```csharp
public static Point Add(Point left, Vector right)
    => left + right;

public static Point operator+(Point left, Vector right)
    => new Point(left.X + right.X, left.Y + right.Y);
```

- Expression-bodied indexers

```csharp
public sealed class ReadOnlyListView<T> : IReadOnlyList<T>
{
    private readonly IList<T> list;
    public ReadOnlyListView(IList<T> list)
    {
        this.list = list;
    }
    // Indexer delegating to list indexer
    public T this[int index] => list[index];
    // Property delegating to list property
    public int Count => list.Count;
    // Method delegating to list method;
    public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
    // Method delegating to the other GetEnumerator method
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

#### Restrictions on expression-bodied members in C# 6

In C# 6 you can't have expression-bodied:

- Static Constructors
- Finalizers
- Instance constructors
- Read/write or write-only properties
- Read/write or write-only indexers
- Events

#### Guidelines for using expression-bodied members

#### Summary

- Automatically implemented properties can now be read-only and acked by a read-only field.
- Automatically implemented properties can now have initializers rather than nondefault values having to be initialized
  in a constructor.
- Structs can use automatically implemented properties without having to chain constructors together.
- Expression-bodied members allow simple (single-expression) code to be written with less ceremony.
- Although restrictions limit the kinds of members that can be written with expression bodies in C# 6, those
  restrictions are lifted in C# 7

[Back to top ⇧](#c-6)

