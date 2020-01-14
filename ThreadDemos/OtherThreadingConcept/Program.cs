namespace OtherThreadingConcept
{
    using System;
    using System.Threading;
    class Program
    {
        static void Main(string[] args)
        {
            Thread thread1 = new Thread(PrintHelloWorld);
            thread1.Start();
            thread1.IsBackground = true;
            //It tells the main thread to wait unitl the worker thread thread1 is finished executing
            thread1.Join();
            Console.WriteLine("Hello world printed");
            Console.ReadKey();
        }

        private static void PrintHelloWorld()
        {
            Console.WriteLine("Hello World");
            Thread.Sleep(5000);
        }
    }
}
