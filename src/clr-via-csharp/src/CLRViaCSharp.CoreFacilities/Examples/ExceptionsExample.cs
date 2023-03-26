using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.CoreFacilities.Examples;

internal class ExceptionsExample : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Exceptions example start");

        Console.WriteLine("Exceptions example end");
        Console.WriteLine("---------------------------");
    }

    private static void ReadData(String pathname)
    {
        FileStream fs = null;
        try
        {
            fs = new FileStream(pathname, FileMode.Open);
            // Process data in the file...
        }
        catch (IOException)
        {
            // Put code that recovers from IOException
        }
        finally
        {
            // Make sure that the file gets closed
            if (fs is not null)
            {
                fs.Close();
            }
        }
    }

    private static void ReThrowingExceptions()
    {
        try
        {
            // some code that throws exception
        }
        catch (Exception ex)
        {
            // CLR thinks this is where exception originated
            // CLR resets starting point for exception
            throw ex;
        }

        try
        {
            // some code that throws exception
        }
        catch (Exception ex)
        {
            // This has no effect on where CLR thinks the exception originated.
            // CLR does not reset the stack's starting point
            throw;
        }
    }
}
