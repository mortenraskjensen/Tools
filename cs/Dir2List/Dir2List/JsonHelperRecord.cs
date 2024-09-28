using System;
using System.Collections.Generic;
//using static System.Net.WebRequestMethods;
using System.Linq;
using System.Text;

namespace Tools.Dir
{
    public interface IJsonJavaScript
    {
        //const string RootName = "Lists";
        //const string ListId = "ListId";
        //const string ListName = "ListName";
        //const string DirList = "DirList";
        //const string FileList = "FileList";


        //int[] var2 { get; }
        int FileId { get; set; }
        int DirId { get; set; }
        string VarName { get; set; }
        string WorkingDir { get; set; }
        Dictionary<string, string> ExtFilter { get; set; }
        string StartData { get; }
        string EndData { get; }
        string LineStart { get; }
        string LineEnd { get; }

        //string RootField(string listRootContent);
        //string EntryFields(int listId, string listName);
        //string EntryFields(int listId, string listName, string dirList, string fileList);
        //string DirListStart();
        //string FileStart();
        //string ListEnd();
        //string Data(string id, string title, string src, string name);
        //string Data(string id, string title, string src, string name, long size);

        //string BeginStruct(string VarName);
        //string ListLine(int listId, string listName, bool isFirst);
        //string DataLine(string id, string title, string src, string name, bool isFirst);
        //string EndList();
        void SetParameters(int dirId, int fileId, string varName, string workingDir, Dictionary<string, string> extFilter);
        string EntryFields(int listId, string listName);
        string Data(string id, string title, string src, string name);
        string DirListStart();
        string FileStart();
        string ListEnd();
        string PreLine(DirectoryInfoType dir, int level, bool isFirst);
        string PostJson(DirectoryInfoType dir, int level, bool isFirst);
        List<string> PostJsonLines(DirectoryInfoType dir, int level, bool isFirst);

        int Metode(Data p);
        void test<T>(T p);
        T RootField<T>(T p);
        string RootField(string p);
        List<string> RootField(List<string> p);
        string JsonString(DirectoryInfoType dir, bool isFirst, int level);
        //string EndStruct();
    }
    public abstract class JsonJavaScriptBase : IJsonJavaScript
    {
        public const string RootName = "Lists";
        protected const string ListId = "ListId";
        protected const string ListName = "ListName";
        protected const string DirList = "DirList";
        protected const string FileList = "FileList";
        private int _fileId = 900001;
        private int _dirId = 0;
        private string _varName = "data1";
        private string _workingDir = FileManager.normalizeFileName("L:\\Backup\\pic");
        private Dictionary<string, string> _extFilter = new Dictionary<string, string>();
        public static readonly List<string> ExclusionExtensionList = new List<string>() { ".webm", ".db", ".lnk" };

        public int FileId
        {
            get
            {
                return _fileId;
            }
            set
            {
                _fileId = value;
            }
        }
        public int DirId
        {
            get
            {
                return _dirId;
            }
            set
            {
                _dirId = value;
            }
        }
        public string VarName
        {
            get
            {
                return _varName;
            }
            set
            {
                _varName = value;
            }
        }
        public string WorkingDir
        {
            get
            {
                return _workingDir;
            }
            set
            {
                _workingDir = value;
            }
        }
        public Dictionary<string, string> ExtFilter
        {
            get
            {
                return _extFilter;
            }
            set
            {
                _extFilter = value;
            }
        }
        public JsonJavaScriptBase()
        {
            ExtFilter = FileManager.GetImageExtensionFilter();
        }
        public JsonJavaScriptBase(int dirId, int fileId, string varName)
        {
            FileId = fileId;
            DirId = dirId;
            VarName = varName;
            ExtFilter = FileManager.GetImageExtensionFilter();
        }

        public void SetParameters(int dirId, int fileId, string varName, string workingDir, Dictionary<string, string> extFilter)
        {
            FileId = fileId;
            DirId = dirId;
            VarName = varName;
            WorkingDir = workingDir;
            ExtFilter = extFilter;
        }

        //public const string StartJSON = "JSON.parse('";
        public abstract string StartData { get; }
        public abstract string EndData { get; }
        public abstract string LineStart { get; }
        public abstract string LineEnd { get; }

        public string EntryFields(int listId, string listName)
        {
            return "\"" + ListId + "\":" + listId.ToString() + ",\"" + ListName + "\":\"" + listName + "\"";
        }

        public string Data(string id, string title, string src, string name)
        {
            return "{\"id\":\"" + id + "\", \"title\":\"" + title + "\", \"src\":\"" + src + "\", \"name\":\"" + name + "\"}";
        }
        public string Data(string id, string title, string src, string name, long size)
        {
            return "{\"id\":\"" + id + "\", \"title\":\"" + title + "\", \"src\":\"" + src + "\", \"name\":\"" + name + "\"" + ", \"size\":\"" + size.ToString() + "\"}";
            return JavaScriptFileItem.GetJson(id, title, src, name, size.ToString());
        }

        public string DirListStart()
        {
            return ",\"" + DirList + "\":[";
        }
        public string FileStart()
        {
            return ",\"" + FileList + "\":[";
        }
        public string ListEnd()
        {
            return "]";
        }
        public abstract string PreLine(DirectoryInfoType dir, int level, bool isFirst);
        public abstract string PostJson(DirectoryInfoType dir, int level, bool isFirst);
        public abstract List<string> PostJsonLines(DirectoryInfoType dir, int level, bool isFirst);

        public string PreLineGeneral(DirectoryInfoType dir, int level, bool isFirst, string start, string end)
        {
            return "".PadLeft(level, ' ') + start + (isFirst ? "{" : ",{") + EntryFields(DirId++, dir.Name) + DirListStart() + end;
        }
        public string PostJsonGeneral(DirectoryInfoType dir, int level, bool isFirst, string start, string end)
        {
            return StrList2Str(PostJsonLinesGeneral(dir, level, isFirst, start, end));
        }
        public static string StrList2Str(List<string> l)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in l)
                sb.Append(s);
            return sb.ToString();
        }

        public List<string> PostJsonLinesGeneral(DirectoryInfoType dir, int level, bool isFirst, string start, string end)
        {
            List<string> l = new List<string>();
            string sp = "".PadLeft(level, ' ');
            var imgList = dir.ChildFiles.Where(fi => !fi.Name.Contains("'") && !ExclusionExtensionList.Contains(fi.Extension.ToLower())).ToList();
            if (dir.ChildDirectories.Count > 0)
            {
                if (dir.ChildDirectories.Count > 0 && imgList.Count == 0)
                    l.Add(sp + start + ListEnd() + FileStart() + ListEnd() + "}" + end);

                if (dir.ChildDirectories.Count > 0 && imgList.Count > 0)
                    l.Add(sp + start + ListEnd() + FileStart() + end);
            }
            if (dir.ChildDirectories.Count == 0)
            {
                if (imgList.Count > 0)
                    l.Add(sp + start + (isFirst ? "{" : ",{") + EntryFields(DirId++, dir.Name) + DirListStart() + ListEnd() + FileStart() + end);

                if (imgList.Count == 0)
                    l.Add(sp + start + (isFirst ? "{" : ",{") + EntryFields(DirId++, dir.Name) + DirListStart() + ListEnd() + FileStart() + ListEnd() + "}" + end);
            }
            if (imgList.Count > 0)
            {
                bool isFirstInList = true;
                foreach (FileInfoType fi in imgList)
                {
                    //string ext_norm = fi.Extension.ToLower();
                    //if (!FileManager.isExtensionExcluded(ext_norm, extFilter) && !fi.Name.Contains("'"))
                    {
                        string src = FileManager.removeWorkingDirectory(fi.FullName.Replace("\\", "/"), WorkingDir);
                        l.Add(sp + start + (isFirstInList ? "" : ",") + JsonJavaScriptRecord.Data((FileId++).ToString(), "", src, fi.Name, fi.Length) + end);
                        isFirstInList = false;
                    }
                }
                l.Add(sp + start + JsonJavaScriptRecord.ListEnd() + "}" + end);
            }

            return l;
        }

        public abstract int Metode(Data p);
        public abstract void test<T>(T p);
        public abstract T RootField<T>(T p);
        public abstract string RootField(string p);
        public abstract List<string> RootField(List<string> p);
        //public abstract string BeginStruct(string VarName);
        //public abstract string ListLine(int listId, string listName, bool isFirst);
        //public abstract string DataLine(string id, string title, string src, string name, bool isFirst);
        //public abstract string EndList();
        //public abstract string EndStruct();


        public static string jsonString(DirectoryInfoType dir, string sWorkingDir, bool withParser)
        {
            IJsonJavaScript manager;//= (withParser ? new JsonHelperRecord0() : new JsonHelperRecord1());
            if (withParser)
                manager = new JsonHelperRecord0();
            else
                manager = new JsonHelperRecord1();
            manager.SetParameters(0, 900001, "data1", sWorkingDir, FileManager.GetImageExtensionFilter());
            return manager.RootField(manager.JsonString(dir, true, 0));
        }
        public string JsonString(DirectoryInfoType dir, bool isFirst, int level)
        {
            StringBuilder sb = new StringBuilder();
            if (dir.ChildDirectories.Count > 0)
            {
                sb.Append(PreLine(dir, level, isFirst));
                bool isFirstInList = true;
                foreach (DirectoryInfoType dirItem in dir.ChildDirectories)
                {
                    sb.Append(JsonString(dirItem, isFirstInList, (level + 1)));
                    isFirstInList = false;
                }
            }
            return sb.ToString() + PostJson(dir, level, isFirst);
        }
    }

    public class JsonHelperRecord0 : JsonJavaScriptBase
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
        //public override string StartData2 { get { return ""; } }
        //public override string EndData2   { get { return ";\r\n"; } }
        public override string LineStart { get { return "+'"; } }
        public override string LineEnd { get { return "'\r\n"; } }
        //public override string LineStart2 { get {return ""; } }
        //public override string LineEnd2   { get { return "\r\n"; } }

        public JsonHelperRecord0()
          : base()
        {

        }

        public JsonHelperRecord0(int dirId, int fileId, string varName)
          : base(dirId, fileId, varName)
        {

        }

        public override string PreLine(DirectoryInfoType dir, int level, bool isFirst)
        {
            return PreLineGeneral(dir, level, isFirst, LineStart, LineEnd);
        }

        public override string PostJson(DirectoryInfoType dir, int level, bool isFirst)
        {
            return PostJsonGeneral(dir, level, isFirst, LineStart, LineEnd);
        }

        public override List<string> PostJsonLines(DirectoryInfoType dir, int level, bool isFirst)
        {
            return PostJsonLinesGeneral(dir, level, isFirst, LineStart, LineEnd);
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
            return "const " + VarName + " = " + StartData + "{\"" + RootName + "\":[" + LineEnd + p + LineStart + "]}" + EndData;
        }
        public override List<string> RootField(List<string> p)
        {
            List<string> l = new List<string>();
            l.Add("const " + VarName + " = " + StartData + "{\"" + RootName + "\":[" + LineEnd);
            l.AddRange(p);
            l.Add(LineStart + "]}" + EndData);
            return l;
        }
    }


    public class JsonHelperRecord1 : JsonJavaScriptBase
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
        public override string StartData { get { return ""; } }
        public override string EndData { get { return ";\r\n"; } }
        public override string LineStart { get { return ""; } }
        public override string LineEnd { get { return "\r\n"; } }

        //public override string ListLine(int listId, string listName, bool isFirst)
        //{
        //return (isFirst ? "+ '" : "+',") + "{\"ListId\":" + listId.ToString() + ",\"ListName\":\"" + listName + "\",\"ListData\":['";
        //}
        //public override string DataLine(string id, string title, string src, string name, bool isFirst)
        //{
        //return (isFirst ? "+ '" : "+',") + "{\"id\":\"" + id + "\", \"title\":\"" + title + "\", \"src\":\"" + src + "\", \"name\":\"" + name + "\"}'";
        //}
        public JsonHelperRecord1()
          : base()
        {

        }
        public JsonHelperRecord1(int dirId, int fileId, string varName)
          : base(dirId, fileId, varName)
        {

        }
        public override string PreLine(DirectoryInfoType dir, int level, bool isFirst)
        {
            return PreLineGeneral(dir, level, isFirst, LineStart, LineEnd);
        }

        public override string PostJson(DirectoryInfoType dir, int level, bool isFirst)
        {
            return PostJsonGeneral(dir, level, isFirst, LineStart, LineEnd);
        }
        public override List<string> PostJsonLines(DirectoryInfoType dir, int level, bool isFirst)
        {
            return PostJsonLinesGeneral(dir, level, isFirst, LineStart, LineEnd);
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
            return "const " + VarName + " = " + StartData + "{\"" + RootName + "\":[" + LineEnd + p + LineStart + "]}" + EndData;
        }
        public override List<string> RootField(List<string> p)
        {
            List<string> l = new List<string>();
            l.Add("const " + VarName + " = " + StartData + "{\"" + RootName + "\":[" + LineEnd);
            l.AddRange(p);
            l.Add(LineStart + "]}" + EndData);
            return l;
        }
    }
}