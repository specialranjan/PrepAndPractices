
using System;
using DataStructure.LinkedList;

namespace DataStructure
{
    public class SinglyLinkedList
    {
        public Node head;

        public void Append(int data)
        {
            Node newNode = new Node(data);

            if (head == null)
            {
                head = newNode;
            }
            else
            {
                Node n = head;
                while (n.next != null)
                {
                    n = n.next;
                }

                n.next = newNode;
            }
        }

        public Node SearchNode(int searchData)
        {
            Node n = this.head;

            while (n.next != null)
            {
                if (n.data == searchData)
                {
                    break;
                }
                n = n.next;
            }

            return n;
        }

        public void InsertAfter(int data, Node prevNode)
        {
            Node newNode = new Node(data);
            Node temp = prevNode.next;
            prevNode.next = newNode;
            newNode.next = temp;
        }

        public void InsertAtFront(int data)
        {
            Node newNode = new Node(data);
            Node temp = this.head;
            this.head = newNode;
            newNode.next = temp;
        }

        public void Print()
        {
            Node n = this.head;
            while (n != null)
            {
                Console.Write("{0}", n.data);
                if (n.next != null)
                    Console.Write("-->");
                n = n.next;
            }

            Console.WriteLine();
        }

        public int NodeCount()
        {
            int nodeCount = 0;
            Node n = this.head;
            while (n != null)
            {
                nodeCount++;
                n = n.next;
            }

            return nodeCount;
        }

        public void Push(int data)
        {
            Node new_node = new Node(data);
            new_node.next = this.head;
            head = new_node;
        }
    }
}
