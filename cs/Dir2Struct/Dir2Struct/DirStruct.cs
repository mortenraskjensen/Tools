using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Collections;

namespace Dir2Struct
{
    public class LineStruct
    {
        public string Name { get; set; }
        public string EntryOutputPath { get; set; }
        public string EntryFullPath { get; set; }
        public string FirstFileName { get; set; }
        public string FileNameListCommaSeperated { get; set; }
        public List<string> FileNameList { get; set; }
    }
    public class DirStruct
    {
        const int maxListLength = 10;
        const int minDynamicDif = 20;

        const string pathprefix = "L:\\Backup\\pic\\";
        public string EntryOutputPath { get; set; }
        public string EntryFullPath { get; set; }
        public string Filter { get; set; }
        public List<LineStruct> Lines { get; set; }
        public DirStruct(string internalPath, string externalPath, string filter = "")
        {
            this.Filter = filter;
            this.EntryFullPath = internalPath;
            this.EntryOutputPath = externalPath;//format neaded for output
            this.Lines = new List<LineStruct>();
        }
        public static bool IfExists(string path)
        {
            return (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path));
        }
        public static bool IfFileExists(string path)
        {
            return (!string.IsNullOrWhiteSpace(path) && File.Exists(path));
        }
        public static DirectoryInfo GetDirectory(string path)
        {
            if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
                return new DirectoryInfo(path);
            return null;
        }
        private static bool HasValidExtension(string fileName)
        {
            return ThumpNailImage.IsValidImage_(fileName);
        }
        private static bool IsValidImage(FileInfo f)
        {
            string ext = f.Extension.ToLower();
            bool isValid = false;
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".png":
                    isValid = true;
                    break;
                default:
                    isValid = false;
                    break;
            }
            return isValid;
        }
        /*
        public static List<FileInfo[]> SplitFileArray(FileInfo[] fileArray)
        {
            List<FileInfo[]> ret = new List<FileInfo[]>();
            if (fileArray.Count() < maxListLength)
                return new List<FileInfo[]>() { fileArray };
            int counter = 0;
            List<FileInfo> fileList = new List<FileInfo>();
            foreach (FileInfo f in fileArray)
            {
                if (counter > maxListLength)
                {
                    ret.Add(fileList.ToArray());
                    fileList = new List<FileInfo>();
                    counter = 0;
                }
                if (IsValidImage(f))
                {
                    fileList.Add(f);
                    counter++;
                }
            }
            ret.Add(fileList.ToArray());
            //List<FileInfo> flist = files.ToList();

            return ret;
        }*/
        public static List<FileInfo[]> SplitFileArray(FileInfo[] fileArray, int minDynamic = -1)
        {
            int maxDynamic = (minDynamic < 0 ? maxListLength : minDynamic + minDynamicDif);
            List<FileInfo[]> ret = new List<FileInfo[]>();
            if (fileArray.Count() < maxDynamic)
                return new List<FileInfo[]>() { fileArray.Where(f => IsValidImage(f)).OrderBy(h => h.Name).ToArray() };
            int counter = 0;
            List<FileInfo> fileList = new List<FileInfo>();
            string oldFileName = null;
            int oldMatch = 0;
            foreach (FileInfo f in fileArray.OrderBy(h => h.Name))
            {
                int actualMatch = 0;
                if (counter > maxDynamic || (!IsFileNameClose(oldFileName, f.Name, 3, oldMatch, out actualMatch) && minDynamic > 0 && counter > minDynamic))
                {
                    ret.Add(fileList.ToArray());
                    fileList = new List<FileInfo>();
                    counter = 0;
                }
                if (IsValidImage(f))
                {
                    fileList.Add(f);
                    oldFileName = f.Name;
                    oldMatch = actualMatch;
                    counter++;
                }
            }
            ret.Add(fileList.ToArray());
            //List<FileInfo> flist = files.ToList();

            return ret;
        }
        private static bool IsFileNameClose(string a, string b, int len)
        {
            if (a == null || b == null)
                return false;
            int testLength = len;
            if (a.Length < testLength)
                testLength = a.Length;
            if (b.Length < testLength)
                testLength = b.Length;

            return a.Substring(0, testLength).Equals(b.Substring(0, testLength));
        }
        private static bool IsFileNameClose(string a, string b, int minLength, int lastLength, out int actual)
        {
            actual = FileNameMatchLength(a,b);
            if (actual >= (lastLength - 1) && actual >= minLength)
                return true;
            return false;
        }
        private static int FileNameMatchLength(string a, string b)
        {
            int match = 0;
            if (a == null || b == null)
                return match;

            for (int idx = 0; a.Length > idx && b.Length > idx && a[idx] == b[idx]; idx++)
                match = idx;

            return match;
        }
        public static List<string> AddHeaderAndFooter(List<string> plines)
        {
            List<string> l = new List<string>();
            string[] header = new string[6] { "function loadPage()", "{", "var i = 1;", "var jjj = 0;", "", "" };
            string[] footer = new string[6] { "", "", "}"
                        ,"document.addEventListener(\"DOMContentLoaded\", function(event) {"
                        ,"loadPage();", "});" };
            foreach (string ll in header) l.Add(ll);
            foreach (string ll in plines) l.Add(ll);
            foreach (string ll in footer) l.Add(ll);
            return l;
        }

        public static string stringlimit(string data, int maxlength)
        {
            return (data == null || data.Length <= maxlength ? data : data.Substring(0, maxlength));
        }
        public static string GetFileNamesSeperatedByComma(FileInfo[] files)
        {
            string outstr = "";
            if (files != null)
                foreach (var f in files)
                    if (!string.IsNullOrEmpty(f.Name))
                    {
                        if (string.IsNullOrEmpty(outstr))
                            outstr = f.Name;
                        else
                            outstr += "," + f.Name;
                    }
            return outstr;
        }

        /// <summary>
        /// I need to create a txt file with the content of the lines in lines and return its filename
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="n"></param>
        public static string WriteLines(List<string> lines, string outputFilenameWithFullPath)
        {
            StringBuilder s = new System.Text.StringBuilder();
            foreach (string l in lines)
                s.AppendLine(l);

            //using (StreamWriter w = File.AppendText(outputFilenameWithFullPath))
            //{
            //w.WriteLine(s);
            //}

            //I want the file overwritten
            string adjustedFileName = outputFilenameWithFullPath;// GetAdjustedFilePathIfFileExists(outputFilenameWithFullPath);

            WriteTextData(s.ToString(), adjustedFileName);

            return adjustedFileName;
        }

        public static void WriteTextData(string data, string file)
        {
            StreamWriter w = new System.IO.StreamWriter(file, false, Encoding.UTF8);
            w.WriteLine(data);
            w.Flush();
        }

        public static string GetAdjustedFilePathIfFileExists(string fullPath)
        {
            return fullPath;
        }
        public static string GetDirLine(string dirName, string filepathToFirstFile, string fileList)
        {
            return string.Format("SetImg(i++, \"{0}\", \"{1}\", \"{2}\");", dirName, filepathToFirstFile, fileList);
        }

        public static string GetLastDir(string path)
        {
            string ret = path.Replace("\\", "/");
            string[] a = ret.Split('/');
            for (int i = a.Count() - 1; i >= 0; i--)
                if (!string.IsNullOrEmpty(a[i]))
                    return a[i];
            return ("");
        }
        public static string[] GetDirList(string path)
        {
            string ret = path.Replace("\\", "/");
            string[] dirs = ret.Split('/');
            if (dirs != null)
                return dirs;
            return (new string[0]);
        }

        public static string AddLastBackSlash(string path)
        {
            string ch = path.Substring(path.Length - 1);
            return (ch.Equals("\\") || ch.Equals("/") ? path : path + "\\");
        }
        public static string GetOutputPath(string inputPath)
        {
            string outputPath = inputPath;
            int idx = inputPath.IndexOf(pathprefix);
            if (idx == 0)
                outputPath = outputPath.Substring(pathprefix.Length + idx);
            return outputPath.Replace("\\", "/");
        }
        /// <summary>
        /// I need a list of filenames from a directory
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="n"></param>
        public static List<string> GetFileNames(DirStruct ou)
        {
            return (from los in ou.Lines
                    select DirStruct.GetDirLine(los.Name, los.EntryOutputPath + los.FirstFileName, los.FileNameListCommaSeperated)).ToList();
        }
        public static List<string> GetFileNames(List<DirStruct> ouList)
        {
            List<string> ret = new List<string>();
            foreach (DirStruct ou in ouList)
                foreach (LineStruct los in ou.Lines)
                    ret.Add(DirStruct.GetDirLine(los.Name, los.EntryOutputPath + los.FirstFileName, los.FileNameListCommaSeperated));
            return ret;
        }
        public static List<string> GetFileNames2(string path, string filter = "")
        {
            List<string> l = new List<string>();
            DirectoryInfo di2 = DirStruct.GetDirectory(path);
            if (di2 != null)
            {
                FileInfo[] fileArray = di2.GetFiles(filter + "*");
                if (fileArray != null)
                    foreach (var f in fileArray)
                    {
                        string s = DirStruct.GetDirLine(DirStruct.stringlimit(f.Name, 15), DirStruct.GetOutputPath(path) + f.Name, f.Name);
                        if (!string.IsNullOrEmpty(s))
                            l.Add(s);
                    }
            }
            return l;
        }
        public static List<string> GetFileNames(string path, string outputPath, string filter = "")
        {
            List<string> l = new List<string>();
            DirectoryInfo di2 = DirStruct.GetDirectory(path);
            if (di2 != null)
            {
                FileInfo[] fileArray = di2.GetFiles(filter + "*");
                if (fileArray != null)
                    foreach (var f in fileArray)
                    {
                        string s = DirStruct.GetDirLine(DirStruct.stringlimit(f.Name, 15), outputPath + f.Name, f.Name);
                        if (!string.IsNullOrEmpty(s))
                            l.Add(s);
                    }
            }
            return l;
        }
        public static string RemoveExtension(string fileName)
        {
            int idx = fileName.LastIndexOf(".");
            return (idx > 0 ? fileName.Substring(0, idx) : fileName);
        }
        public static void CreatePath(string path)
        {
            string[] dirs = GetDirList(path);
            string path2 = "";
            foreach (string dir in dirs)
            {
                if (!string.IsNullOrEmpty(dir))
                {
                    if (!IfExists(path2 + dir + "/"))
                    {
                        DirectoryInfo di1 = DirStruct.GetDirectory(path2);
                        di1.CreateSubdirectory(dir);
                    }
                    path2 += dir + "/";
                }
            }
        }
        public static List<string[]> GetFileListPath2(string pathInput, string pathOutput, string filter = "")
        {
            List<string[]> ret = new List<string[]>();
            string inPath = DirStruct.AddLastBackSlash(pathInput);
            string outPath = DirStruct.AddLastBackSlash(pathOutput);
            CreatePath(pathOutput);
            DirectoryInfo di1 = DirStruct.GetDirectory(inPath);
            if (di1 != null)
            {
                DirectoryInfo[] dirArray = di1.GetDirectories(filter + "*");
                FileInfo[] fileArray = di1.GetFiles(filter + "*");
                if (IfExists(outPath) && fileArray != null)
                    foreach (FileInfo f in fileArray)
                        if (!IfFileExists(outPath + f.Name))//Already exists
                            ret.Add(new string[3] { inPath + f.Name, "300", outPath + f.Name });
            }
            return ret;
        }
        public static List<string[]> GetFileListPath(string pathInput, string pathOutput, string filter = "")
        {
            string inPath = DirStruct.AddLastBackSlash(pathInput);
            string outPath = DirStruct.AddLastBackSlash(pathOutput);
            DirectoryInfo di1 = DirStruct.GetDirectory(inPath);
            return GetSubFileListPath(di1, inPath, outPath, filter);
        }
        public static List<string[]> GetSubFileListPath(DirectoryInfo di1, string inPathSub, string outPathSub, string filter)
        {
            List<string[]> ret = new List<string[]>();
            if (di1 != null)
            {
                CreatePath(outPathSub);
                int dubletter = 0;
                DirectoryInfo[] dirArray = di1.GetDirectories(filter + "*");
                FileInfo[] fileArray = di1.GetFiles(filter + "*");
                Dictionary<string, bool> fileTargetDict = GetFileNameDict(outPathSub);
                if (IfExists(outPathSub) && fileArray != null)
                {
                    foreach (FileInfo f in fileArray)
                        if (IsValidImage(f))
                            //if (!IfFileExists(outPathSub + f.Name))//Already exists
                            if (!fileTargetDict.ContainsKey(f.Name))//Already exists
                                    ret.Add(new string[3] { inPathSub + f.Name, "300", outPathSub + f.Name });
                            else
                            {
                                dubletter++;
                                fileTargetDict[f.Name] = false;
                            }
                    foreach (var fp in fileTargetDict)
                        if (fp.Value)
                            ret.Add(new string[4] { outPathSub + fp.Key, fp.Key, outPathSub + fp.Key, "delete" });
                }
                foreach (DirectoryInfo d in dirArray)
                {
                    List<string[]> ret2 = GetSubFileListPath(d, inPathSub + d.Name + "\\", outPathSub + d.Name + "\\", filter);
                    foreach (var l in ret2)
                        ret.Add(l);
                }
            }
            return ret;
        }

        private static Dictionary<string, bool> GetFileNameDict(string dirPath)
        {
            Dictionary<string, bool> ret = new Dictionary<string, bool>();
            {
                DirectoryInfo di = DirStruct.GetDirectory(dirPath);
                FileInfo[] fileThumbsArray = di.GetFiles("*");
                if (fileThumbsArray != null)
                    foreach(FileInfo f in fileThumbsArray)
                        ret.Add(f.Name, true);
            }
            return ret;
        }

        public static void AddFileNames(DirStruct ou, List<string> dir, int minDynamic = -1)
        {
            string los_EntryFullPath = ou.EntryFullPath;
            string los_EntryOutputPath = ou.EntryOutputPath;

            foreach (var d in dir)
            {
                los_EntryFullPath = los_EntryFullPath + d + "\\";
                los_EntryOutputPath = los_EntryOutputPath + d + "/";
            };

            string los_Name = DirStruct.GetLastDir(los_EntryFullPath);

            DirectoryInfo diVar = DirStruct.GetDirectory(los_EntryFullPath);
            if (diVar != null)
            {
                int counter = 0;
                FileInfo[] fileArayTotal = diVar.GetFiles(ou.Filter + "*");
                if (fileArayTotal != null && fileArayTotal.Count() > 0)
                    foreach (FileInfo[] fileAray in DirStruct.SplitFileArray(fileArayTotal, minDynamic))
                        if (fileAray.Count() > 0)
                        {
                            counter++;
                            LineStruct los = new LineStruct()
                            {
                                EntryFullPath = los_EntryFullPath,
                                EntryOutputPath = los_EntryOutputPath,
                                Name = (counter > 1 ? string.Format("{0}{1}", DirStruct.stringlimit(los_Name, 15), counter) : DirStruct.stringlimit(los_Name, 15)),
                                FirstFileName = fileAray.FirstOrDefault().Name,
                                FileNameList = (from f in fileAray select f.Name).ToList(),
                                FileNameListCommaSeperated = DirStruct.GetFileNamesSeperatedByComma(fileAray),
                            };
                            ou.Lines.Add(los);
                        }
                DirectoryInfo[] dirArray = diVar.GetDirectories(ou.Filter + "*");
                if (dirArray != null)
                {
                    foreach (var d in dirArray)
                    {
                        List<string> dir2 = (from dd in dir select dd).ToList();
                        dir2.Add(d.Name);
                        AddFileNames(ou, dir2, minDynamic);
                    };
                }
            }
        }
    }
}