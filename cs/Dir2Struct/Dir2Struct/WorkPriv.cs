using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace Tools.Dir2List
{
    public class Work0 : WorkBase
    {
        const string _RootPath = "L:\\Backup\\pic";
        const string _SubDir1 = "Download2/Andet";
        const string _SubDir2 = "Download2/Piger";
        const string _SubDir3 = "Download2/Test";
        public override string RootPath { get { return _RootPath; } }
        public override string AndetDir { get { return _SubDir1; } }
        public override string SubDir2 { get { return _SubDir2; } }
        public override string TestDir { get { return _SubDir3; } }


        public Work0()
          : base()
        {

        }

        public override void test3(string pSubPath, string pRootPath)
        {
            DirectoryInfoType entry = (new FileManager()).LoadEntry(pRootPath + "/" + pSubPath, "");

            DirectoryListing.WriteJsonFileOnlyOneLevel(entry, pSubPath, "piger.js", FileManager.AddExtensionFilter(new List<string>() { ".gif" }));
            DirectoryListing.WriteJsonFileOnlyOneLevel(entry.GetSubDir("Katia"), pSubPath, "Katia.js", FileManager.GetImageExtensionFilter());
            DirectoryListing.WriteJsonFileOnlyOneLevel(entry.GetSubDir("KaceyKox"), pSubPath, "KaceyKox.js", FileManager.GetImageExtensionFilter());
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

                //succ = DirectoryListing.WriteFileList(entry, rootPath, "dir.txt");
                //succ = DirectoryListing.WriteJsonFileOnlyOneLevel(entry, rootPath, jsonOutputFileName);
                //succ = DirectoryListing.WriteJsonFileOld(entry, rootPath, jsonOutputFileName);
                succ = DirectoryListing.TestJsonFileOld(entry, rootPath, jsonOutputFileName);
                //succ = DirectoryListing.WriteJsonFile(entry, rootPath, jsonOutputFileName);
                succ = DirectoryListing.TestJsonFile(entry, rootPath, jsonOutputFileName);
            }
        }
        public override void test6(string pSubPath, string pRootPath)
        {
            FileManager fm = new FileManager();
            DirectoryInfoType entry = fm.LoadEntry(pRootPath + "/" + pSubPath, "");
            DirectoryListing.WriteJsonFile(entry, pRootPath, "face2");
            WriteUniqueFileList(entry, pRootPath, "unique.txt");
        }

        public override void work()
        {
            test5("Download2/Test", "dirlistTest.txt", "jsonTest.js", RootPath);
            //test5("Download2/Andet", "dirlistAndet.txt", "jsonAndet.js", RootPath);
            //test5("Download2/Piger", "dirlistPiger.txt", "jsonPiger.js", RootPath);
            //test6("Download2/Andet", RootPath);
            //test7();
        }


        #region subs
        public List<string> getFilenameListFromDirectoryInfoType(DirectoryInfoType dir)
        {
            return dir.GetFileList.Select(h => h.Name).ToList();
        }
        public void test7()
        {
            const string testdata = "{\"faces\" : [\r\n{\"name\":\"AJApplegate\",\"path\":\"/Andet/Zzz_Blandet_gif_NoName/\",\"file\":\"AJApplegate_tnzoom.gif\"}" +
                ",{\"name\":\"ZoeySinn\",\"path\":\"/Andet/Zzz_Blandet_gif_NoName/\",\"file\":\"ZoeySinn_tnzoom.gif\"}\r\n]}";

            FaceType f = new FaceType();
            Face f1 = new Face("AJApplegate", "/Andet/Zzz_Blandet_gif_NoName/", "AJApplegate_tnzoom.gif");
            Face f2 = new Face("ZoeySinn", "/Andet/Zzz_Blandet_gif_NoName/", "ZoeySinn_tnzoom.gif");
            f.faces = new List<Face> { f1, f2 };
            //f.faces = new Face[2] {f1,f2 };
            //f.nr = 7;
            //f.faces.Add(new Face("AJApplegate", "/Andet/Zzz_Blandet_gif_NoName/", "AJApplegate_tnzoom.gif"));
            //string s = f.Serialize();
            string ret = "";
            var options = new JsonSerializerOptions { WriteIndented = true };
            System.Console.WriteLine(f.Serialize());
            FaceType f_ = new FaceType(testdata);
            System.Console.WriteLine(f_.Serialize());
            ret += JsonSerializer.Serialize(f) + "\r\n";
            ret += "\r\n";
            ret += JsonSerializer.Serialize(f_) + "\r\n";
        }

        private bool WriteUniqueFileList(DirectoryInfoType pEntry, string pRootPath, string jsonOutputFileName)
        {
            string inFile = Io.Read2String(pRootPath + "/" + "wrong.json");
            FaceType wrongFile = new FaceType(inFile);
            Dictionary<string, Face> wrongDict = new Dictionary<string, Face>();
            foreach (Face fi in wrongFile.faces)
            {
                string s = fi.name.ToLower();
                if (!wrongDict.ContainsKey(s))
                    wrongDict.Add(s, fi);
            }


            inFile = Io.Read2String(pRootPath + "/" + "face.json");
            FaceType f = new FaceType(inFile);
            Dictionary<string, Face> dict = new Dictionary<string, Face>();
            Dictionary<string, string> dictTen = new Dictionary<string, string>();

            foreach (Face fi in f.faces)
            {
                string s = fi.name.ToLower();
                if (!dict.ContainsKey(s))
                {
                    dict.Add(s, fi);
                    if (s.Length >= 10)
                    {
                        string ten = s.Substring(0, 10);
                        if (!dictTen.ContainsKey(ten))
                            dictTen.Add(ten, fi.name);
                    }
                }
            }

            bool bExtra = false;
            var facefiles = DirectoryListing.DirectoryInfoType2JavaScripFileItem(pEntry.GetSubDir("Zzz_Blandet_gif_NoName"), pRootPath);
            foreach (var fi in facefiles)
            {
                string fname = fi.name.Split('_')[0];
                string s = fname.ToLower();
                if (!dict.ContainsKey(s))
                {
                    Face f0 = new Face(fname, "/Andet/Zzz_Blandet_gif_NoName/", fi.name);
                    dict.Add(s, f0);
                    f.faces.Add(f0);
                    bExtra = true;
                    if (s.Length >= 10)
                    {
                        string ten = s.Substring(0, 10);
                        if (!dictTen.ContainsKey(ten))
                            dictTen.Add(ten, fi.name);
                    }
                }
            }
            if (bExtra)
            {
                int first3 = 0;
                List<string> newface = new List<string>();
                foreach (Face f0 in f.faces)
                    newface.Add((first3++ == 0 ? "{\"faces\" : [\r\n" : ",") + f0.GetJson());
                //newface.Add((first3++ == 0 ? "{\"faces\" : [\r\n" : ",") + (new Face(par.name, "/Andet/Zzz_Blandet_gif_NoName/", par.file)).GetJson());
                newface.Add("]}");
                FileManager.WriteAdjustedNewFile(pRootPath + "/" + "newface.json", newface);
            }


            Dictionary<string, string> extFilter = FileManager.GetImageExtensionFilter();
            Dictionary<string, Item> dictFile = new Dictionary<string, Item>();
            var fileListlist = pEntry.GetFileList;
            string sWorkingDir = FileManager.normalizeFileName(pRootPath);
            var regex1 = new Regex(@"\d");//No digits
            var regex2 = new Regex(Regex.Escape("("));//No "("
            foreach (FileInfoType fi in fileListlist)
            {
                string ext_norm = fi.Extension.ToLower();
                string fn = fi.Name.Replace(fi.Extension, "").Replace(" ", "").Replace(".", "").Replace("-", "");
                if (!FileManager.isExtensionExcluded(ext_norm, extFilter))
                    foreach (string si in fn.Split('_'))
                    {
                        if (!string.IsNullOrEmpty(si) && !regex1.IsMatch(si) && !regex2.IsMatch(si))
                        {
                            string s = si.ToLower();

                            if (!dictFile.ContainsKey(s))
                                dictFile.Add(s, new Item(s, si, fi));
                            else
                                dictFile[s].FileList.Add(fi);
                        }
                    }
            }

            bool first = true;
            int first2 = 0;

            int nextDirId = 800001;
            int nextFileId = 900001;
            List<Item> flist = new List<Item>();
            List<Item> flist1 = new List<Item>();
            List<Item> flist2 = new List<Item>();
            List<Item> flist3 = new List<Item>();
            List<Item> flist4 = new List<Item>();
            List<string> flist_ = new List<string>();
            List<string> wrongList = new List<string>();
            foreach (var item in dictFile.OrderBy(h => h.Key))
                if (dict.ContainsKey(item.Key))
                    flist.Add(item.Value);
                else if (//item.Value.FileList.Count == 1 && 
                    (wrongDict.ContainsKey(item.Key)
                    //item.Key.Length < 3 || item.Key.Length >= 25
                    ))
                    flist4.Add(item.Value);
                else if (item.Key.Length >= 10 && dictTen.ContainsKey(item.Key.Substring(0, 10)))
                    flist4.Add(item.Value);
                else if (item.Value.FileList.Count >= 5 && item.Key.Length > 2)
                    flist1.Add(item.Value);
                else if (item.Value.FileList.Count == 1 //&& (item.Key.Length < 3 || item.Key.Length >= 25)
                    )
                    flist3.Add(item.Value);
                else
                    flist2.Add(item.Value);

            //flist_.Add(GetItem(item, ref nextDirId, ref nextFileId, ref first, sWorkingDir));


            foreach (Item item in flist)
                flist_.Add(item.GetJson(ref nextDirId, ref nextFileId, ref first, sWorkingDir));
            FileManager.WriteAdjustedNewFile(pRootPath + "/" + "unique0.txt", flist_);
            flist_ = new List<string>();

            foreach (Item item in flist4)
            {
                flist_.Add(item.GetJson(ref nextDirId, ref nextFileId, ref first, sWorkingDir));
                //wrongList.Add((first2++ == 0 ? "{\"faces\" : [\r\n" : ",") + (new Face(item.Key, "", "")).GetJson());
            }
            FileManager.WriteAdjustedNewFile(pRootPath + "/" + "first10.txt", flist_);
            flist_ = new List<string>();

            foreach (Item item in flist1)
            {
                flist_.Add(item.GetJson(ref nextDirId, ref nextFileId, ref first, sWorkingDir));
                wrongList.Add((first2++ == 0 ? "{\"faces\" : [\r\n" : ",") + (new Face(item.Key, "", "")).GetJson());
            }
            FileManager.WriteAdjustedNewFile(pRootPath + "/" + "unique5.txt", flist_);
            flist_ = new List<string>();

            foreach (Item item in flist2)
            {
                flist_.Add(item.GetJson(ref nextDirId, ref nextFileId, ref first, sWorkingDir));
            }
            FileManager.WriteAdjustedNewFile(pRootPath + "/" + "unique2.txt", flist_);
            flist_ = new List<string>();
            foreach (Item item in flist3)
            {
                flist_.Add(item.GetJson(ref nextDirId, ref nextFileId, ref first, sWorkingDir));
                //wrongList.Add((first2++ == 0 ? "{\"faces\" : [\r\n" : ",") + (new Face(item.Key, "", "")).GetJson());
            }
            wrongList.Add("]}");
            FileManager.WriteAdjustedNewFile(pRootPath + "/" + "wrong.json", wrongList);
            return FileManager.WriteAdjustedNewFile(pRootPath + "/" + "unique1.txt", flist_);
        }

        #endregion subs
    }

}