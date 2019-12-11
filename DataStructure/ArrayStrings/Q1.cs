using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure.ArrayStrings
{
    public partial class Questions
    {
        // Find the element in given array which occurres at least n/2 time.
        // [1,1,1,2,3,4] -> 1
        // [3,3,4,5,3,3,6] -> 3;
        // [3,2,4,4,5,3,1,6] -> -1
        // [1,1,1,1,1,2,2,2,2,2] ->
        public static void Q2()
        {
            const int SIZE = 8;
            int[] a = new int[SIZE] { 3, 2, 4, 4, 5, 3, 1, 6 };
            ArrayOperations.PrintFormatted(a, SIZE, "Given Array");
            int candidate = FindMajorityCandidate(a, SIZE);
            if (candidate > 0)
                Console.WriteLine("The majority candidate is {0}.", candidate);
            else
                Console.WriteLine("No majority candidate.");
                
        }

        // Finds the majority element
        // T O(N*N)
        // S O(1)
        static int FindMajorityCandidate(int[] a, int n)
        {
            int count = 0;
            for (int i=0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (a[i] == a[j])
                        count++;
                }

                if (count >= (n / 2))
                    return a[i];
                else
                    count = 0;
            }

            return -1;
        }

        // Using Moore's Voting Algorithm
        // T O(N)+O(N) = O(N)
        // S O(1)
        static int FindMajorityCandidateUsingMooreVotingAlgo(int[] a, int n)
        {
            int count1 = 0, count2 = 0;
            int first = int.MaxValue;
            int second = int.MaxValue;

            for (int i = 1; i < n; i++)
            {
                if (first == a[i])
                    count1++;

                else if (second == a[i])
                    count2++;

                else if (count1 == 0)
                {
                    count1++;
                    first = a[i];
                }

                else if (count2 == 0)
                {
                    count2++;
                    second = a[i];
                }
                else
                {
                    count1--;
                    count2--;
                }
            }

            count1 = 0;
            count2 = 0;

            for (int i = 0; i < n; i++)
            {
                if (a[i] == first)
                    count1++;

                else if (a[i] == second)
                    count2++;
            }

            if (count1 >= n / 2)
                return first;

            if (count2 >= n / 2)
                return second;

            return -1;
        }
    }
}
