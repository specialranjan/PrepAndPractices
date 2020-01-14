using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLock
{
    class Program
    {
        static ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();
        static Dictionary<int, string> persons = new Dictionary<int, string>();
        static Random random = new Random();
        static void Main(string[] args)
        {
            var task1 = Task.Factory.StartNew(Read);
            var task2 = Task.Factory.StartNew(Write, "Ranjan");
            var task3 = Task.Factory.StartNew(Write, "Ankit");
            var task4 = Task.Factory.StartNew(Read);
            var task5 = Task.Factory.StartNew(Read);
            Task.WaitAll(task1, task2, task3, task4, task5);
            Console.ReadKey();
        }
        static void Read()
        {
            for (int i = 0; i < 10; i++)
            {
                lockSlim.EnterReadLock();
                Thread.Sleep(50);
                lockSlim.ExitReadLock(); 
            }
        }

        static void Write(object user)
        {
            for (int i = 0; i < 10; i++)
            {
                int id = GetRandom();
                lockSlim.EnterWriteLock();
                var person = "Person " + i;
                persons.Add(id, person);
                lockSlim.ExitWriteLock();
                Console.WriteLine(user + " added " + person);
                Thread.Sleep(250);
            }
        }

        static int GetRandom()
        {
            lock (random)
            {
                return random.Next(2000, 5000);
            }
        }
    }
}
