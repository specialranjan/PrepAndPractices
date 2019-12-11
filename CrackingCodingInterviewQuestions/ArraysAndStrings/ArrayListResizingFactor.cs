using System;
using System.Collections;

namespace CrackingCodingInterviewQuestions
{
    public class ArrayListResizingFactor
    {
        /* Capacity is the size of the internal array... 
         * It starts at 0 then defaults to 4 on first single add.
         * It then goes up by a factor of 2 when it needs more capacity.. 
         * This keeps the memory copying at a minimum (at the cost of a small bit of memory).
         * When we add items in the array list more than its capacity then 
         * A new array is created and the contents of the old one are copied over. 
         * The old one becomes eligible for garbage collection, like any object that no longer has anything referencing it.
         * ArrayList provide fast random access by index. 
         * It is slow to insert or remove in the middle.*/
        public static void CheckArrayListResizingFactor()
        {
            ArrayList numbers = new ArrayList();
            Console.WriteLine("Array List is created with 0 items.");
            Console.WriteLine("Array List size is {0} and capacity is {1}.", numbers.Count, numbers.Capacity);
            Console.WriteLine("Now adding 40 numbers in the Array List.");
            int number;
            for (number = 1; number <= 10; number++)
            {
                numbers.Add(number);
            }

            Console.WriteLine("Array List size is {0} and capacity is {1}.", numbers.Count, numbers.Capacity);
            Console.WriteLine("Now adding 40 more numbers in the Array List.");
            for (; number <= 20; number++)
            {
                numbers.Add(number);
            }

            Console.WriteLine("Array List size is {0} and capacity is {1}.", numbers.Count, numbers.Capacity);
        }
    }
}
