Reference: https://www.linkedin.com/learning/threading-in-c-sharp/tasks-vs-threads?u=3322

Tasks
-----
Higher-level abstractions.
Capable of returning values.
Can be chained.
May use a thread pool.
May be used for I/O bound operations.

CPU Bound
----------
Uses resources of a local machine.
Computation-intensive operations.

I/O Bound
---------------
Out-of-Process calls.
Operations can take an indeterminate amount of time because wait for external input.
Release local resources while waiting for response.

Tasks with Continuation
-----------------------
Contiuation Task is an asynchronous task that is invoked by another task called an antecedent.
Pass data from antecedent to the continuation task.
Use exception passing from antecedent to continuation.
Control how the continuation is invoked.
Able to cancel a continuation.
Invoke multiple continuations.
Invoke continuation based on completion of antecedents.

Synchronization
-----------------------
Sinchronization is necessary when multiple running multiple threads to get the predictble outcome.
Act of coordinating actions of multiple threads or tasks running concurrently.

Different way to achieve Synchronization
----------------------------------------
Blocking methods
Locks
Signals
Nonblocking constructs

Blocking Method
---------------------
Thread.Sleep
Thread.Join
Task.Wait

It blocks the task execution until the other task is completed.

Blocking Vs Spinning
--------------------------------------
Blocking

Blocking threads do not consume CPU.
Blocked threads do consume memory.

Spinning

Consumes CPU for as long as the thread is blocked.
while(x < limit) //Uses CPU as long as the condition is not met.

Locks
----------------------------------
Limit the number of threads.

Exclusive Lock
NonExclusive Lock

Exclusive Lock
---------------------
Allow only one thread to access a certain section of code.
lock keyword uses exclusive lock.
Alternative is to use Monitor.Enter/Monitor.Exit

NonExclusive Lock
----------------------------
Semaphore
SemaphoreSlim
Reader/Writer
Allow multiple threads to access a resource.

Signaling Constructs
----------------------
Threads pause until they receive a signal from another thread.

There are two comonly used signaling devices, they are called 
1. Event wait handles 
2. Monitor's Wait/Pulse methods.

Intruduced in .NET framework 4
3. CountdownEvent class
4. Barrier classe

Nonblocking Synchronization
-------------------------------
Thread.MemoryBarrier
Thread.VolitileRead
Thread.VolitileWrite
Volitile keyword 
Interlocked class

Protect access to a common field.
