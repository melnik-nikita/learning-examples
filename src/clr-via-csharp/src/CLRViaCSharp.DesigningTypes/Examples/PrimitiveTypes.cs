using CLRViaCSharp.DesigningTypes.Interfaces;

#pragma warning disable CS0219

namespace CLRViaCSharp.DesigningTypes.Examples;

internal sealed class PrimitiveTypes : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
        sbyte sb = 0; // System.SByte
        byte b = 0; // System.Byte
        short s = 0; // System.Int16
        ushort us = 0; // System.UInt16
        var i = 0; // System.Int32
        uint ui = 0; // System.UInt32
        long l = 0; // System.Int64
        ulong ul = 0; // System.UInt64
        var c = '0'; // System.Char
        var f = 0.0f; // System.Single
        var d = 0.0; // System.Double
        var boolean = false; // System.Boolean
        var dDecimal = 1.0000M; // System.Decimal
        var str = "string"; // System.String
        object obj = new (); // System.Object
        dynamic dyn = new Object(); // System.Object
    }
}
