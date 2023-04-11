# Threading

1. [Thread Basics](#thread-basics)
2. [Compute-Bound Asynchronous Operations](#compute-bound-asynchronous-operations)
3. [I/O-Bound Asynchronous Operations](#hybrid-thread-synchronization-constructs)
4. [Primitive Thread Synchronization Constructs](#io-bound-asynchronous-operations)
5. [Hybrid Thread Synchronization Constructs](#primitive-thread-synchronization-constructs)

## Thread basics

A thread is a Windows concept whose job is to virtualize the CPU.

#### Thread Overhead

Every thread has one of each of the following:

- Thread kernel object
- Thread environment block (TEB)
- User-mode stack
- Kernel-mode stack
- DLL thread-attach and thread-detach notifications

At any given moment in time, Windows assigns one thread to a CPU. When the time-slice of the thread expires, windows
context switches to another thread. Every context-switch requires that Windows performs the following actions

1) Save the values in the CPU's registers to the currently running thread's context structure inside the thread's
   kernel object.
2) Select one thread from the set of existing threads to schedule next.
3) Load the values in the selected thread's context structure into the CPU's registers.

#### Reasons to use threads

- Responsiveness (typically for client-side GUI applications)
- Performance (for client and serer side applications)

[Back to top ⇧](#threading)

## Compute-Bound Asynchronous Operations

The ThreadPool.QueueUserWorkItem method and the Timer class always queue work
items to the global queue. Worker threads pull items from this queue using a first-in-first-out (FIFO)
algorithm and process them. Because multiple worker threads can be removing items from the global
queue simultaneously, all worker threads contend on a thread synchronization lock to ensure that
two or more threads don’t take the same work item. This thread synchronization lock can become a
bottleneck in some applications, thereby limiting scalability and performance to some degree.
![The CLR's thread pool](../../img/the-clrs-thread-pool.png "The CLR's thread pool")

Now let’s talk about Task objects scheduled using the default TaskScheduler (obtained by
querying TaskScheduler’s static Default property). When a non-worker thread schedules a Task,
the Task is added to the global queue. But, each worker thread has its own local queue, and when a
worker thread schedules a Task, the Task is added to calling the thread’s local queue.

When a worker thread is ready to process an item, it always checks its local queue for a Task first.
If a Task exists, the worker thread removes the Task from its local queue and processes the item.
Note that a worker thread pulls tasks from its local queue by using a last-in-first-out (LIFO) algorithm.
Because a worker thread is the only thread allowed to access the head of its own local queue, no
thread synchronization lock is required and adding and removing Tasks from the queue is very fast.
A side effect of this behavior is that Tasks are executed in the reverse order that they were queued.

If a worker thread sees that its local queue is empty, then the worker thread will attempt to steal
a Task from another worker thread’s local queue. Tasks are stolen from the tail of a local queue and
require that a thread synchronization lock be taken, which hurts performance a little bit. Of course,
the hope is that stealing rarely occurs, so this lock is taken rarely. If all the local queues are empty,
then the worker thread will extract an item from the global queue (taking its lock) using the FIFO
algorithm. If the global queue is empty, then the worker thread puts itself to sleep waiting for something
to show up. If it sleeps for a long time, then it will wake itself up and destroy itself, allowing the
system to reclaim the resources (kernel object, stacks, TEB) that were used by the thread.

The thread pool will quickly create worker threads so that the number of worker threads is equal
to the value pass to ThreadPool’s SetMinThreads method. If you never call this method (and it’s
recommended that you never call this method), then the default value is equal to the number of CPUs
that your process is allowed to use as determined by your process’s affinity mask. Usually your process
is allowed to use all the CPUs on the machine, so the thread pool will quickly create worker threads
up to the number of CPUs on the machine. After this many threads have been created, the thread
pool monitors the completion rate of work items and if items are taking a long time to complete
(the meaning of which is not documented), it creates more worker threads. If items start completing
quickly, then worker threads will be destroyed.

[Back to top ⇧](#threading)

## I/O-Bound Asynchronous Operations

[Back to top ⇧](#threading)

## Primitive Thread Synchronization Constructs

[Back to top ⇧](#threading)

## Hybrid Thread Synchronization Constructs

[Back to top ⇧](#threading)
