namespace CLRViaCSharp.DesigningTypes.Examples;

public class TypeAndTypeMembers
{
    // Nested class
    private class SomeNestedType
    {
    }

    // Constant, read-only and static read/write field
    private const Int32 SomConstant = 1;
    private readonly String _someReadOnlyField = "2";
    private static Int32 SomeReadWriteField = 3;

    // Type ctor
    static TypeAndTypeMembers()
    {
    }

    // Instance ctors
    public TypeAndTypeMembers(Int32 x) { }
    public TypeAndTypeMembers() { }

    // Instance and static methods
    private String InstanceMethod() { return null; }
    public static void Main() { }

    // Instance Property
    public Int32 SomeProp
    {
        get => 0;
        set { }
    }

    // Instance parameterful property (indexer)
    public Int32 this[String s]
    {
        get => 0;
        set { }
    }

    // Instance event
    public event EventHandler SomeEvent;
}
