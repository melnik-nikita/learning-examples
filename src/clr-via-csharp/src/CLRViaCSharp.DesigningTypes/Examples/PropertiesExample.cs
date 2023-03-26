using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.DesigningTypes.Examples;

internal sealed class PropertiesExample : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
        Console.WriteLine("Properties example start");

        var employee = new Employee { Age = 30, Name = "Name" };

        Console.WriteLine("Properties example end");
    }
}

file sealed class Employee
{
    // Automatically implemented property
    public String Name { get; set; }
    public Int32 Age { get; set; }
}

file sealed class EmployeeSourceImplementation
{
    private String _name { get; set; }
    private Int32 _age { get; set; }

    public String get_Name()
    {
        return _name;
    }

    public void set_Name(String value)
    {
        _name = value;
    }

    public Int32 get_Age()
    {
        return _age;
    }

    public void set_Age(Int32 value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value.ToString(), "The value must be greater than or equal to 0");
        }

        _age = value;
    }
}
