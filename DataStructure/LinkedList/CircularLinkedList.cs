
using System;

namespace DataStructure.LinkedList
{
    public class CircularLinkedList
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

        public Node GetLeafNode()
        {
            Node n = head;
            while (n.next != null)
            {
                n = n.next;
            }

            return n;
        }

        public void Print()
        {
            Node n = this.head;
            if (head != null)
            {
                do
                {
                    Console.Write("{0}->", n.data);
                    n = n.next;
                } while (n != this.head);
            }
        }
    }
}
