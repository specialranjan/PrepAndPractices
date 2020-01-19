using System;
using System.Collections.Generic;
using System.Text;
using DataStructure.LinkedList;

namespace DataStructure
{
    public class GeneralQuestions
    {
        //Convert the binary number represented to a decimal number
        //The binary number are stored in Singly LinkedList
        public static void Q1()
        {
            string binaryNum = "0000011111";
            SinglyLinkedList.Node root = CreateLinkedList(binaryNum);
            long decValue = ConvertBinaryToDecimal(root);
            Console.WriteLine("Binary number {0} decimal value is {1}", binaryNum, decValue);
        }
        private static SinglyLinkedList.Node CreateLinkedList(string binaryNum)
        {
            SinglyLinkedList list = new SinglyLinkedList();
            foreach (var d in binaryNum)
            {
                int digit = d == '0' ? 0 : 1;
                list.Append(digit);
            }
            
            return list.head;
        }

        private static long ConvertBinaryToDecimal(SinglyLinkedList.Node head)
        {
            StringBuilder nums = new StringBuilder();
            while (head != null)
            {
                nums.Append(head.data);
                head = head.next;
            }

            long power = nums.Length - 1, baseValue = 2, decValue = 0;
            for (int i=0;i<nums.Length;i++)
            {
                long binary = nums[i] == '0' ? 0 : 1;
                decValue += binary * getPowerValue(baseValue, power);
                power--;
            }
            return decValue;
        }

        private static long getPowerValue(long baseValue, long power)
        {
            if (power == 0)
                return 1;
            return baseValue * getPowerValue(baseValue, power - 1);
        }
    }
}
