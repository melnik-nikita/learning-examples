# [C# 6](../../README.md)

1. [Super-sleek properties and expression-bodied members](#super-sleek-properties-and-expression-bodied-members)
    - Upgrades to automatically implemented properties
    - Expression-bodied members
2. [Stringy features](#stringy-features)
    - A recap of string formatting in .NET
    - Introducing interpolated string literals
    - Localization using FormattableString
    - Uses, guidelines, and limitation
    - Accessing identifiers with nameof

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

## Stringy features

### A recap of string formatting in .NET

```csharp
Console.Write("What's your name? ");
stirng name = Console.ReadLine();
Console.WriteLine("Hello, {0}!", name);
```

### Introducing interpolated string literals

```csharp
Console.Write("What's your name? ");
stirng name = Console.ReadLine();
Console.WriteLine($"Hello, {name}!"); // interpolated string

decimal price = 95.25m;
decimal tip = price * 0.2m;
// Right-justify prices using nine-digit alignment
Console.WriteLine($"Price: {price,9:C}");
Console.WriteLine($"Tip: {tip,9:C}");
Console.WriteLine($"Total: {price + tip,9:C}");
```

#### Verbatim string literals

They start with ___@___ before the double quote. Within a verbatim string literal, backslashes and line breaks are
included in the string.
Typically used for:

- Strings breaking over multiple lines
- Regular expressions (which use backslashes for escaping, quite separate from the escaping the C# compiler uses in
  regular string literal)
- Hardcoded Windows filenames

```csharp
string sql = @"
    SELECT City, ZipCode
    FROM Address
    WHERE Country = 'US'";
Regex lettersDotDigits = new Regex(@"[a-z]+\.\d+");
string file = @"c:users\skeet\Test\Test.cs";
```

Verbatim string literals also can be interpolated

```csharp
decimal price = 95.25m;
decimal tip = price * 0.2m;
// Right-justify prices using nine-digit alignment
Console.WriteLine(@$"Price: {price,9:C}
Tip: {tip,9:C}
Total: {price + tip,9:C}");
```

#### Compiler handling of interpolated string literals (part 1)

Compiler converts the interpolated string literal into a call to ___string.Format___, and it extracts the expressions
form the format items and passes them as arguments after the composite format string.

```csharp
int x = 10, y = 10;
string text = $"x={x}, y={y}"; // converted to string.Format("x={0}, y={1}", x, y);
```

### Localization using FormattableString

To perform formatting in a specific culture, you need three pieces of information:

- The composite format string, which includes the hardcoded text and the format items as placeholders for the real
  values
- The values themselves
- The culture you want to format the string in

The ___FormattableString___ class in the System namespace helps to specify the culture

```csharp
var dateOfBirth = new DateTime(1976, 6, 19);
FormattableString formattableString = $"Jon was born on {dateOfBirth:d}"; // Keeps the compoite format string and value in a FormattableString
var culture = CultureInfo.GetCultureInfo("en-US");
var result = formattableString.ToString(culture); // Format in the specified culture
```

#### Compiler handling of interpolated string literals (part 2)

When the compiler needs to convert an interpolated string literal into a ___FormattableString___, it performs most of
the same steps as for a conversion to ___string___. But instead of ___string.Format___, it calls the static ___Create___
method on the ___FormattableStringFactory___ class.

```csharp
int x = 10, y = 20;
FormattableString = formattable = FormattableStringFactory.Create("x={0}, y={1}, x, y);
```

### Uses, guidelines, and limitation

Should be used:

- Developers and machines, but maybe not end users
    - Machine readable strings
    - Messages for other developers
    - Messages for end users
- Hard limitation of interpolated string literals
    - No dynamic formatting
    - No expression reevaluation
    - No bare colons
- When you can but really shouldn't
    - Defer formatting for strings that may not be used
    - Format for readability

### Accessing identifiers with nameof

___nameof___ operator takes an expression referring to a member or local variable, and the result is a compile-time
constant string with the simple name for that member or variable.

Common uses of ___nameof___:

- Argument validation
    ```csharp
    public void Test(string someArgument) {
        Preconditions.CheckNotNull(someArgument, nameof(someArgument));
    }
    ```
- Property change notification for computed properties
    ```csharp
    public class Rectangle : INotifyPropertyChanged
    {
        private double weight, height;
  
        public event PropertyChangedEventHandler PropertyChanged;
  
        public double Width
        {
            get { return width; }
            set
            {
                if (width == value) return;
                
                width = value;
                RaisePropertyChanged(); // Raises the event for the Width property
                RisePropertyChanged(nameof(Area)); // Raises the event for the Area property
            }
        }
        public double Height { ... }
        public double Area => Width * Height;
  
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null) { ... }
    }
    ```
- Attributes
    ```csharp
    public class Rectangle : INotifyPropertyChanged
    {
        ...
        [DerivedProperty(nameof(Area))
        public double Width { ... }
        ...
    }
    ```

[Back to top ⇧](#c-6)
[Back to top ⇧](#c-6)