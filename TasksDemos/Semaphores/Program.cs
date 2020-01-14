namespace Semaphores
{
    using System;
    using System.Threading;
    class Program
    {
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(3);
        static void Main(string[] args)
        {
            for (int i = 1; i <= 10; i++)
            {
                new Thread(EnterSemaphore).Start(i);
            }
            Console.ReadKey();
        }

        private static void EnterSemaphore(object id)
        {
            Console.WriteLine("Thread {0} is waiting to be part of the club", id);
            semaphoreSlim.Wait();
            Console.WriteLine("Thread {0} is part of the club", id);
            Thread.Sleep(1000 / (int)id);
            Console.WriteLine("Thread {0} left the club", id);
        }
    }
}
