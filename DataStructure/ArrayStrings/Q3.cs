using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure.ArrayStrings
{
    public partial class Questions
    {
        // Find pair of elements whose sum is k in given array.
        // Example:
        // K= 7
        // input - [2, 4, 1, 5, 3]
        // output -  4,3 and 5, 2
        // input - [1, 2, 3, 4, 5]
        // output -  4,3 and 5,2
        public static void Q3()
        {
            const int SIZE = 5;
            int[] a = new int[SIZE] { 2, 4, 1, 5, 3 };
            ArrayOperations.PrintFormatted(a, SIZE, "Given Array");
            FindPairsOfSum(a, SIZE, 7);
        }

        static void FindPairsOfSum(int[] a, int n, int k)
        {
            int start = 0, end = n - 1;
            Console.Write("Pair of sum {0} are: ", k);
            while (end > start)
            {
                if (a[start] + a[end] == k)
                {
                    Console.Write("{0},{1}", a[start], a[end]);
                    break;
                }
                else if (a[start] + a[end] > k)
                    end--;
                else
                    start++;
            }
        }
    }
}
