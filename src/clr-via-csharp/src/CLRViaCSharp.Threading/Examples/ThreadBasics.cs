using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.Threading.Examples;

internal class ThreadBasics : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
        // DedicatedThread.CreateADedicatedThread();
        // BackgroundVsForeground.Run();
        // ComputeBoundAsyncOperations.RunThreadPoolExample();
        // CancellationDemo.Run();
        // TaskExample();
        TaskCancellationExample();
    }

    private static void TaskExample()
    {
        var sum = (Int32 n) =>
        {
            var sum = 0;
            for (; n > 0; n--)
            {
                checked { sum += n; }
            }

            return sum;
        };

        // Create a Task (it does not start running now)
        Task<Int32> task = new (n => sum((Int32)n), 100000000);

        // You can start the task sometime later
        task.Start();

        // Optionally, you can explicitly wait for the task to complete
        task.Wait(); // FYI: Overloads exist accepting timeout/CancellationToken

        // You can get the result (the Result property internally calls Wait)
        Console.WriteLine("The Sum is: {0}", task.Result);
    }

    private static void TaskCancellationExample()
    {
        var Sum = (Int32 n, CancellationToken cancellationToken) =>
        {
            var sum = 0;
            for (; n > 0; n--)
            {
                // The following line throws OperationCancelledException when Cancel
                // is called on the CancellationTokenSource referred to by the token
                cancellationToken.ThrowIfCancellationRequested();
                checked { sum += n; }
            }

            return sum;
        };

        CancellationTokenSource cts = new ();
        var task = Task.Run(() => Sum(10000, cts.Token), cts.Token);

        // Sometime later, cancel the CancellationTokenSource to cancel the Task
        cts.Cancel(); // THis is an asynchronous request, the Task may have completed already
        try
        {
            // If the task got canceled, Result will throw an AggregateException
            Console.WriteLine("The sum is: {0}", task.Result);
        }
        catch (AggregateException ex)
        {
            // Consider any OperationCanceledException objects as handled.
            // ANy other exceptions cause a new AggregateException containing
            // only the unhandled exceptions to be thrown

            ex.Handle(e => e is OperationCanceledException);

            // If all the exceptions were handled, the following executes
            Console.WriteLine("Sum was cancelled");
        }
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

    // This method's signature must match the ParameterizedThreadStart delegate
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
        Thread t = new (Worker);
        // Make the thread a background thread
        t.IsBackground = true;

        // Start the thread
        t.Start();
        // If t is a foreground thread, the application won't die for about 10 seconds
        // If t s a background thread, the application dies immediately
        Console.WriteLine("returning from Run method");
    }

    private static void Worker()
    {
        Thread.Sleep(10000);
        Console.WriteLine("Returning from Worker");
    }
}

internal static class ComputeBoundAsyncOperations
{
    public static void RunThreadPoolExample()
    {
        Console.WriteLine("Main thread: queuing an asynchronous operation");
        ThreadPool.QueueUserWorkItem(ComputeBoundOp, 5);
        Console.WriteLine("Main thread: doing other work here...");
        Thread.Sleep(10000); // Simulating other work on main thread
        Console.WriteLine("Hit <Enter> to end this program...");
        Console.ReadLine();
    }

    private static void ComputeBoundOp(Object state)
    {
        // This method is executed by a thread pool thread
        Console.WriteLine("In ComputeBoundOp: state={0}", state);

        Thread.Sleep(1000); // Simulates other work
        // When this method returns, the thread goes back
        // to the pool and waits for another task
    }
}

internal static class CancellationDemo
{
    public static void Run()
    {
        CancellationTokenSource cts = new ();
        // Pass the CancellationToken and the number-to-count-to into the operation
        ThreadPool.QueueUserWorkItem(o => Count(cts.Token, 1000));

        Console.WriteLine("Press <enter> to cancel the operation");
        Console.ReadLine();
        cts.Cancel(); // If Count returned already, Cancel has no effect on it
        // Cancel returns immediately, and the method continues running here
        Console.ReadLine();
    }

    private static void Count(CancellationToken cancellationToken, Int32 countTo)
    {
        for (var i = 0; i < countTo; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Count is cancelled");
                break;
            }

            Console.WriteLine(i);
            Thread.Sleep(200);
        }

        Console.WriteLine("Count is done");
    }
}
