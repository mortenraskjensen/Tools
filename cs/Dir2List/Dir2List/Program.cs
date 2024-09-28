using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Reflection.Emit;
using System.Data;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Text.Json;

namespace Tools.Dir
{
    class Program
    {
        static void Main(string[] args)
        {
            Run r = new Run(args);
        }
    }
    class Run
    {
        public Run(string[] args)
        {
            Console.WriteLine("Hello World!");
            IWork arbejde = new Work();

            ListDirItem List = new ListDirItem(){ Lists = InitDirItem() };
            string ret = JsonSerializer.Serialize(List);

            arbejde.test3(arbejde.SubDir2, arbejde.RootPath);
            arbejde.test4(arbejde.TestDir, arbejde.RootPath);
            arbejde.test6(arbejde.AndetDir, arbejde.RootPath);
        }
        public List<DirItem> InitDirItem()
        {
            DirItem d = GetDirItem(0, "Test");
            List<DirItem> ret = new List<DirItem>() { d };
            d.FileList.Add(GetFileItem("900060", "", "Download2/Test/AnyaMozok_face.jpg", "AnyaMozok_face.jpg", "6960"));
            d.FileList.Add(GetFileItem("900061", "", "Download2/Test/MaciWinslett_tnzoom.gif", "MaciWinslett_tnzoom.gif", "16757"));
            d.DirList.Add(GetDirItem(1, "Piger0"));
            d.DirList.Add(GetDirItem(2, "Piger1"));
            d.DirList.Add(GetDirItem(6, "Piger3"));
            return ret;
        }
        public DirItem GetDirItem(int pListId, string pListName)
        {
            return new DirItem()
            {
                ListId = pListId,
                ListName = pListName,
                DirList = new List<DirItem>(),
                FileList = new List<JavaScriptFileItem>()
            };
        }
        public JavaScriptFileItem GetFileItem(string id, string title, string src, string name, string size)
        {
            return new JavaScriptFileItem()
            {
                id = id,
                title = title,
                src = src,
                name = name,
                size = size
            };
        }

    }
}
