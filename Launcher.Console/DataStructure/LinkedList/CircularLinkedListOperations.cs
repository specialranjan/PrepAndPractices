
using DataStructure.LinkedList;

namespace Launcher.Console.DataStructure.LinkedList
{
    public class CircularLinkedListOperations
    {
        public static void Create()
        {
            CircularLinkedList cllist = new CircularLinkedList();
            cllist.Append(1);
            cllist.Append(2);
            cllist.Append(3);
            cllist.Append(4);

            Node leafNode = cllist.GetLeafNode();
            leafNode.next = cllist.head;

            cllist.Print();
        }
    }
}
