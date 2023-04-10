using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.Threading.Examples;

internal class ThreadBasics : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
        // DedicatedThread.CreateADedicatedThread();
        BackgroundVsForeground.Run();
    }
}

internal static class DedicatedThread
{
    public static void CreateADedicatedThread()
    {
        Console.WriteLine("Main thread: starting a dedicated thread to do an asynchronous operation");
        var dedicatedThread = new Thread(ComputeBoundOp);
        dedicatedThread.Start(5);
        Console.WriteLine("Main thread: doing other work here...");
        Thread.Sleep(10000); // Simulating other work (10 seconds)
        dedicatedThread.Join(); // Wait for thread to terminate
        Console.WriteLine("Hit <Enter> to end this program");
        Console.ReadLine();
    }

    // This method's signature must match the PaarameterizedThreadStart delegate
    private static void ComputeBoundOp(object? state)
    {
        // This method is executed by a dedicated thread
        Console.WriteLine("In ComputeBoundOp: state={0}", state);
        Thread.Sleep(1000); // Simulates other work (1 second)
        // When this method returns, the dedicated thread dies
    }
}

internal static class BackgroundVsForeground
{
    public static void Run()
    {
        // Create a new thread (defaults to foreground)
        var t = new Thread(Worker);
        // Make the thread a background thread
        t.IsBackground = true;

        // Start the thread
        t.Start();
        // If t is a foreground thread, the application won't die fro about 10 seconds
        // If t s a background thread, the application dies immediately
        Console.WriteLine("returning from Run method");
    }

    private static void Worker()
    {
        Thread.Sleep(10000);
        Console.WriteLine("Returning from Worker");
    }
}
