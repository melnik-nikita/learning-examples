using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.Threading.Examples;

public class PrimitiveThreadSynchronizationConstructs : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
    }
}

internal sealed class ThreadsSharingData
{
    private Int32 _flag = 0;
    private Int32 _value = 0;

    // This method is executed by one thread
    public void Thread1()
    {
        // Note: 5 must be written to _value before 1 is written to _flag
        _value = 5;
        Volatile.Write(ref _flag, 1);
    }

    // This method is executed by another thread
    public void Thread2()
    {
        // Note: _value must be read after _flag is read
        if (Volatile.Read(ref _flag) == 1)
        {
            Console.WriteLine(_value);
        }
    }
}

internal sealed class ThreadsSharingDataUsigVolatileKeyword
{
    private volatile Int32 _flag = 0;
    private Int32 _value = 0;

    // This method is executed by one thread
    public void Thread1()
    {
        // Note: 5 must be written to _value before 1 is written to _flag
        _value = 5;
        _flag = 1;
    }

    // This method is executed by another thread
    public void Thread2()
    {
        // Note: _value must be read after _flag is read
        if (_flag == 1)
        {
            Console.WriteLine(_value);
        }
    }
}
