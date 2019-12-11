using System;

namespace DataStructure.SearchAlgorithm
{
    public class BinarySearch
    {
        int[] a;
        int n = 11, low, high, key, mid, noOfRecursion = 0, noOfIteration = 0;
        bool showMessages = true;
        public enum SearchMethod { Iterative, Recursive }
        
        public BinarySearch(int[] arr, int n, int key, bool showMessages = false)
        {
            this.a = arr;
            this.n = n;
            this.low = 0;
            this.high = n - 1;
            this.key = key;
            this.showMessages = showMessages;
        }

        public int SearchInArray(SearchMethod searchMethod = SearchMethod.Iterative)
        {
            int keyIndex;
            if (showMessages) Console.Write("Array: ");
            DataStructure.ArrayOperations array = new DataStructure.ArrayOperations(n);
            if (showMessages) array.Print(this.a);

            if (searchMethod == SearchMethod.Iterative)
            {
                keyIndex = SearchInArray_IterativeMethod();
                if (showMessages) Console.WriteLine("Search element {0} found at index {1}", key, keyIndex);
                if (showMessages) Console.WriteLine("No of iterations: {0}", noOfIteration);
            }
            else
            {
                keyIndex = SearchInArray_RecursiveMethod(low, high, key);
                if (showMessages) Console.WriteLine("Search element {0} found at index {1}", key, keyIndex);
                if (showMessages) Console.WriteLine("No of recursion: {0}", noOfRecursion);
            }

            return keyIndex;
        }

        private int SearchInArray_IterativeMethod()
        {
            int index = -1;

            if (high == low)
            {
                index = low;
            }
            else if (high < low)
            {
                index = -1;
            }
            else
            {
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
