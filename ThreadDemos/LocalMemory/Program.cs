namespace LocalMemory
{
    using System;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            //Worker Thread
            new Thread(PrintOneToThirty).Start();
            //Main Thread
            PrintOneToThirty();
            Console.ReadKey();
        }

        private static void PrintOneToThirty()
        {
            //Here value of variable i will be different in worker and main thread so both thread will their own seperate memory and value of of the variable of value type
            for (int i = 0; i < 30; i++)
            {
                Console.Write(i + 1 + " ");
            }
        }
    }
}
