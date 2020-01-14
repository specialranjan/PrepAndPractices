namespace SharedResources
{
    using System;
    using System.Threading;

    class Program
    {
        private static bool isComplated;
        static readonly object lockCompleted = new object();

        static void Main(string[] args)
        {
            Thread thread = new Thread(PrintHelloWorld);
            //Worker thread
            thread.Start();
            //Main Thread
            PrintHelloWorld();

            Console.ReadKey();
        }

        private static void PrintHelloWorld()
        {
            lock (lockCompleted)
            {
                if (!isComplated)
                {
                    isComplated = true;
                    Console.WriteLine("Hello World should print only once");
                }
            }
        }
    }
}
