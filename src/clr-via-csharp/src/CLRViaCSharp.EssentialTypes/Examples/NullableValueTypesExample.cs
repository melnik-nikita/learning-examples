namespace CLRViaCSharp.EssentialTypes.Examples;

internal class NullableValueTypesExample
{
    public static void Run()
    {
        Console.WriteLine("Nullable Value Types example start");
        int? x = null;
        Nullable<int> y = null; // same as int? y = new int?();

        ConversionsAndCasting();
        Operators();
        NullCoalescingOperator();
        BoxingNullableValueTypes();
        Console.WriteLine("Nullable Value Types example end");
    }

    private static void ConversionsAndCasting()
    {
        // Implicit conversion from Int32 to Nullable<Int32>
        int? a = 5;

        // Implicit conversion from 'null' to Nullable<Int32>
        int? b = null;

        // Explicit conversion from Nullable<Int32> to non-nullable Int32
        var c = (int)a;
        // Casting between nullable primitive types
        Double? d = 5;
        Double? e = b;
    }

    // @formatter:off
    private static void Operators()
    {
        Int32? a = 5;
        Int32? b = null;

        // Unary operators
        a++; // a = 6
        b = -b; // b = null

        a = a + 3; // a = 9
        b = b * 3; // b = null

        // Equality operators
        if (a == null) { /* no */ } else { /* yes */ }
        if (b == null) { /* yes */ } else { /* no */ }
        if (a != b) { /* yes */ } else { /* no */ }

        // Comparison operators
        if (a < b) { /* no */ } else { /* yes */ }
    }
    // @formatter:on

    private static void NullCoalescingOperator()
    {
        Int32? b = null;

        var x = b ?? 123;
        Console.WriteLine(x);

        var filename = GetFileName() ?? "Untitled";
    }

    private static void BoxingNullableValueTypes()
    {
        Int32? n = null;
        Object o = n;
        Console.WriteLine("o is null={0}", o == null); // True

        n = 5;
        o = n;
        Console.WriteLine("o's type={0}", o.GetType()); // System.Int32
    }

    // @formatter:off
    private static void UnboxingNullableValueTypes()
    {
        Object o = 5;

        Int32? a = (int?)o;
        Int32 b = (int)o;
        o = null;

        a = (int?)o;
        // b = (int)o; // NullReferenceException
    }
    // @formatter:on

    private static string GetFileName()
    {
        return null;
    }
}
