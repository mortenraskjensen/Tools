using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Net.WebRequestMethods;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection.Emit;

namespace Tools.Dir
{
    public abstract class JsonJavaScriptBase0
    {
        //public abstract string EndStruct();
        public abstract int Metode(Data p);
        public abstract void test<T>(T p);
    }
    class SubProg1 : JsonJavaScriptBase0
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
    class SubProg2 : JsonJavaScriptBase0
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
    public enum SubType
    {
        SubType1 = 1,
        SubType2 = 2,
    }
    
    

    public class SerialTest
    {
        public void SerializeNow()
        {/*
            ClassToSerialize c = new ClassToSerialize();
            File f = new File("temp.dat");
            Stream s = f.Open(FileMode.Create);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(s, c);
            s.Close();*/
        }
        public void DeSerializeNow()
        {/*
            ClassToSerialize c = new ClassToSerialize();
            File f = new File("temp.dat");
            Stream s = f.Open(FileMode.Open);
            BinaryFormatter b = new BinaryFormatter();
            c = (ClassToSerialize)b.Deserialize(s);
            Console.WriteLine(c.name);
            s.Close();*/
        }
        /*
        public static void Main(string[] s)
        {
            FaceType f = new FaceType();
            f.faces.Add(new Face("AJApplegate", "/Andet/Zzz_Blandet_gif_NoName/", "AJApplegate_tnzoom.gif"));
            f.faces.Add(new Face("ZoeySinn", "/Andet/Zzz_Blandet_gif_NoName/", "ZoeySinn_tnzoom.gif"));
            SerialTest st = new SerialTest();
            st.SerializeNow();
            st.DeSerializeNow();
            string jsonString = System.Text.Json.JsonSerializer.Serialize(f);

            Console.WriteLine(jsonString);

        }*/
    }

    public class JsonJavaScriptRecord2 : JsonJavaScriptBase
    {
        public bool ok
        {
            get
            {
                return ok;
            }
            set
            {
                ok = value;
            }
        }
        public override string StartData { get { return "JSON.parse('"; } }
        public override string EndData { get { return "');\r\n"; } }
        public override string LineStart { get { return "+'"; } }
        public override string LineEnd { get { return "'\r\n"; } }

        public const string StartField = "+'";
        public const string EndField = "'";
        //public const string StartJSON = "JSON.parse('";
        //public const string EndJSON = "')";
        //public static string RootField(string listRootContent)
        //{
            //return "{\"" + RootName + "\":[" + listRootContent + "]}";
        //}

        public static string EntryFields(int listId, string listName, string dirList, string fileList)
        {
            return "\"" + ListId + "\":" + listId.ToString() + ",\"" + ListName + "\":\"" + listName + "\"" + ",\"" + DirList + "\":[" + dirList + "]" + ",\"" + FileList + "\":[" + fileList + "]";
        }


        public override string PreLine(DirectoryInfoType dir, int level, bool isFirst)
        {
            return "";
        }

        public override string PostJson(DirectoryInfoType dir, int level, bool isFirst)
        {
            return "";
        }

        public override List<string> PostJsonLines(DirectoryInfoType dir, int level, bool isFirst)
        {
            return new List<string>();
        }

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
        public override T RootField<T>(T p)
        {
            return p;
        }
        public override string RootField(string p)
        {
            return p;
        }
        public override List<string> RootField(List<string> p)
        {
            return p;
        }
    }


}