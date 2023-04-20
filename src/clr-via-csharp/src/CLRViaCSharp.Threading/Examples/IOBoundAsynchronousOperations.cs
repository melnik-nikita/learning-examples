using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.Threading.Examples;

public class IOBoundAsynchronousOperations : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
    }

    private static async Task<Type1> Method1Async()
    {
        return await Task.FromResult(new Type1());
    }

    private static async Task<Type2> Method2Async()
    {
        return await Task.FromResult(new Type2());
    }

    private static async Task<String> MyMethodAsync(Int32 argument)
    {
        var local = argument;
        try
        {
            var result1 = await Method1Async();
            for (var x = 0; x < 3; x++)
            {
                var result2 = await Method2Async();
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Catch");
        }
        finally
        {
            Console.WriteLine("Finally");
        }

        return "Done";
    }
}

internal sealed class Type1
{
}

internal sealed class Type2
{
}
