namespace CLRViaCSharp.EssentialTypes.Examples;

internal static class CharactersExample
{
    public static void Run()
    {
        Console.WriteLine("Characters example start");

        Char c;
        Int32 n;

        c = (Char)65;
        Console.WriteLine(c); // Displays 'A'

        n = (Int32)c;
        Console.WriteLine(n); // Displays '65'

        c = unchecked((Char)(65536 + 65));
        Console.WriteLine(c); // Displays 'A'

        c = Convert.ToChar(65);
        Console.WriteLine(c); // Displays 'A'

        n = Convert.ToInt32(c);
        Console.WriteLine(c); // Displays '65'

        try
        {
            c = Convert.ToChar(70000);
            Console.WriteLine(c);
        }
        catch (OverflowException)
        {
            Console.WriteLine("Can't convert 70000 to a char");
        }

        c = ((IConvertible)65).ToChar(null);
        Console.WriteLine(c); // Displays 'A'

        n = ((IConvertible)c).ToInt32(null);
        Console.WriteLine(n); // Displays '65'

        Console.WriteLine("Characters example end");
    }
}
