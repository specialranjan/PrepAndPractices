
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using DataStructure;
using DataStructure.LinkedList;

namespace Launcher.Console.DataStructure.LinkedList
{
    public class SinglyLinkedListOperations
    {
        public static void Create()
        {
            SinglyLinkedList sllist = new SinglyLinkedList();
            sllist.Append(1);
            sllist.Append(2);
            sllist.Append(3);
            sllist.Append(4);

            SinglyLinkedList.Node node = sllist.SearchNode(3);
            sllist.InsertAfter(5, node);            
            sllist.InsertAtFront(6);
            sllist.Print();
        }

        // Create new linked list from two given linked list with greater element at each node
        public static void Q1()
        {
            System.Console.WriteLine("Create new linked list from two given linked list with greater element at each node");
            SinglyLinkedList list1 = new SinglyLinkedList();
            list1.Append(5);
            list1.Append(2);
            list1.Append(3);
            list1.Append(8);

            System.Console.WriteLine("List1:");
            list1.Print();

            SinglyLinkedList list2 = new SinglyLinkedList();
            list2.Append(1);
            list2.Append(7);
            list2.Append(4);
            list2.Append(5);

            System.Console.WriteLine("List2:");
            list2.Print();

            SinglyLinkedList newList = new SinglyLinkedList();
            SinglyLinkedList.Node l1 = list1.head, l2 = list2.head;
            while (l1 != null && l2 != null)
            {
                if (l1.data > l2.data)
                {
                    newList.Append(l1.data);
                }
                else
                {
                    newList.Append(l2.data);
                }

                l1 = l1.next;
                l2 = l2.next;
            }

            System.Console.WriteLine("New List:");
            newList.Print();
        }

        // Swap Kth node from beginning with Kth node from end in a Linked List
        public static void Q2()
        {
            System.Console.WriteLine("Swap Kth node from beginning with Kth node from end in a Linked List");
            SinglyLinkedList list1 = new SinglyLinkedList();
            list1.Append(5);
            list1.Append(2);
            list1.Append(3);
            list1.Append(8);
            list1.Append(4);
            list1.Append(1);
            list1.Append(9);

            System.Console.WriteLine("Original list:");
            list1.Print();
            
            int k = 2;
            int n = list1.NodeCount();
            if (k > n)
            {
                return;
            }

            SinglyLinkedList.Node x = list1.head;
            SinglyLinkedList.Node x_Prev = null;
            for (int i = 1; i < k; i++)
            {
                x_Prev = x;
                x = x.next;
            }

            SinglyLinkedList.Node y = list1.head;
            SinglyLinkedList.Node y_Prev = null;
            for (int i = 1; i < n - k + 1; i++)
            {
                y_Prev = y;
                y = y.next;
            }

            if (x_Prev != null)
                x_Prev.next = y;

            if (y_Prev != null)
                y_Prev.next = x;

            SinglyLinkedList.Node temp = x.next;
            x.next = y.next;
            y.next = temp;

            if (k == 1)
                list1.head = y;

            if (k == n)
                list1.head = x;

            System.Console.WriteLine("Modified list for K={0}", k);
            list1.Print();
        }

        //Remove first node of the linked list
        public static void Q3()
        {
            System.Console.WriteLine("Remove first node of the linked list");
            SinglyLinkedList sllist = new SinglyLinkedList();
            sllist.Append(1);
            sllist.Append(2);
            sllist.Append(3);
            sllist.Append(4);

            System.Console.WriteLine("Original list:");
            sllist.Print();
            if (sllist.head != null)
            {
                SinglyLinkedList.Node temp = sllist.head.next;
                sllist.head = null;
                sllist.head = temp;
            }
            System.Console.WriteLine("Modified list:");
            sllist.Print();
        }

        //Remove last node of the linked list
        public static void Q4()
        {
            System.Console.WriteLine("Remove last node of the linked list");
            SinglyLinkedList sllist = new SinglyLinkedList();
            sllist.Append(1);
            sllist.Append(2);
            sllist.Append(3);
            sllist.Append(4);

            System.Console.WriteLine("Original list:");
            sllist.Print();

            SinglyLinkedList.Node node = sllist.head;
            while (node.next.next != null)
            {
                node = node.next;
            }

            node.next = null;
            System.Console.WriteLine("Modified list:");
            sllist.Print();
        }

        //Delete Nth node from the end of the given linked list
        public static void Q5()
        {
            System.Console.WriteLine("Delete Nth node from the end of the given linked list");

            //for (int k = 1; k <= 4; k++)
            //{
                SinglyLinkedList sllist = new SinglyLinkedList();
                sllist.Push(1);
                sllist.Push(2);
                sllist.Push(3);
                sllist.Push(4);

                System.Console.WriteLine("Original list:");
                sllist.Print();

            int n = sllist.NodeCount(), k = 3;
            if (k > n) return;

            SinglyLinkedList.Node x = sllist.head;
                System.Console.WriteLine("i  x");
                for (int i = 0; i < k; i++)
                {                    
                    if (x.next == null)
                    {
                        if (i == k - 1)
                        {
                            sllist.head = sllist.head.next;
                        }
                        break;
                    }
                    x = x.next;
                    System.Console.WriteLine("{0}  {1}", i, x.data);
                }

            
            System.Console.WriteLine("Modified list after deleting k={0} element from end", k);
            sllist.Print();
            //}
        }
    }
}
