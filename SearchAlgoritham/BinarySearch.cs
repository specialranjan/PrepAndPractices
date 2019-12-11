
using System;
using System.Globalization;

namespace SearchAlgoritham
{
    public class BinarySearch
    {
        int[] a;
        int n = 11, low, high, key, mid, noOfRecursion = 0, noOfIteration = 0;

        public BinarySearch()
        {
            low = 0;
            high = n - 1;
            key = 28;

            int i = 0;
            a = new int[n];
            for (int d = 20; d < 31; d++)
            {
                a[i++] = d;
            }
        }

        public static void SearchInArray()
        {
            BinarySearch binarySearch = new BinarySearch();            
            Console.Write("Array: ");
            DataStructure.ArrayOperations array = new DataStructure.ArrayOperations(binarySearch.n);
            array.Print(binarySearch.a);

            int index = binarySearch.SearchInArray_IterativeMethod();
            Console.WriteLine("Search element {0} found at index {1}", binarySearch.key, index);
            Console.WriteLine("No of iterations: {0}", binarySearch.noOfIteration);

            index = binarySearch.SearchInArray_RecursiveMethod(binarySearch.low, binarySearch.high, binarySearch.key);
            Console.WriteLine("Search element {0} found at index {1}", binarySearch.key, index);
            Console.WriteLine("No of recursion: {0}", binarySearch.noOfRecursion);
        }

        private int SearchInArray_IterativeMethod()
        {
            int index = -1;
            while (low <= high)
            {
                mid = (low + high) / 2;
                if (key == a[mid])
                {
                    index = mid;
                    break;
                }

                if (key > a[mid])
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }

                noOfIteration++;
            }

            return index;
        }

        private int SearchInArray_RecursiveMethod(int low, int high, int key)
        {
            noOfRecursion++;
            if (low == high)
            {
                if (key == a[low])
                    return low;
                else
                    return -1;
            }
            else
            {
                mid = (low + high) / 2;
                if (key == a[mid])
                    return mid;
                else if (key > a[mid])
                    return SearchInArray_RecursiveMethod(mid + 1, high, key);
                else
                    return SearchInArray_RecursiveMethod(low, mid - 1, key);
            }
        }
    }
}
