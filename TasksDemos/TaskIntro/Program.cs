namespace TaskIntro
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            Task task = new Task(SimpleMethod);
            task.Start();
            Task<string> taskThatReturns = new Task<string>(MethodThatReturns);
            taskThatReturns.Start();
            task.Wait();
            Console.WriteLine(taskThatReturns.Result);
            Console.ReadKey();
        }

        private static string MethodThatReturns()
        {
            Thread.Sleep(2000);
            return "Hello";
        }

        private static void SimpleMethod()
        {
            Console.WriteLine("Hello World");
        }
    }
}
