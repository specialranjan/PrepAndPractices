using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DataStructure.ArrayStrings
{
    public partial class Questions
    {        
        // Find the no of pairs in given array
        public static void Q2()
        {
            const int SIZE = 9;
            int[] a = new int[SIZE] { 10, 20, 20, 10, 10, 30, 50, 10, 20 };
            ArrayOperations.PrintFormatted(a, SIZE, "Given Array");
            int result = FindPairs(a, SIZE);
            Console.WriteLine("The number of pairs in given array is {0}.", result);
        }

        private static int FindPairs(int[] a, int n)
        {
            Array.Sort(a);
            int count = 0, occurrance = 0, noVisited = int.MaxValue;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (a[j] != noVisited && a[i] == a[j])
                    {
                        occurrance++;
                    }
                }

                noVisited = a[i];
                if (occurrance > 0)
                {
                    count += occurrance / 2;
                    occurrance = 0;
                }
            }

            return count;
        }
    }
}
