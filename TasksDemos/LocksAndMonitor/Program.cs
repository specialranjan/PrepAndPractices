namespace LocksAndMonitor
{
    using System;
    using System.Threading.Tasks;
    class Program
    {
        static void Main(string[] args)
        {
            //Account account = new Account(20000);
            Account1 account = new Account1(20000);
            Task task1 = Task.Factory.StartNew(() => account.WithdrawRandomOnly());
            Task task2 = Task.Factory.StartNew(() => account.WithdrawRandomOnly());
            Task task3 = Task.Factory.StartNew(() => account.WithdrawRandomOnly());
            Task.WaitAll(task1, task2, task3);
            Console.WriteLine("All tasks completed");
            Console.ReadKey();
        }
    }
}
