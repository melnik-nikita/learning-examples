using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.Threading.Examples;

public class HybridThreadSynchronizationConstructs : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
    }
}

// A Simple Hybrid lock example
internal sealed class SimpleHybridLock : IDisposable
{
    // The Int32 is used by the primitive user-mode constructs (Interlocked methods)
    private Int32 _waiters = 0;

    // The AutoResetEvent is the primitive kernel-mode construct
    private readonly AutoResetEvent _waiterLock = new (false);

    public void Enter()
    {
        // Indicate that this thread wants the lock
        if (Interlocked.Increment(ref _waiters) == 1)
        {
            return; // Lock was free, no contention, just return
        }

        // Another thread was the lock (contention), make this thread wait
        _waiterLock.WaitOne(); // Bad performance hit here
    }

    public void Leave()
    {
        // This thread is releasing the lock
        if (Interlocked.Decrement(ref _waiters) == 0)
        {
            return; // No other threads are waiting, just return
        }

        // Other threads are waiting, wake 1 of them
        _waiterLock.Set(); // Bad performance hit here
    }

    /// <inheritdoc />
    public void Dispose() => _waiterLock.Dispose();
}

// Lock that offers spinning, thread ownership, and recursion
internal sealed class AnotherHybridLock : IDisposable
{
    // The Int32 is used by the primitive user-mode constructs (Interlocked methods)
    private Int32 _waiters = 0;

    // The AutoResetEvent is the primitive kernel-mode construct
    private readonly AutoResetEvent _waiterLock = new (false);

    // This field controls spinning in an effort to improve performance
    private readonly Int32 _spinCount = 4000; // Arbitrarily chosen count

    // These fields indicate which thread owns the lock and how many times it owns it
    private Int32 _owningThreadId = 0;
    private Int32 _recursion = 0;

    public void Enter()
    {
        // If calling thread already owns the lock, increment recursion count and return
        var threadId = Thread.CurrentThread.ManagedThreadId;

        if (threadId == _owningThreadId)
        {
            _recursion++;
            return;
        }

        // The calling thread doesn't own the lock, try to get it
        var spinWait = new SpinWait();
        for (var spinCount = 0; spinCount < _spinCount; spinCount++)
        {
            // If the lock was free, this thread  got it; set some state and return
            if (Interlocked.CompareExchange(ref _waiters, 1, 0) == 0)
            {
                goto GotLock;
            }

            // Black magic: give other threads a chance to run
            // in hopes that the lock will be released
            spinWait.SpinOnce();
        }

        // Spinning is over and the lock was still not obtained, try one more time
        if (Interlocked.Increment(ref _waiters) > 1)
        {
            // Still contention, this thread must wait
            _waiterLock.WaitOne(); // Wait for the lock; performance hit
            // When this thread wakes, it owns the lock; set some state and return
        }

        GotLock:
        // when a thread gets the lock, we record its ID and
        // Indicate that the thread owns the lock once
        _owningThreadId = threadId;
        _recursion = 1;
    }

    public void Leave()
    {
        // if the calling thread doesn't own the lock, there is a bug
        var threadId = Thread.CurrentThread.ManagedThreadId;
        if (threadId != _owningThreadId)
        {
            throw new SynchronizationLockException("Lock not owned by calling thread");
        }

        // Decrement the recursion count. If this thread still owns the lock just return
        if (--_recursion > 0)
        {
            return;
        }

        _owningThreadId = 0; // No thread owns the lock now

        // If no other threads are waiting, just return
        if (Interlocked.Decrement(ref _waiters) == 0)
        {
            return;
        }

        // Other threads are waiting, wake 1 of them
        _waiterLock.Set(); // Bad performance hit
    }

    /// <inheritdoc />
    public void Dispose() => _waiterLock.Dispose();
}

// Monitor class using private lock
internal sealed class Transaction
{
    private readonly Object _lock = new (); // Each transaction has a PRIVATE lock
    private DateTime _timeOfLastTrans;

    public void PerformTransaction()
    {
        Monitor.Enter(_lock); // Enter the private lock
        // this code has exclusive access to the daa...
        _timeOfLastTrans = DateTime.Now;
        Monitor.Exit(_lock); // Exit the private lock
    }

    public DateTime LastTransaction
    {
        get
        {
            Monitor.Enter(_lock); // Enter the private lock
            var temp = _timeOfLastTrans;
            Monitor.Exit(_lock); // Exit the private lock
            return temp;
        }
    }
}

internal class ReadWriteLockSlimTransaction : IDisposable
{
    private readonly ReaderWriterLockSlim _lock = new (LockRecursionPolicy.NoRecursion);
    private DateTime _timeOfLastTransaction;

    public void PerformTransaction()
    {
        _lock.EnterWriteLock();
        // This code has exclusive access to the data...
        _timeOfLastTransaction = DateTime.Now;
        _lock.ExitWriteLock();
    }

    public DateTime LastTransaction
    {
        get
        {
            _lock.EnterReadLock();
            // This code has shared access to the data...
            var temp = _timeOfLastTransaction;
            _lock.ExitReadLock();
            return temp;
        }
    }

    /// <inheritdoc />
    public void Dispose() => _lock.Dispose();
}
