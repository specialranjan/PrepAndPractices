namespace Exceptions
{
    using System;
    using System.Threading;
    class Program
    {
        static void Main(string[] args)
        {
            Demo();
            Console.ReadKey();
        }

        // Initial code in which a worker thread is throwing exception and main thread to trying to catch but uanle to
        //private static void Demo()
        //{
        //    try {
        //        //All other statements under this method is part of main thread except this line.
        //       new Thread(Execute).Start();
        //    }
        //    catch(Exception ex) {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        //static void Execute()
        //{
        //    //Running under worker thread
        //    throw null;
        //}

        //This is the approach to catch the exception thrown in worker thread
        private static void Demo()
        {
            new Thread(Execute).Start();
        }
        static void Execute()
        {
            try
            { 
                //Running under worker thread
                throw null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
