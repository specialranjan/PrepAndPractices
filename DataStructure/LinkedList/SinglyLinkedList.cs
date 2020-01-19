namespace DataStructure
{
    using System;

    public class SinglyLinkedList
    {
        public class Node
        {
            public int data;
            public Node next;

            public Node(int data)
            {
                this.data = data;
                next = null;
            }
        }

        public Node head;

        public void Swap(char x, char y)
        {
            // Nothing to do if x and y are same  
            if (x == y)
                return;

            // Search for x (keep track of prevX and CurrX)  
            Node currX = this.head, prevX = null;
            while (currX != null && currX.data != x)
            {
                prevX = currX;
                currX = currX.next;
            }

            // Search for y (keep track of prevY and currY)
            Node currY = this.head, prevY = null;
            while (currY != null && currY.data != y)
            {
                prevY = currY;
                currY = currY.next;
            }

            // If either x or y is not present, nothing to do  
            if (currX == null || currY == null)
                return;

            // If x is not head of linked list  
            if (prevX != null)
                prevX.next = currY;
            else //make y the new head  
                head = currY;

            // If y is not head of linked list  
            if (prevY != null)
                prevY.next = currX;
            else // make x the new head  
                head = currX;

            // Swap next pointers  
            Node temp = currX.next;
            currX.next = currY.next;
            currY.next = temp;
        }

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

        public void AddToTail(char data)
        {
            if (this.head == null)
            {
                this.head = new Node(data);
                return;
            }

            Node node = this.head;
            while (node.next != null) node = node.next;
            node.next = new Node(data);
        }
    }
}
