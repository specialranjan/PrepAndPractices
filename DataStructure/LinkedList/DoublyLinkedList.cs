namespace DataStructure.LinkedList
{
    using System;
    public class DoublyLinkedList
    {
        public class Node
        {
            public Node prev = null;
            public Node next = null;
            public char? data = null;

            public Node(char data)
            {
                this.data = data;
            }
        }

        public Node head;

        public void Swap(char x, char y)
        {
            if (x == y)
                return;

            Node a = null, b = null, node = this.head;
            if (this.head.data == x)
                a = this.head;
            if (this.head.data == y)
                b = this.head;

            while (node.next != null)
            {
                if (node.next.data == x && a == null)
                    a = node.next;
                if (node.next.data == y && b == null)
                    b = node.next;

                node = node.next;
            }

            if (a.next == b)
            {
                // right next to each other

                a.next = b.next;
                b.prev = a.prev;

                if (a.next != null)
                {
                    a.next.prev = a;
                }

                if (b.prev != null)
                {
                    b.prev.next = b;
                }

                b.next = a;
                a.prev = b;
            }
            else
            {
                Node n = b.next;
                Node p = b.prev;

                b.next = a.next;
                b.prev = a.prev;

                a.prev = p;
                a.next = n;

                if (b.next != null)
                    b.next.prev = b;
                if (b.prev != null)
                    b.prev.next = b;

                if (a.next != null)
                    a.next.prev = a;
                if (a.prev != null)
                    a.prev.next = a;
            }

            if (a.prev == null)
                this.head = a;
            if (b.prev == null)
                this.head = b;
        }

        public void AddToTail(char data)
        {
            Node newNode = new Node(data);
            Node last = this.head;
            if (this.head == null)
            {
                this.head = newNode;
                return;
            }
            else
            {
                while (last.next != null) last = last.next;

                last.next = newNode;
                newNode.prev = last;
            }
        }

        public void Print()
        {
            Node node = this.head;
            while (node != null)
            {
                Console.Write("{0}  ", node.data);
                node = node.next;
            }
        }
    }
}
