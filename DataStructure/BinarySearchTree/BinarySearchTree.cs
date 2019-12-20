
using System;

namespace DataStructure.BinarySearchTree
{
    public class BinarySearchTree
    {
        public class Node
        {
            public int key;
            public Node left, right;

            public Node(int item)
            {
                key = item;
                left = right = null;
            }
        }

        // Root of BST 
        Node root;

        // Constructor 
        public BinarySearchTree()
        {
            root = null;
        }

        void insert(int key)
        {
            root = insertRec(root, key);
        }

        /* A recursive function to insert a new key in BST */
        Node insertRec(Node root, int key)
        {

            /* If the tree is empty, return a new node */
            if (root == null)
            {
                root = new Node(key);
                return root;
            }

            /* Otherwise, recur down the tree */
            if (key < root.key)
                root.left = insertRec(root.left, key);
            else if (key > root.key)
                root.right = insertRec(root.right, key);

            /* return the (unchanged) node pointer */
            return root;
        }

        // This method mainly calls InorderRec() 
        void inorder()
        {
            inorderRec(root);
        }

        // A utility function to do inorder traversal of BST 
        void inorderRec(Node root)
        {
            if (root != null)
            {
                inorderRec(root.left);
                Console.WriteLine(root.key);
                inorderRec(root.right);
            }
        }

        Node search(Node root, int key)
        {
            // Base Cases: root is null or key is present at root 
            if (root == null || root.key == key)
                return root;

            // val is greater than root's key 
            if (root.key > key)
                return search(root.left, key);

            // val is less than root's key 
            return search(root.right, key);
        }

        public void BinarySearchTreeMain()
        {
            insert(50);
            insert(30);
            insert(20);
            insert(40);
            insert(70);
            insert(60);
            insert(80);

            // print inorder traversal of the BST 
            inorder();

            Console.WriteLine("Search node with value 40 in binary search tree above.");
            var foundNode = search(this.root, 40);
            if (foundNode != null)
            {
                Console.WriteLine("Node found in binary search tree");
            }
        }
    }
}
