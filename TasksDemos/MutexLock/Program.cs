namespace MutexLock
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    class Program
    {
        static Mutex mutex = new Mutex(false, "RanjanMutex");
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(AcquireMutex);
                thread.Name = string.Format("Thread{0}", i + 1);
                thread.Start();
            }
            Console.ReadKey();
        }

        private static void AcquireMutex()
        {
            if (!mutex.WaitOne(TimeSpan.FromSeconds(1), false))
            {
                Console.WriteLine("{0}", Thread.CurrentThread.Name);
                return;
            }
            //mutex.WaitOne();
            DoSomething();
            mutex.ReleaseMutex();
            Console.WriteLine("Mutx have been released by {0}", Thread.CurrentThread.Name);
        }

        private static void DoSomething()
        {
            Thread.Sleep(3000);
        }
    }
}
