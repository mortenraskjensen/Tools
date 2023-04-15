using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dir2List
{
    public interface IWork
    {
        const string jsonOutputFileName = "sharpjson.js";
        const string RootName = "Lists0";

        int FileId { get; set; }
        string VarName { get; set; }
        string RootPath { get; }
        string AndetDir { get; }
        string SubDir2 { get; }
        string TestDir { get; }
        public void test3(string pSubPath, string pRootPath);
        public void test4(string pSubPath, string pRootPath);
        public void test6(string pSubPath, string pRootPath);
        public void work();
    }
    public abstract class WorkBase : IWork
    {
        public const string jsonOutputFileName = "sharpjson.js";

        public const string RootName = "Lists";
        private int _fileId = 900001;
        private string _varName = "data1";

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
        public WorkBase()
        {
        }
        public WorkBase(int dirId, int fileId, string varName)
        {
            FileId = fileId;
            VarName = varName;
        }
        public abstract string RootPath { get; }
        public abstract string AndetDir { get; }
        public abstract string SubDir2 { get; }
        public abstract string TestDir { get; }

        public abstract void test3(string pSubPath, string pRootPath);
        public abstract void test4(string pSubPath, string pRootPath);
        public void test5(string subPath, string fileListFileName, string jsonFileName, string pRootPath)
        {
            FileManager fm = new FileManager();

            string rootPath = pRootPath;


            {
                DirectoryInfoType entry = fm.LoadEntry(rootPath + "/" + subPath, "");

                //Dictionary<string, int> dictFile2 = new Dictionary<string, int>();
                //List<string> fileNameList = new List<string>();
                //checkDir(entry.ChildDirectories, entry.FullName, FileManager.normalizeFileName(rootPath));


                bool succ = DirectoryListing.WriteFileList(entry, rootPath, fileListFileName);
                succ = DirectoryListing.WriteJsonFile(entry, rootPath, jsonFileName);
            }
        }
        public abstract void test6(string pSubPath, string pRootPath);
        public abstract void work();
    }

    public class Work : WorkBase
    {
        const string _RootPath = "C:";
        const string _SubDir1 = "Andet";
        const string _SubDir2 = "Windows10Upgrade";
        const string _SubDir3 = "Temp";
        public override string RootPath { get { return _RootPath; } }
        public override string AndetDir { get { return _SubDir1; } }
        public override string SubDir2 { get { return _SubDir2; } }
        public override string TestDir { get { return _SubDir3; } }


        public Work()
          : base()
        {

        }

        public Work(int dirId, int fileId, string varName)
          : base(dirId, fileId, varName)
        {

        }

        public override void test3(string pSubPath, string pRootPath)
        {
            string x = StringTools.NumberFixedLengthWithLeadingZeros(-123,5);

            string y = StringTools.FixedColumns(new string[3] { "AB","ABC","ABCDE" }, 3);

            string z = StringTools.FixedColumns(new string[3] { "AB", "ABC", "ABCDE" }, 3, false);

            DirectoryInfoType entry = (new FileManager()).LoadEntry(pRootPath + "/" + pSubPath, "");
            DirectoryInfoType.checkDir(entry);


            DirectoryListing.WriteJsonFileOnlyOneLevel(entry, pRootPath, pRootPath + "/" + "Temp/temp.js", FileManager.GetImageExtensionFilter());
            DirectoryListing.WriteJsonFileOnlyOneLevel(entry.GetSubDir("i386"), pRootPath, pRootPath + "/" + "Temp/resources.js", FileManager.AddExtensionFilter(new List<string>() { ".png" }));
            DirectoryListing.WriteJsonFileOnlyOneLevel(entry.GetSubDir("dll2"), pRootPath, pRootPath + "/" + "Temp/Skype.js", FileManager.AddExtensionFilter(new List<string>() { ".dll" }));

        }

        public override void test4(string subPath, string pRootPath)
        {
            FileManager fm = new FileManager();

            string rootPath = pRootPath;

            FileInfoType f = fm.GetCurrentDirectory();
            //List<FileInfoType> fl = fm.ListFiles(rootPath, "");
            //List<FileInfoType> filel = de.GetFileList();
            //checkDir(entry.ChildDirectories, entry.FullName, FileManager.normalizeFileName(rootPath));

            {
                DirectoryInfoType entry = fm.LoadEntry(rootPath + "/" + subPath, "");

                bool succ = false;

                succ = DirectoryListing.TestJsonFile(entry, rootPath, "jsonOutputFileName.js");
            }
        }
        public override void test6(string pSubPath, string pRootPath)
        {
            int nextDirId = 800001;
            int nextFileId = 900001;
            FileManager fm = new FileManager();
            DirectoryInfoType entry = fm.LoadEntry(pRootPath + "/" + pSubPath, "");
            //DirectoryListing.WriteJsonFile(entry, pRootPath, "face2");
            Dictionary<string, Item> dictFile = new Dictionary<string, Item>();
            var fileListlist = entry.GetFileList;
            string sWorkingDir = FileManager.normalizeFileName(pRootPath);
            bool first = true;
            string info = "";
            foreach (FileInfoType e in fileListlist)
                info += "\r\n" + JsonJavaScriptRecord.GetItem(new Item(e.Name, e.Name, e), ref nextDirId, ref nextFileId, ref first, sWorkingDir);


        }
        public override void work()
        {

        }
    }

}