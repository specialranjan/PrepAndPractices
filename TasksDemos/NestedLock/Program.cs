namespace NestedLock
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        static object ranjanLock = new object();
        static void Main(string[] args)
        {
            //Though we have multiple locks added in other methods that doesn't matter.
            //The lock which is added in main method where the thread starts will drive the lock and once the code within the lock execution is completed he lock will be released.
            lock (ranjanLock)
            {
                DoSomething();
            }
        }

        private static void DoSomething()
        {
            lock (ranjanLock)
            {
                Task.Delay(2000);
                AnotherMethod();
                }
            }

        private static void AnotherMethod()
        {
            lock (ranjanLock)
            {
                Task.Delay(2000);
            }
        }
    }
}
