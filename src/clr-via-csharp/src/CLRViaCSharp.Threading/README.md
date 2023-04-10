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

[Back to top ⇧](#threading)

## I/O-Bound Asynchronous Operations

[Back to top ⇧](#threading)

## Primitive Thread Synchronization Constructs

[Back to top ⇧](#threading)

## Hybrid Thread Synchronization Constructs

[Back to top ⇧](#threading)
