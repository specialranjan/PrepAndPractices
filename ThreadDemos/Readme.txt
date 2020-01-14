Reference: https://www.linkedin.com/learning/threading-in-c-sharp/thread-naming?u=3322

Thread
------
Basic units of execution that are allocated processor time by an operating system.
Sequence of program instructions that can be managed independently by a scheduler.
Thread schedulers are part of the operation system.
A process is an executing program or an application.
Single Threaded program: Onlu one thread has full access to the process.
Multithreaded program: Execution model allows multiple threads to coexist within the process.
Threading is also called multithreading.
Thread execute independently but share resources within the process.
Complicated process of retuning value from a seperate thread.
Lowest-level of construct of multithreading.
Working with thread can be challenging.

How Threading works
-------------------
Multithreading is managed internally by a thread scheduler.
.Net CLR delegates thread scheduling task to the operating systems.

Share Resources
---------------
The CLR assigns each thread its own local memory stack to keep local variable seperate.
A seperate copy of local variables is created on each thread's stack.

Thread Scheduler
----------------
Ensure all active threads are allocated appropriate execution time.
So how does it work in case of a single processor computer? We use something called timeslicing, which means the thread scheduler rapidly switching execution between each of the active thread.

What is Preempted Thread? 
---------------------------
Thread that has execution interrupted, usually by an external factor such as timeslicing.
Thread has no control over when and where it is preempted.

Thread Vs. Process
----------------------
Thread:
Run in parallel within a a single process.
Share memory(Heap) with other threads running in the same application.

Processes:
Are fully isolated to each other.


Thread Pool
----------------
Every thread has overhead in time and memory.
Thread pool reduce the performance penalty by sharing and recycling thread.
Thread pool only create background thread.
Limits the nomber of threads can run simultaneously.
When the limit is reached, all jobs from a queue and begin only when another job finishes.
Thread.CurrentThread.IsThreadPoolThread property - determine if execution is happening in a pool thread.

Background Thread
-----------------
Identical to foreground thread, except the managed execution environment is not kept running.
If the main thread dies, background thread will terminate abrupty.

Ways to enter Thread Pool
--------------------------
Task Parallel Library
Asynchronous delegates
Background work
Call ThreadPool.QueueUserWorkItem






