namespace ThreadNaming
{
    using System;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            //Here the processor used to switch the context between main and worker thread and the sequence will not match when running the program again and again.
            Thread thread = new Thread(WriteUsingNewThread);
            thread.Name = "Ranjan Worker";
            //Worker thread
            thread.Start();

            Thread.CurrentThread.Name = "Ranjan Main";
            //Main Thread
            for (int i = 0; i < 100; i++)
            {
                Console.Write(" A{0} ", i);
            }

            Console.ReadKey();
        }

        private static void WriteUsingNewThread()
        {            
            for (int i = 0; i < 100; i++)
            {
                Console.Write(" Z{0} ", i);
            }
        }
    }
}
