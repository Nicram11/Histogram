using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Histogram
{
    public class DrzewoBinarne
    {

        private class Node
        {
            public Node left;
            public Node right;
            public int wartosc;
            public int klucz;

            public Node(int key)
            {
                klucz = key;
                wartosc = 1;
            }
        }

        private Node Root;
        private int liczbaOcen = 0;

        public void Put(int key)
        {

            Node node = Root;
            liczbaOcen++;
            Node nowy = new Node(key);
            if (Root == null)  // puste drzewo
            {

                Root = nowy;
                return;
            }

            while (node != null)
            {
                if (key < node.klucz)
                {
                    if (node.left == null)
                    {
                        node.left = nowy;
                        return;
                    }
                    node = node.left;

                }

                else if (key > node.klucz)
                {
                    if (node.right == null)
                    {
                        node.right = nowy;
                        return;
                    }

                    node = node.right;
                }
                else if (key == node.klucz)
                {
                    node.wartosc++;
                    return;
                }
            }

        }

        public int Get(int key)
        {
            Node node = Root;
            while (node != null)
            {

                if (key < node.klucz)
                    node = node.left;

                else if (key > node.klucz)
                    node = node.right;

                else
                    return node.wartosc;

            }
            return 0;
        }

        public void Del(int key)
        {
            Root = Del(Root, key);
        }


        private Node Del(Node n, int key)
        {
            if (key > n.klucz)
                n.right = Del(n.right, key);

            else if (key < n.klucz)
                n.left = Del(n.left, key);

            else
            {
                liczbaOcen -= n.wartosc;

                if (n.left == null)
                    return n.right;

                if (n.right == null)
                    return n.left;
                else
                {
                    n.klucz = Min(n.right);
                    n.right = Del(n.right, n.klucz);
                }
            }
            return n;
        }
        public int Min()
        {
            return Min(Root);
        }
        private int Min(Node n)
        {
            int min = n.klucz;
            while (n.left != null)
            {
                min = n.left.klucz;
                n = n.left;
            }
            return min;
        }
        public int Max()
        {
            return Max(Root);
        }
        private int Max(Node n)
        {
            int max = n.klucz;
            while (n.right != null)
            {
                max = n.right.klucz;
                n = n.right;
            }
            return max;
        }
        int[] tab;
        public int[] ZamienNaTablice() 
        { 
               tab = new int[Max()+1];
            for(int i = 0; i<Max()+1; i++)
                tab[i] = 0;
            ZamienNaTablice(Root);
            return tab;
        }

        private void ZamienNaTablice(Node parent)
        {
            if (parent != null)
            {
                ZamienNaTablice(parent.left);
                tab[parent.klucz] = parent.wartosc;
                ZamienNaTablice(parent.right);
            }
        }

        public int LiczbaOcen()
        {
            return liczbaOcen;
        }
   
    }
}
