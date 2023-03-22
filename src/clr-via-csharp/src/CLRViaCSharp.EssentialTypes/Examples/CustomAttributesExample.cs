using System.Runtime.InteropServices;

namespace CLRViaCSharp.EssentialTypes.Examples;

internal class CustomAttributesExample
{
    public static void Run()
    {
        Console.WriteLine("Custom Attributes example start");
        Console.WriteLine("------------------------------------");

        Console.WriteLine("------------------------------------");
        Console.WriteLine("Custom Attributes example end");
    }
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal sealed class OSVERSIONINFO
{
    private readonly uint OSVersionInfoSize = 0;
    private readonly uint MajorVersion = 0;
    private readonly uint MinorVersion = 0;
    private readonly uint BuildNumber = 0;
    private readonly uint PlatformId = 0;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public String CSDVersion = null;

    public OSVERSIONINFO()
    {
        OSVersionInfoSize = (UInt32)Marshal.SizeOf(this);
    }
}

internal sealed class MyClass
{
    [DllImport("Kernel32", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern Boolean GetVersionEx([In] [Out] OSVERSIONINFO ver);
}

[AttributeUsage(AttributeTargets.Enum, Inherited = false)]
public class FlagsAttribute : Attribute
{
    public FlagsAttribute()
    {
    }
}
