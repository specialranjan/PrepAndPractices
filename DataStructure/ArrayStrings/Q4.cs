using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure.ArrayStrings
{
    public partial class Questions
    {
        /*
         Find all elements whose occurrance is even in given integer array
             */
        public static void Q4()
        {
            const int SIZE = 10;
            int[] a = new int[SIZE] { 1, 1, 2, 3, 2, 2, 4, 4, 4, 4 };
            ArrayOperations.PrintFormatted(a, SIZE, "Given Array");
            //FindEvenOccurranceNumbersUsingDictionary(a, SIZE);
            X1(a, SIZE);
        }

        static void X1(int[] a, int n)
        {
            int res = 0;
            for (int i = 0; i < n; i++)
                res = res ^ a[i];

            Console.WriteLine(res);
        }

        static void FindEvenOccurranceNumbers(int[] a, int n)
        {
            int count = 0, prev_num = int.MaxValue;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (a[i] == a[j])
                        count++;
                }

                if (a[i] != prev_num && (count % 2) == 0)
                {                    
                    Console.Write("{0}, ", a[i]);
                    prev_num = a[i];
                }
                count = 0;
            }
        }

        static void FindEvenOccurranceNumbersUsingDictionary(int[] a, int n)
        {
            var d = new Dictionary<int, int>();
            d.Add(a[0], 1);
            for (int i = 1; i < n; i++)
            {
                if (d.ContainsKey(a[i]))
                    d[a[i]]++;
                else
                    d.Add(a[i], 1);
            }

            foreach(var kv in d)
            {
                if (kv.Value % 2 == 0)
                    Console.Write("{0}, ", kv.Key);
            }
        }

        static void FindEvenOccurranceNumbersUsingXOROperator(int[] a, int n)
        {
            long _xor = 0L;
            long pos;

            // do for each element of array  
            for (int i = 0; i < n; ++i)
            {
                // left-shift 1 by value of  
                // current element  
                pos = 1 << a[i];

                // Toggle the bit everytime  
                // element gets repeated  
                _xor ^= pos;
            }

            // Traverse array again and use _xor  
            // to find even occurring elements  
            for (int i = 0; i < n; ++i)
            {
                // left-shift 1 by value of  
                // current element  
                pos = 1 << a[i];

                // Each 0 in _xor represents 
                // an even occurrence  
                if (!((pos & _xor) != 0))
                {
                    // print the even occurring numbers  
                    Console.Write(a[i] + " ");

                    // set bit as 1 to avoid  
                    // printing duplicates  
                    _xor ^= pos;
                }
            }
        }
    }
}
