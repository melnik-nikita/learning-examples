using System.Reflection;

namespace CLRViaCSharp.EssentialTypes.Examples;

internal static class EnumeratedTypesExample
{
    public static void Run()
    {
        Console.WriteLine("Enumerated Types example start");

        EnumExamples();
        EnumFlagsExamples();

        Console.WriteLine("Enumerated Types example end");
    }

    private static void EnumExamples()
    {
        Console.WriteLine("Underlying type is:");
        Console.WriteLine(Enum.GetUnderlyingType(typeof(Color)));
        Console.WriteLine("------------------------------------");

        var color = Color.Blue;

        Console.WriteLine(color);
        Console.WriteLine(color.ToString());
        Console.WriteLine(color.ToString("G"));
        Console.WriteLine(color.ToString("D"));
        Console.WriteLine(color.ToString("X"));
        Console.WriteLine("------------------------------------");

        var colors = Enum.GetValues<Color>();
        Console.WriteLine($"Number of symbols defined: {colors.Length}");
        Console.WriteLine($"Value\tSymbol{Environment.NewLine}-----\t------");
        foreach (var col in colors)
        {
            Console.WriteLine("{0,5:D}\t{0:G}", col);
        }

        Console.WriteLine("------------------------------------");

        var c = Enum.Parse<Color>("orange", true);
        // c = Enum.Parse<Color>("Brown", true); // Argument exception, brown is not defined
        Enum.TryParse<Color>("1", false, out c);
        Enum.TryParse<Color>("23", false, out c);
        Console.WriteLine(Enum.IsDefined(typeof(Color), (Byte)1)); // true
        Console.WriteLine(Enum.IsDefined(typeof(Color), "White")); // true
        Console.WriteLine(Enum.IsDefined(typeof(Color), "white")); // false IsDefined is case sensitive
        Console.WriteLine(Enum.IsDefined(typeof(Color), (Byte)10)); // false
        Console.WriteLine("------------------------------------");
    }

    private static void EnumFlagsExamples()
    {
        // Check file attributes
        Console.WriteLine("Determine if file is hidden");
        var file = Assembly.GetEntryAssembly().Location;
        var attributes = File.GetAttributes(file);
        Console.WriteLine("IS {0} hidden? {1}", file, (attributes & FileAttributes.Hidden) != 0);

        // Change file attributes
        // File.SetAttributes(file, FileAttributes.ReadOnly | FileAttributes.Hidden);

        var actions = Actions.Read | Actions.Delete;
        Console.WriteLine($"Actions: {actions.ToString()}"); // Read, Delete
        Console.WriteLine("------------------------------------");

        var fa = FileAttributes.System;
        fa = fa.Set(FileAttributes.ReadOnly);
        fa = fa.Clear(FileAttributes.System);
        Console.WriteLine("List of flags");
        fa.ForEach(f => Console.WriteLine(f));
        Console.WriteLine("------------------------------------");
    }
}

internal static class FileAttributesExtensions
{
    public static Boolean IsSet(this FileAttributes flags, FileAttributes flagToTest)
    {
        if (flagToTest == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(flagToTest), "Value must not be 0");
        }

        return (flags & flagToTest) == flagToTest;
    }

    public static Boolean IsClear(this FileAttributes flags, FileAttributes flagToTest)
    {
        if (flagToTest == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(flagToTest), "Value must not be 0");
        }

        return !IsSet(flags, flagToTest);
    }

    public static Boolean AnyFlagsSet(this FileAttributes flags, FileAttributes testFlags)
    {
        return (flags & testFlags) != 0;
    }

    public static FileAttributes Set(this FileAttributes flags, FileAttributes setFlags)
    {
        return flags | setFlags;
    }

    public static FileAttributes Clear(this FileAttributes flags, FileAttributes clearFlags)
    {
        return flags & ~clearFlags;
    }

    public static void ForEach(this FileAttributes flags, Action<FileAttributes> processFlag)
    {
        if (processFlag is null)
        {
            throw new ArgumentNullException(nameof(processFlag));
        }

        for (UInt32 bit = 1; bit != 0; bit <<= 1)
        {
            var temp = (UInt32)flags & bit;
            if (temp != 0)
            {
                processFlag((FileAttributes)temp);
            }
        }
    }
}

[Flags]
internal enum Actions
{
    None = 0,
    Read = 0x0001,
    Write = 0x0002,
    ReadWrite = Read | Write,
    Delete = 0x0004,
    Query = 0x0008,
    Sync = 0x0010
}

// [Flags] [Serializable]
// internal enum FileAttributes
// {
//     ReadOnly = 0x00001,
//     Hidden = 0x00002,
//     System = 0x00004,
//     Directory = 0x00010,
//     Archive = 0x00020,
//     Device = 0x00040,
//     Normal = 0x00080,
//     Temporary = 0x00100,
//     SparseFile = 0x00200,
//     ReparsePoint = 0x00400,
//     Compressed = 0x00800,
//     Offline = 0x01000,
//     NotContentIndexed = 0x02000,
//     Encrypted = 0x04000,
//     IntegrityStream = 0x08000,
//     NoScrubData = 0x20000
// }

internal enum Color : byte
{
    White,
    Red,
    Green,
    Blue,
    Orange
}

internal enum ColorInt
{
    White,
    Red,
    Green,
    Blue,
    Orange
}
