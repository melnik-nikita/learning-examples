using System.Data.SqlClient;
using System.Runtime.InteropServices;
using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.CoreFacilities.Examples;

public class GarbageCollection : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Garbage collection example start");
        Console.WriteLine("Garbage collection example end");
        Console.WriteLine("---------------------------");
    }
}

public class Resource : IDisposable
{
    private bool _isDisposed;
    private IntPtr _nativeResource = Marshal.AllocHGlobal(100);
    private readonly SqlConnection _sqlConnection;

    public Resource(string connectionString)
    {
        _sqlConnection = new SqlConnection(connectionString);
    }

    // Dispose() calls Dispose(true)
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // The bulk of the clean-up code is implemented in Dispose(bool)
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            // free managed resources
            _sqlConnection.Dispose();
        }

        // free native resources if there are any.
        if (_nativeResource != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(_nativeResource);
            _nativeResource = IntPtr.Zero;
        }

        _isDisposed = true;
    }

    // NOTE: Leave out the finalizer altogether if this class doesn't
    // own unmanaged resources, but leave the other methods
    // exactly as they are.
    ~Resource()
    {
        // Finalizer calls Dispose(false)
        Dispose(false);
    }
}
