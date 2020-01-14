namespace ThreadPool
{
    using System;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Is Main function is in Thread Pool before executing worker thread: {0}", Thread.CurrentThread.IsThreadPoolThread);
            
            Employee employee = new Employee();
            employee.Name = "Ranjan Tiwari";
            employee.CompanyName = "Microsoft";

            ThreadPool.QueueUserWorkItem(new WaitCallback(DisplayEmployeeInfo), employee);

            //One approach to set the limit of threads in Pool
            //var processorCount = Environment.ProcessorCount;
            //ThreadPool.SetMaxThreads(processorCount * 2, processorCount * 2);
            
            //Another approach to set the limit of threads in Pool
            int workerThread = 0, completionPortThread = 0;
            ThreadPool.GetMinThreads(out workerThread, out completionPortThread);
            ThreadPool.SetMaxThreads(workerThread * 2, completionPortThread * 2);
            
            Console.WriteLine("Is Main function is in Thread Pool after executing worker thread: {0}", Thread.CurrentThread.IsThreadPoolThread);
            
            Console.ReadKey();
        }

        private static void DisplayEmployeeInfo(object employee)
        {
            Console.WriteLine("Is DisplayEmployeeInfo function is in Thread Pool: {0}", Thread.CurrentThread.IsThreadPoolThread);
            Employee emp = employee as Employee;
            Console.WriteLine("Employee Name: {0} Company Name: {1}", emp.Name, emp.CompanyName);
        }
    }

    class Employee
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
    }
}
