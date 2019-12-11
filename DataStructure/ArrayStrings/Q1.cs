using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure.ArrayStrings
{
    public partial class Questions
    {
        // Find the no of pairs in given array
        public static void Q1()
        {
            const int SIZE = 9;
            int[] a = new int[SIZE] { 10, 20, 20, 10, 10, 30, 50, 10, 20 };
            PrintFormatted(a, SIZE);
            int result = Q1_1(a, SIZE);
            Console.WriteLine("The number of pairs in given array is {0}.", result);
        }

        public static int Q1_1(int[] a, int n)
        {
            Array.Sort(a);
            int count = 0;
            for (int i = 0; i < n-1; i++)
            {                
                if (a[i] == a[i + 1])
                    count++;
            }
            return count;
        }

        public static void PrintFormatted(int[] a, int n)
        {
            Console.Write("Given array a = [");
            for (int i = 0; i < n; i++)
            {
                if (i == n - 1)
                    Console.Write("{0}", a[i]);
                else
                    Console.Write("{0}, ", a[i]);
            }

            Console.Write("]");
            Console.WriteLine();
        }
    }
}
