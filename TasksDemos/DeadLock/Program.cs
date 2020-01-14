namespace DeadLock
{
    using System;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            object lock1 = new object();
            object lock2 = new object();
            new Thread(() =>
              {
                  lock (lock1)
                  {
                      Console.WriteLine("Worker Thread obtained Lock1");
                      Thread.Sleep(2000);
                      lock (lock2)
                      {
                          Console.WriteLine("Worker Thread obtained Lock2");
                      }
                  }
              }).Start();

            lock (lock2)
            {
                Console.WriteLine("Main thread obtained lock2");
                Thread.Sleep(1000);
                lock (lock1)
                {
                    Console.WriteLine("Main thread obtained lock1");
                }
            }

            Console.ReadKey();
        }
    }
}
