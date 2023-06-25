using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nedarvning
{
    #region  GenericList
    public interface ISortedList<T> : System.Collections.Generic.IEnumerable<T>
    {//Only for reusing tests in GenerigSort
        void AddHead(T t);
        void BubbleSort();
    }
    public interface IGenericEnumerableList<T> : System.Collections.Generic.IEnumerable<T>
    {
        void AddHead(T t);
    }
    public abstract class ListBase<T> : System.Collections.Generic.IEnumerable<T>
    {
        // The nested class is also generic on T.
        protected class Node
        {
            // T used in non-generic constructor.
            public Node(T t)
            {
                last = null;//Only for my list
                next = null;
                data = t;
            }

            private Node next;
            public Node Next
            {
                get { return next; }
                set { next = value; }
            }

            // T as private member data type.
            private T data;

            // T as return type of property.
            public T Data
            {
                get { return data; }
                set { data = value; }
            }

            //Only for my list
            private Node last;
            public Node Last
            {
                get { return last; }
                set { last = value; }
            }

        }
        protected Node head;
        protected Node listEnd;//Only for my list
        protected Node current = null;//needed for bubble sort

        // constructor
        public ListBase()
        {
            head = null;
            listEnd = null;
        }

        public abstract void AddHead(T t);

        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;

            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
        // IEnumerable<T> inherits from IEnumerable, therefore this class 
        // must implement both the generic and non-generic versions of 
        // GetEnumerator. In most cases, the non-generic method can 
        // simply call the generic method.
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public abstract class SortedBase<T> : ListBase<T>, ISortedList<T> where T : System.IComparable<T>
    {
        // A simple, unoptimized sort algorithm that 
        // orders list elements from lowest to highest:

        public void BubbleSort()
        {
            if (null == head || null == head.Next)
            {
                return;
            }
            bool swapped;

            do
            {
                Node previous = null;
                Node current = head;
                swapped = false;

                while (current.Next != null)
                {
                    //  Because we need to call this method, the SortedList
                    //  class is constrained on IEnumerable<T>
                    if (current.Data.CompareTo(current.Next.Data) > 0)
                    {
                        Node tmp = current.Next;

                        current.Last = tmp;
                        tmp.Last = previous;
                        if (current.Next.Next != null)
                            current.Next.Next.Last = current;
                        else
                            listEnd = current;



                        current.Next = current.Next.Next;
                        tmp.Next = current;

                        if (previous == null)
                        {
                            head = tmp;
                        }
                        else
                        {
                            previous.Next = tmp;
                        }
                        previous = tmp;
                        swapped = true;
                    }
                    else
                    {
                        previous = current;
                        current = current.Next;
                    }
                }
            } while (swapped);
        }
    }
    public class StackBase<T> : System.Collections.Generic.IEnumerable<T>
    {
        // The nested class is also generic on T.
        protected class Node
        {
            // T used in non-generic constructor.
            public Node(T t)
            {
                next = null;
                data = t;
            }

            private Node next;
            public Node Next
            {
                get { return next; }
                set { next = value; }
            }

            // T as private member data type.
            private T data;

            // T as return type of property.
            public T Data
            {
                get { return data; }
                set { data = value; }
            }
        }

        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        protected Node head;
        protected Node current = null;//needed for bubble sort


        // constructor
        public StackBase()
        {
            head = null;
        }

        // T as method parameter type:
        public void Push(T t)
        {
            Node n = new Node(t);
            n.Next = head;
            head = n;
        }

        public void AddHead(T t)
        {
            Push(t);
        }


        public void Pop()
        {
            Node n = head;
            if (n != null)
            {
                head = n.Next;
                n = null;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;

            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
        // IEnumerable<T> inherits from IEnumerable, therefore this class 
        // must implement both the generic and non-generic versions of 
        // GetEnumerator. In most cases, the non-generic method can 
        // simply call the generic method.
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public static void Test()
        {
            // int is the type argument
            StackBase<int> list = new StackBase<int>();
            list.Name = "Ten first integers";

            for (int x = 0; x < 10; x++)
            {
                list.Push(x);
            }

            foreach (int i in list)
            {
                System.Console.Write(i + " ");
            }
            System.Console.WriteLine("");
            list.Pop();
            foreach (int i in list)
            {
                System.Console.Write(i + " ");
            }
            System.Console.WriteLine("\nDone {0}", list.Name);
        }
    }
    public class NamedList<T> : System.Collections.Generic.IEnumerable<T>
    {//Adding in the end of the list
        // The nested class is also generic on T.
        private class Node
        {
            // T used in non-generic constructor.
            public Node(T t)
            {
                last = null;
                next = null;
                data = t;
            }

            private Node next;
            public Node Next
            {
                get { return next; }
                set { next = value; }
            }
            private Node last;
            public Node Last
            {
                get { return last; }
                set { last = value; }
            }


            // T as private member data type.
            private T data;

            // T as return type of property.
            public T Data
            {
                get { return data; }
                set { data = value; }
            }
        }

        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private Node listEnd;
        private Node head;

        // constructor
        public NamedList()
        {
            listEnd = null;
            head = null;
        }

        // T as method parameter type:
        public void Add(T t)
        {
            Node n = new Node(t);
            n.Last = listEnd;

            if (head == null)
                head = n;//First
            else //if (listEnd != null)//Not first
                listEnd.Next = n;

            listEnd = n;
        }

        private void Del(Node n)
        {
            if (n != null)
            {
                if (n.Next != null)
                    n.Next.Last = n.Last;
                else
                    listEnd = n.Last;

                if (n.Last != null)
                    n.Last.Next = n.Next;
                else
                    head = n.Next;

                n = null;
            }
        }

        public void DelIdx(int idx)
        {
            Node n = head;
            //for (int x = 0; x < idx && n.Next != null; x++)//idx > index for last element => n == listEnd => DelLast()!
            for (int x = 0; x < idx && n != null; x++)//idx > index for last element => n == null => do nothing! (maybe it should fail?)
                n = n.Next;

            Del(n);
        }

        public void DelLast()
        {
            Node n = listEnd;
            Del(n);
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;

            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
        // IEnumerable<T> inherits from IEnumerable, therefore this class 
        // must implement both the generic and non-generic versions of 
        // GetEnumerator. In most cases, the non-generic method can 
        // simply call the generic method.
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public static void Test()
        {
            // int is the type argument
            NamedList<int> list = new NamedList<int>();
            list.Name = "Ten first integers";

            for (int x = 0; x < 10; x++)
            {
                list.Add(x);
            }

            foreach (int i in list)
                System.Console.Write(i + " ");
            System.Console.WriteLine("");

            list.DelIdx(9);
            foreach (int i in list)
                System.Console.Write(i + " ");
            System.Console.WriteLine("");

            list.DelIdx(10);
            foreach (int i in list)
                System.Console.Write(i + " ");
            System.Console.WriteLine("");

            list.DelIdx(5);
            foreach (int i in list)
                System.Console.Write(i + " ");
            System.Console.WriteLine("");

            list.DelLast();
            foreach (int i in list)
                System.Console.Write(i + " ");
            System.Console.WriteLine("\nDone {0}", list.Name);
        }
    }
    public class SortedNamedList<T> : SortedBase<T> where T : System.IComparable<T>
    {//Adding in the end of the list and having a name

        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        // constructor
        public SortedNamedList()
            : base()
        {
        }

        // T as method parameter type:
        public void Add(T t)
        {
            Node n = new Node(t);
            n.Last = listEnd;

            if (head == null)
                head = n;//First
            else //if (listEnd != null)//Not first
                listEnd.Next = n;

            listEnd = n;
        }

        public override void AddHead(T t)
        {
            Add(t);
        }

        private void Del(Node n)
        {
            if (n != null)
            {
                if (n.Next != null)
                    n.Next.Last = n.Last;
                else
                    listEnd = n.Last;

                if (n.Last != null)
                    n.Last.Next = n.Next;
                else
                    head = n.Next;

                n = null;
            }
        }

        public void DelIdx(int idx)
        {
            Node n = head;
            //for (int x = 0; x < idx && n.Next != null; x++)//idx > index for last element => n == listEnd => DelLast()!
            for (int x = 0; x < idx && n != null; x++)//idx > index for last element => n == null => do nothing! (maybe it should fail?)
                n = n.Next;

            Del(n);
        }

        public void DelLast()
        {
            Node n = listEnd;
            Del(n);
        }
    }
    public class SortedStack<T> : StackBase<T>, ISortedList<T> where T : System.IComparable<T>
    {
        // A simple, unoptimized sort algorithm that 
        // orders list elements from lowest to highest:

        public void BubbleSort()
        {
            if (null == head || null == head.Next)
            {
                return;
            }
            bool swapped;

            do
            {
                Node previous = null;
                Node current = head;
                swapped = false;

                while (current.Next != null)
                {
                    //  Because we need to call this method, the SortedList
                    //  class is constrained on IEnumerable<T>
                    if (current.Data.CompareTo(current.Next.Data) > 0)
                    {
                        Node tmp = current.Next;
                        current.Next = current.Next.Next;
                        tmp.Next = current;

                        if (previous == null)
                        {
                            head = tmp;
                        }
                        else
                        {
                            previous.Next = tmp;
                        }
                        previous = tmp;
                        swapped = true;
                    }
                    else
                    {
                        previous = current;
                        current = current.Next;
                    }
                }
            } while (swapped);
        }
    }
    public class SortedList2<T> : GenericList3<T>, ISortedList<T> where T : System.IComparable<T>
    {
        // A simple, unoptimized sort algorithm that 
        // orders list elements from lowest to highest:

        public void BubbleSort()
        {
            if (null == head || null == head.Next)
            {
                return;
            }
            bool swapped;

            do
            {
                Node previous = null;
                Node current = head;
                swapped = false;

                while (current.Next != null)
                {
                    //  Because we need to call this method, the SortedList
                    //  class is constrained on IEnumerable<T>
                    if (current.Data.CompareTo(current.Next.Data) > 0)
                    {
                        Node tmp = current.Next;
                        current.Next = current.Next.Next;
                        tmp.Next = current;

                        if (previous == null)
                        {
                            head = tmp;
                        }
                        else
                        {
                            previous.Next = tmp;
                        }
                        previous = tmp;
                        swapped = true;
                    }
                    else
                    {
                        previous = current;
                        current = current.Next;
                    }
                }
            } while (swapped);
        }
    }
    #endregion  GenericList
    #region  GenericListTest
    class GenerigListTest
    {
        public static void TestGenericList(ISortedList<int> list)
        {
            for (int x = 0; x < 10; x++)
            {
                list.AddHead(x);
            }

            foreach (int i in list)
            {
                System.Console.Write(i + " ");
            }
            System.Console.WriteLine("\nDone");
        }


        public static void TestNamedList()
        {
            // int is the type argument
            SortedNamedList<int> list = new SortedNamedList<int>();
            list.Name = "Ten first integers";

            for (int x = 0; x < 10; x++)
            {
                list.Add(x);
            }

            foreach (int i in list)
                System.Console.Write(i + " ");
            System.Console.WriteLine("");

            list.DelIdx(9);
            foreach (int i in list)
                System.Console.Write(i + " ");
            System.Console.WriteLine("");

            list.DelIdx(10);
            foreach (int i in list)
                System.Console.Write(i + " ");
            System.Console.WriteLine("");

            list.DelIdx(5);
            foreach (int i in list)
                System.Console.Write(i + " ");
            System.Console.WriteLine("");

            list.DelLast();
            foreach (int i in list)
                System.Console.Write(i + " ");
            System.Console.WriteLine("\nDone {0}", list.Name);
        }
        public static void RunSortTest(ISortedList<Person> list)
        {
            //Declare and instantiate a new generic SortedList class.
            //Person is the type argument.
            //SortedList<Person> list = new SortedList<Person>();

            //Create name and age values to initialize Person objects.
            string[] names = new string[]
            {
            "Franscoise",
            "Bill",
            "Li",
            "Sandra",
            "Gunnar",
            "Alok",
            "Hiroyuki",
            "Maria",
            "Alessandro",
            "Raul"
            };

            int[] ages = new int[] { 45, 19, 28, 23, 18, 9, 108, 72, 30, 35 };

            //Populate the list.
            for (int x = 0; x < 10; x++)
            {
                list.AddHead(new Person(names[x], ages[x]));
            }

            //Print out unsorted list.
            foreach (Person p in list)
            {
                System.Console.WriteLine(p.ToString());
            }
            System.Console.WriteLine("Done with unsorted list");

            //Sort the list.
            list.BubbleSort();

            //Print out sorted list.
            foreach (Person p in list)
            {
                System.Console.WriteLine(p.ToString());
            }
            System.Console.WriteLine("Done with sorted list");
        }
    }
    #endregion  GenericListTest
    public interface ICommonBehavior
    {
        List<string> GetEmployee(string emp);
        string ShowEmployee(string emp);
    }
    public class test1 : ICommonBehavior
    {
        public List<string> GetEmployee(string emp)
        {
            List<string> ret = new List<string>();
            return ret;
        }
        //public string ShowEmployee(string emp)
            //=> emp;
        public string ShowEmployee(string emp)
        {
            string ret = "";
            foreach (string s in GetEmployee(emp))
                ret += s + "\r\n";
            return ret;
        }
    }
    public class test2 : ICommonBehavior
    {
        public List<string> GetEmployee(string emp)
        {
            List<string> ret = new List<string>();
            return ret;
        }
        public string ShowEmployee(string emp)
        {
            string ret = "";
            foreach (string s in GetEmployee(emp))
                ret += s + "\r\n";
            return ret;
        }
    }
    public class test3 : ICommonBehavior
    {
        public List<string> GetEmployee(string emp)
        {
            List<string> ret = new List<string>();
            return ret;
        }
        public string ShowEmployee(string emp)
        {
            string ret = "";
            foreach (string s in GetEmployee(emp))
                ret += s + "\r\n";
            return ret;
        }
    }
    public class GenericFunctions
    {
        public static List<string> GetEmployee<T>(T d, string emp) where T : ICommonBehavior
        {
            return d.GetEmployee(emp);
        }
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        void SwapIfGreater<T>(ref T lhs, ref T rhs) where T : System.IComparable<T>
        {
            T temp;
            if (lhs.CompareTo(rhs) > 0)
            {
                temp = lhs;
                lhs = rhs;
                rhs = temp;
            }
        }
        public static void Test1()
        {
            var d = new TestGeneric<test3>() { Name = "Davs", Data = new test3() };
            var a = GenericFunctions.GetEmployee(new test1(), "emp1");
            var b = GenericFunctions.GetEmployee(new test2(), "emp1");
            var c = GenericFunctions.GetEmployee(d.Data, "emp1");
        }
    }
    public class TestGeneric<T>
    {
        public string Name { get;set;}
        public T Data { get;set;}
    }
}
