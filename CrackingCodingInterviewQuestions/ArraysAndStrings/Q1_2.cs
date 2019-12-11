using System;
using System.Collections.Generic;
using System.Text;

namespace CrackingCodingInterviewQuestions.ArraysAndStrings
{
    public partial class InterviewQuestions
    {
        public static void Q1_2()
        {
            int[] a = new int[5] { 1, 2, 3, 4, 5 };
            printpairs(a, 7);
        }

        static void printpairs(int[] arr,
                           int sum)
        {
            HashSet<int> s = new HashSet<int>();
            for (int i = 0; i < arr.Length; ++i)
            {
                int temp = sum - arr[i];

                // checking for condition 
                if (s.Contains(temp))
                {
                    Console.Write("Pair with given sum " + sum + " is (" + arr[i] + ", " + temp + ")");
                }
                s.Add(arr[i]);
            }
        }
    }
}
