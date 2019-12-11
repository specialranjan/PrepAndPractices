using System;
using System.Linq;
using DataStructure.SearchAlgorithm;

namespace DataStructure
{
    public class ArrayOperations
    {
        int n, max, min;
        int[] a;
        
        public ArrayOperations(int n)
        {
            this.n = n;
            a = new int[n];

            for (int i = 0; i < n; i++)
            {
                a[i] = i+1;
            }
        }

        public ArrayOperations(int n, int min, int max)
        {
            this.n = n;
            this.min = min;
            this.max = max;
            this.a = new int[n];
        }

        public int[] CreateWithRandomNumber()
        {
            this.a = Enumerable.Range(this.min, this.max).OrderBy(g => Guid.NewGuid()).Take(this.n).ToArray();
            return this.a;
        }

        public void Add(int value)
        {
            int i = 0;
            while (i < n && a[i] != int.MaxValue) { i++; }
            a[i] = value;
        }
        
        public void Print()
        {            
            foreach (int v in this.a)
            {
                Console.Write("{0} ", v);                
            }

            Console.WriteLine();
        }

        public void Print(int[] arr)
        {
            foreach (int v in arr)
            {
                Console.Write("{0} ", v);
            }

            Console.WriteLine();
        }

        public static void PrintFormatted(int[] a, int n, string msg)
        {
            Console.Write("{0} = [", msg);
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

        //Write a function rotate(ar[], d, n) that rotates a[] of size n by d elements.
        public void Q1()
        {
            System.Console.WriteLine("Write a function rotate(ar[], d, n) that rotates a[] of size n by d elements.");
            int n = 8, k = 2;
            ArrayOperations array = new ArrayOperations(n, 1, 20);
            int[] a = array.CreateWithRandomNumber();
            System.Console.WriteLine();
            System.Console.Write("Original aay: ");
            array.Print(a);

            //Method#1 : Using temp Array
            /*int[] tempArray = new int[n];
            for (int i = k; i < n; i++)
            {
                tempArray[i - k] = a[i];
                if (i == n - 1)
                {
                    int t = i - k;
                    tempArray[t++] = a[i];
                    for (int j = 0; j < k; j++)
                    {
                        tempArray[t++] = a[j];
                    }
                }
            }
            System.Console.WriteLine();
            System.Console.Write("Modified aay: ");
            aay.Print(tempArray);*/

            //Method#2: Using Rotate one by one
            a = LeftRotateQ1(a, k, n);

            System.Console.WriteLine();
            System.Console.Write("Modified aay: ");
            array.Print(a);
        }

        //Given an array, cyclically rotate the array clockwise by one.
        public void Q2()
        {
            Console.WriteLine("Given an array, cyclically rotate the array clockwise by one.");
            Console.Write("Original Array: ");
            Print();

            int d = 1, i = n - 1, x = a[n - 1];
            while (i > 0)
            {
                a[i] = a[i-1];
                i--;
            }

            a[0] = x;
            Console.Write("Modified Array: ");
            Print();
        }

        //Search an element in a sorted and rotated array
        public void Q3()
        {
            Console.WriteLine("Search an element in a sorted and rotated array");
            a = new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 1, 2 };
            Console.Write("Original Array: ");
            Print();
            int key = 1, keyIndex;
            n = 10;
            //BinarySearch binarySearch = new BinarySearch(this.a, this.n, key);
            //keyIndex = binarySearch.SearchInArray(BinarySearch.SearchMethod.Iterative);
            keyIndex = findPivotQ3(a, 0, n - 1);
            Console.WriteLine("Key Index={0}", keyIndex);
        }

        //The following parameters are defined for a string S of length L:
        //1. TDS: The sum of the number of distinct characters in all the distinct substrings of S
        //2. TS: The sum of the number of distinct characters in all the substring of S
        // Write a program to find the absolute difference between TDS and TS
        //Input format:
        // First line: S
        //Output format: 
        //Print the absolute difference between TDS and TS.
        //Constraints
        //1<=L<=500000
        //Sample input: aabb
        //Explanation:
        //Set of all substring of 'aabb'={a,a,b,b,aa,ab,bb,aab,abb,aabb}
        //TS=1+1+1+1+1+2+1+2+2+2=14
        //Set of distinct sub-string of 'aabb'={a,b,aa,ab,bb,aab,abb,aabb}
        //TDS=1+1+1+2+1+2+2+2=12
        //The absolute difference = TDS-TS (12-14)=2
        public static void Q4()
        {
            Console.Write("Enter inout string: ");
            string inputString = Console.ReadLine();
        }

        private int findPivotQ3(int[] arr, int low, int high)
        {
            if (high < low)
                return -1;
            if (high == low)
                return low;

            int mid = (low + high) / 2;

            if (mid < high && arr[mid] > arr[mid + 1])
                return mid;

            if (mid > low && arr[mid] < arr[mid - 1])
                return (mid - 1);

            if (arr[low] >= arr[mid])
                return findPivotQ3(arr, low, mid - 1);

            return findPivotQ3(arr, mid + 1, high);
        }

        private int[] LeftRotateQ1(int[] a, int d, int n)
        {
            for (int i = 0; i < d; i++)
            {
                LeftRotateByOneQ1(a, n);
            }

            return a;
        }

        private int[] LeftRotateByOneQ1(int[] a, int n)
        {
            int j, temp = a[0];
            for (j = 0; j < n - 1; j++)
            {
                a[j] = a[j + 1];
            }

            a[j] = temp;

            return a;
        }
    }
}
