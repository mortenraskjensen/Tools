using Nedarvning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nedarvning
{
    abstract class Shape
    {
        public abstract int GetArea();
    }

    class Square : Shape
    {
        int side;

        public Square(int n) => side = n;

        // GetArea method is required to avoid a compile-time error.
        public override int GetArea() => side * side;
    }
    // Output: Area of the square = 144
    class AbstrakteKlasser
    {
        //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/abstract
        public AbstrakteKlasser()
        {
            var sq = new Square(12);
            Console.WriteLine($"Area of the square = {sq.GetArea()}");
            var ab = new Abstrakt2();
        }
    }
    class Abstrakt2
    {
        public Abstrakt2()
        {
            var go = new prog();
            go.run(1);
            go.run(1);
            go.run(2);
            go.run(1);
            go.run(1);
            go.run(1);
        }
    }
    class prog
    {
        List<SubProg> managerList = new List<SubProg>
        {//Kun smart hvis GetManager kaldes mange gange
            new SubProg1(),
            new SubProg2(),
        };

        public prog()
        {
            
        }

        public int run(int subType)
        {//Her kan man skrive generel kode
            var manager = GetManager((SubType)subType);
            int i = manager.Metode(new Data());//Metode afhaenger derimod af vardien subType
            manager.test(8);
            return i;
        }


        public SubProg GetManager(SubType subType)
        {
            switch (subType)
            {
                case SubType.SubType1:
                    //return new SubProg1();//Saa vil SubProg1 instantieres hver gang GetManager kaldes (SubProg1 og/eller SubProg2 bruges faa gange).
                    return managerList.FirstOrDefault(h => h is SubProg1);//SubProg1 instantieres kun en gang () 
                case SubType.SubType2:
                    //return new SubProg2();//Saa vil SubProg2 instantieres hver gang GetManager kaldes (SubProg1 og/eller SubProg2 bruges faa gange).
                    return managerList.FirstOrDefault(h => h is SubProg2);
                default:
                    return null;
            }
        }
    }
    abstract class SubProg
    {
        public abstract int Metode(Data p);
        public abstract void test<T>(T p);
    }
    class SubProg1 : SubProg
    {
        public override int Metode(Data p)
        {
            //Console.WriteLine("SubProg1.Metode");
            test(7);
            return 1;
            throw new NotImplementedException();
        }
        public override void test<T>(T p)
        {
            Console.WriteLine("SubProg1.Test {0}", p);
        }
    }
    class SubProg2 : SubProg
    {
        public override int Metode(Data p)
        {
            //Console.WriteLine("SubProg2.Metode");
            test(6);
            return 2;
            throw new NotImplementedException();
        }
        public override void test<T>(T p)
        {
            Console.WriteLine("SubProg2.Test {0}", p);
        }
    }
    class Data
    {
    }
    public enum SubType
    {
        SubType1=1,
        SubType2=2,
    }
    #region nogetandet
    public interface IX
    {
        string XMetode(string par1);
        int[] var2 { get; }
    }
    public abstract class XBase : IX
    {
	    protected const string var1 = "";
        public abstract int[] var2 { get; }
        public abstract string XMetode(string par1);
        protected virtual string XMethod2(string var2)
        {
            return "";
        }
    }
    public class XType1 : XBase
    {
        public XType1()
          : base()
        {

        }
        public override int[] var2 { get; }
        public override string XMetode(string par1)
	    {
            return "";
	    }
    }
    public class XType2 : XBase
    {
        public override int[] var2 { get; }
        public override string XMetode(string par1)
	    {
            return "";
	    }
        protected override string XMethod2(string var2)
        {
            return "";
        }
    }
    public class SomeWhere
    {
        void methodSomThing()
        {
            string var1 = "1";
            string var2 = "2";
            var manager = GetManager(var1);
            manager.XMetode(var2);
        }
        private IX GetManager(string var1)
        {
            switch (var1)
            {
                case "1": return new XType1();
                case "2": return new XType2();
            default: return new XType1();
        }
        }
    }
    #endregion nogetandet
}
