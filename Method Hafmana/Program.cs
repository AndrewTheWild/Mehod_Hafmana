using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Method_Hafmana
{ //Клас пара строка-частота
    public class Pair
    {
        private int frequncy;
        private string symbols;
        public Pair() : this(string.Empty, 0)
        { }
        public Pair(string str, int frequncy)
        {
            this.symbols = str;
            this.frequncy = frequncy;
        }
        public string Symbols => symbols;
        public int Frequncy => frequncy;
    }
    //Вузол дерева
    public class Node //узел дерева
    {
        public Node left { get; set; } //указатели узла
        public Node right { get; set; }
        public Pair pair;

        public Node(Pair pair)
        {
            this.pair = pair; //конструктор заполняет узел значением
            left = null;
            right = null;
        }
        //
        public static Node operator+(Node node_1, Node node_2)
        {
            Node rez = new Node(new Pair(node_1.pair.Symbols+node_2.pair.Symbols, node_1.pair.Frequncy + node_2.pair.Frequncy));
            rez.left = node_1;
            rez.right = node_2;
            return rez;
        }

    }
    //Клас бінарне дерево
    public class BinTree
    {
        public Node root; //корень дерева
        public BinTree() //конструктор (по умолчанию) создания дерева
        {
            root = null; //при создании корень не определен
        }

        public BinTree(Pair pair)
        {
            root = new Node(pair); //если изначально задаём корневое значение
        }
        //Додавання елемента
        public void Add(Pair pair) //узел и его значение
        {
            if (root == null)  //если корня нет
            {
                root = new Node(pair); //добавляем элемент как корневой
                return;
            }

            Node current = root; //текущий равен корневому
            bool added = false;
            //обходим дерево
   
            do
            {
                if (pair.Frequncy >current.pair.Frequncy)  //идём вправо
                {
                    if (current.right == null)
                    {
                        current.right = new Node(pair);
                        added = true;

                    }
                    else
                    {
                        current = current.right;
                    }

                }
                if (pair.Frequncy <= current.pair.Frequncy) //идём влево
                {
                    if (current.left == null)
                    {
                        current.left = new Node(pair);
                        added = true;
                    }
                    else
                    {
                        current = current.left;
                    }
                }
            }
            while (!added);

        }


        class Program
        {
            private static Dictionary<char, int> aBC = new Dictionary<char, int>();
            private static Dictionary<string, string> aBC_code = new Dictionary<string, string>();
            public static Dictionary<char, int> ABC { get => aBC; set => aBC = value; }
            public static Dictionary<string, string> ABC_code { get => aBC_code; set => aBC_code= value; }
            static void PrintNode(Node node, string history, string full_history)
            {
                string с_history = string.Empty;
                с_history = с_history + full_history + history;
                if (node != null)               //Пока не встретится пустое звено
                {
                    if (node.pair.Symbols.Length == 1)
                    {
                        ABC_code.Add(node.pair.Symbols, с_history);
                        Console.WriteLine("{0}----{1}", node.pair.Symbols, с_history);

                    }
                    PrintNode(node.left, "0", с_history);               //Рекурсивная функция для вывода левого поддерева
                    PrintNode(node.right, "1", с_history);               //Рекурсивная функци для вывода правого поддерева
                   
                }                
            }
            static void PrintTree(ref BinTree tree)
            {
                if (tree.root != null)
                {
                    if (tree.root.pair.Symbols.Length == 1)
                    {
                        ABC_code.Add(tree.root.pair.Symbols, "1");
                        Console.WriteLine("{0}----{1}", tree.root.pair.Symbols, "1");
                    }
                    PrintNode(tree.root.left,"0", "");
                    PrintNode(tree.root.right, "1", "");


                }
            }
            //
            static Node search_Min(List<Node> nodes)
            {
                Node min = nodes[0];
                for (int i = 1; i < nodes.Count; i++)
                { if (min.pair.Frequncy > nodes[i].pair.Frequncy)
                    {
                        min = nodes[i];
                    }
                }
                return min;
            }
            static void Main(string[] args)
            {
                string code_str;
                Console.WriteLine("Input text:");
                code_str = Console.ReadLine();
                List<Node> free_noodes = new List<Node>();

                foreach (char s in code_str)
                {
                    if (s != ' ')
                        if (Program.ABC.ContainsKey(s))
                        {
                            Program.ABC[s] += 1;
                        }
                        else Program.ABC.Add(s, 1);

                }
                BinTree tree = new BinTree();
                Program.ABC = Program.ABC.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
                foreach (var pair in Program.ABC)
                {
                    Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
                    Pair buf = new Pair(pair.Key.ToString(), pair.Value);
                    free_noodes.Add(new Node(buf));

                    
                }
                while (free_noodes.Count != 1)
                {
                    Node node_1 =search_Min(free_noodes);
                    free_noodes.Remove(node_1);
                    Node node_2 = search_Min(free_noodes);
                    free_noodes.Remove(node_2);
                    free_noodes.Add(node_1 + node_2);

                }
                tree.root = free_noodes[0];
                PrintTree(ref tree);
                Console.ReadLine();
            }
        }
    }
}