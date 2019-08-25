using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nedarvning
{
    class Run
    {
        public Run()
        {
            TestGenericList2 c = new TestGenericList2();
            TestGenericList1 d = new TestGenericList1();
            BilExempler a = new BilExempler();
            Pro b = new Pro();
        }
    }
    #region BilExempler
    //------------------------------------------------------------------------------------
    class BilExempler
    {
        public List<Bil> billiste = new List<Bil>();
        public BilExempler()
        {
            billiste.Add(new Skoda() { SkodaType = SkodaVersion.Octavia, Farve = "Grøn" });
            billiste.Add(new Folkevogn() { vwType = VwVersion.Golf, Farve = "Grøn" });
            billiste.Add(new Bil() { Farve = "Grøn" });
            foreach (Bil b in billiste)
            {
                if (b is Skoda)
                    System.Console.WriteLine("Er en skoda!");
                else if (b is Folkevogn)
                    System.Console.WriteLine("Er en folkevogn!");
                else
                    System.Console.WriteLine("Er en anden bil!");
                System.Console.WriteLine("Med farven {0}",GivFarve(b));
                System.Console.WriteLine("Af typen {0}", GivVersion(b));
                System.Console.WriteLine("Maerke {0}", b.Maerke);

            }
        }
        public string GivFarve(Bil koeretoej)
        {
            return koeretoej.Farve;
        }
        public string GivVersion(Bil koeretoej)
        {
            if (koeretoej is Skoda)
                return ((Skoda)koeretoej).SkodaType.ToString();
            else if (koeretoej is Folkevogn)
                return ((Folkevogn)koeretoej).vwType.ToString();
            else
                return "ukendt";
        }
    }
    public class Bil
    {
        public string Maerke;
        public string Farve;
    }
    public class Skoda : Bil
    {
        public SkodaVersion SkodaType;
        public Skoda()
        {
            Maerke = "Skoda";
        }
    }
    public class Folkevogn : Bil
    {
        public VwVersion vwType;
        public Folkevogn() { Maerke = "Volkswagen"; }
    }
    public enum SkodaVersion
    {
        Octavia,
        Superb,
    }
    public enum VwVersion
    {
        Polo,
        Golf,
        Passat,
    }
    //------------------------------------------------------------------------------------
    #endregion BilExempler
    #region Generic1
    //------------------------------------------------------------------------------------
    class MyGenericClass
    {
        private int genericMemberVariable;

        public MyGenericClass(int value)
        {
            genericMemberVariable = value;
        }

        public int genericMethod(int genericParameter)
        {
            Console.WriteLine("Parameter type: {0}, value: {1}", typeof(int).ToString(), genericParameter);
            Console.WriteLine("Return type: {0}, value: {1}", typeof(int).ToString(), genericMemberVariable);

            return genericMemberVariable;
        }

        public int genericProperty { get; set; }
    }
    //------------------------------------------------------------------------------------
    #endregion Generic1
    #region Generic2
    //------------------------------------------------------------------------------------
    class Pro
    {
        public delegate T add<T>(T param1, T param2);

        static Pro()
        {
            add<int> sum = AddNumber;

            Console.WriteLine(sum(10, 20));

            add<string> conct = Concate;

            Console.WriteLine(conct("Hello", "World!!"));
        }

        public static int AddNumber(int val1, int val2)
        {
            return val1 + val2;
        }

        public static string Concate(string str1, string str2)
        {
            return str1 + str2;
        }
    }
    //------------------------------------------------------------------------------------
    #endregion Generic2
    #region Generic3
    //------------------------------------------------------------------------------------
    public class GenericList1<T>
    {
        public void Add(T input) { }
    }
    class TestGenericList1
    {
        private class ExampleClass { }
        public TestGenericList1()
        {
            // Declare a list of type int.
            GenericList1<int> list1 = new GenericList1<int>();
            list1.Add(1);

            // Declare a list of type string.
            GenericList1<string> list2 = new GenericList1<string>();
            list2.Add("");

            // Declare a list of type ExampleClass.
            GenericList1<ExampleClass> list3 = new GenericList1<ExampleClass>();
            list3.Add(new ExampleClass());
        }
    }
    //------------------------------------------------------------------------------------
    #endregion Generic3
    #region Generic4
    //------------------------------------------------------------------------------------
    // type parameter T in angle brackets
    public class GenericList2<T>
    {
        // The nested class is also generic on T.
        private class Node
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

        private Node head;

        // constructor
        public GenericList2()
        {
            head = null;
        }

        // T as method parameter type:
        public void AddHead(T t)
        {
            Node n = new Node(t);
            n.Next = head;
            head = n;
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
    }
    class TestGenericList2
    {
        public TestGenericList2()
        {
            // int is the type argument
            GenericList2<int> list = new GenericList2<int>();

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
    }
    //------------------------------------------------------------------------------------
    #endregion Generic4
    #region  GenerigInterfaces
    //Type parameter T in angle brackets.
    public class GenericList3<T> : System.Collections.Generic.IEnumerable<T>
    {
        protected Node head;
        protected Node current = null;

        // Nested class is also generic on T
        protected class Node
        {
            public Node next;
            private T data;  //T as private member datatype

            public Node(T t)  //T used in non-generic constructor
            {
                next = null;
                data = t;
            }

            public Node Next
            {
                get { return next; }
                set { next = value; }
            }

            public T Data  //T as return type of property
            {
                get { return data; }
                set { data = value; }
            }
        }

        public GenericList3()  //constructor
        {
            head = null;
        }

        public void AddHead(T t)  //T as method parameter type
        {
            Node n = new Node(t);
            n.Next = head;
            head = n;
        }

        // Implementation of the iterator
        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
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

    public class SortedList<T> : GenericList3<T> where T : System.IComparable<T>
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

                while (current.next != null)
                {
                    //  Because we need to call this method, the SortedList
                    //  class is constrained on IEnumerable<T>
                    if (current.Data.CompareTo(current.next.Data) > 0)
                    {
                        Node tmp = current.next;
                        current.next = current.next.next;
                        tmp.next = current;

                        if (previous == null)
                        {
                            head = tmp;
                        }
                        else
                        {
                            previous.next = tmp;
                        }
                        previous = tmp;
                        swapped = true;
                    }
                    else
                    {
                        previous = current;
                        current = current.next;
                    }
                }
            } while (swapped);
        }
    }

    // A simple class that implements IComparable<T> using itself as the 
    // type argument. This is a common design pattern in objects that 
    // are stored in generic lists.
    public class Person : System.IComparable<Person>
    {
        string name;
        int age;

        public Person(string s, int i)
        {
            name = s;
            age = i;
        }

        // This will cause list elements to be sorted on age values.
        public int CompareTo(Person p)
        {
            return age - p.age;
        }

        public override string ToString()
        {
            return name + ":" + age;
        }

        // Must implement Equals.
        public bool Equals(Person p)
        {
            return (this.age == p.age);
        }
    }

    class GenerigInterfacesRun
    {
        public GenerigInterfacesRun()
        {
            //Declare and instantiate a new generic SortedList class.
            //Person is the type argument.
            SortedList<Person> list = new SortedList<Person>();

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
    #endregion  GenerigInterfaces
}
