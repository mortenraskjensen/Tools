using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Dir
{
    #region FileManager
    public class FileManager
    {
        private string EntryPath;
        private string Filter;
        private DirectoryInfoType Entry;
        public Dictionary<string, DirectoryInfoType> DirDict = new Dictionary<string, DirectoryInfoType>();
        public DiscEntry discEntry = new DiscEntry();
        Dictionary<string, string> ExtensionFilter = null;
        public FileManager()
        {
        }
        public DirectoryInfoType LoadEntry(string entryPath, string filter)
        {
            EntryPath = entryPath;
            Filter = filter;
            Entry = null;
            if (Directory.Exists(entryPath))
                Entry = GetDirectoryInfoType(new DirectoryInfo(entryPath), filter);

            return Entry;
        }
        public DirectoryInfoType GetDirectoryInfoType(DirectoryInfo di, string filter)
        {
            DirectoryInfoType ret = null;
            if (di != null)
            {
                if (!DirDict.ContainsKey(di.FullName))
                {

                    ret = new DirectoryInfoType(di);
                    DirDict.Add(di.FullName, ret);

                    //DirectoryInfoType parent = GetDirectoryInfoType(di.Parent);
                    //DirectoryInfoType root = GetDirectoryInfoType(di.Root);
                    FileInfo[] rgFiles = di.GetFiles(filter + "*");
                    DirectoryInfo[] rgDirs = di.GetDirectories(filter + "*");
                    foreach (FileInfo fi in rgFiles)
                    {
                        ret.ChildFiles.Add(new FileInfoType(fi));
                        //filesList.Add(Tuple.Create(fi.Name));
                    }
                    foreach (DirectoryInfo d in rgDirs)
                    {
                        ret.ChildDirectories.Add(GetDirectoryInfoType(d, filter));
                    }
                }

                return DirDict[di.FullName];
            }
            return null;
        }




        public List<FileInfoType> ListFiles(string path, string filter)
        {
            //var filesList = new List<Tuple<string>>();
            List<FileInfoType> filesList = new List<FileInfoType>();

            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] rgFiles = di.GetFiles(filter + "*");
                DirectoryInfo[] rgDirs = di.GetDirectories(filter + "*");

                foreach (FileInfo fi in rgFiles)
                {
                    filesList.Add(DiscEntry.GetFileInfoType(fi));
                    //filesList.Add(Tuple.Create(fi.Name));
                }
            }
            return filesList;
        }
        public void AddExtensionFilter_(List<string> el)
        {
            ExtensionFilter = AddExtensionFilter(el);
        }
        public bool isExtensionExcluded(string ext)
        {
            return isExtensionExcluded(ext, ExtensionFilter);
        }
        public static Dictionary<string, string> AddExtensionFilter(List<string> el)
        {
            Dictionary<string, string> ExtensionFilterDict = null;
            if (el != null)
            {
                ExtensionFilterDict = new Dictionary<string, string>();
                foreach (string ext in el)
                    if (!string.IsNullOrWhiteSpace(ext))
                    {
                        string extKey = (".".Equals(ext.Substring(0, 1)) ? ext.ToLower() : "." + ext.ToLower());
                        if (!ExtensionFilterDict.ContainsKey(extKey))
                            ExtensionFilterDict.Add(extKey, ext);
                    }
                if (ExtensionFilterDict.Count == 0)
                    ExtensionFilterDict = null;//No usable extensions, so no filter!
            }
            else
            {
                ExtensionFilterDict = null;//The filter is turned off!
            }
            return ExtensionFilterDict;
        }
        public static bool isExtensionExcluded(string ext, Dictionary<string, string> ExtensionFilterDict)
        {
            if (ExtensionFilterDict != null && !ExtensionFilterDict.ContainsKey(ext.ToLower()))
                return true;

            return false;
        }
        public static Dictionary<string, string> GetImageExtensionFilter()
        {
            return FileManager.AddExtensionFilter(new List<string>() { ".gif", ".jpg", ".webp", ".jpeg", ".png", ".webm", ".jfif", ".svg", ".bmp" });
            //return FileManager.AddExtensionFilter(new List<string>() { ".php", "jpe", "014", "pdf" });
            //return FileManager.AddExtensionFilter(new List<string>() { ".webm", ".db", ".lnk" });
            //return FileManager.AddExtensionFilter(new List<string>() { ".webm", ".mp4", ".htm", ".bmp" });
        }

        public FileInfoType GetCurrentDirectory()
        {
            var execDirectoryPath = Path.GetDirectoryName(GetCodeBase())?.Replace("file:\\", "");
            string path = GetCodeBase();
            //path = Assembly.GetExecutingAssembly().Location;
            //path = Application.ResourceAssembly.Location;
            //path = System.Windows.Forms.Application.StartupPath;
            //path = Process.GetCurrentProcess().MainModule.FileName;


            return new FileInfoType()
            {
                FullName = path,
                DirectoryName = System.IO.Path.GetDirectoryName(path),
                Name = System.IO.Path.GetDirectoryName(path),
            };
            //if you need the directory, use 
            //System.IO.Path.GetDirectoryName
            //on that result.
            //Or, there's the shorter Application.ExecutablePath which "Gets the path 
        }
        public string GetCodeBase()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
        }

        public static bool DirExists(string dir)
        {
            return Directory.Exists(dir);
        }
        public static bool WriteAdjustedNewFile(string path, string data)
        {
            string adjustedFileName = GetAdjustedFilePathIfFileExists(path);
            return WriteNewFile(adjustedFileName, data);
        }
        public static bool WriteAdjustedNewFile(string path, List<string> data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in data)
                sb.AppendLine(s);
            return WriteAdjustedNewFile(path, sb.ToString());
        }
        public static bool WriteNewFile(string path, string data, string parentDir)
        {
            if (!Directory.Exists(parentDir))
                return false;
            return WriteNewFile(path, data);
        }
        public static bool WriteNewFile(string path, List<string> data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in data)
                sb.AppendLine(s);
            return WriteNewFile(path, sb.ToString());
        }
        public static bool WriteNewFile(string path, string data)
        {
            if (string.IsNullOrWhiteSpace(path) || File.Exists(path))
                return false;
            try {
                //File.Create(path);
                TextWriter tw = new StreamWriter(path);
                tw.Write(data);
                tw.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void WriteFile(string path, string data)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = @"E:\AppServ\Example.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
                TextWriter tw = new StreamWriter(path);
                tw.Write(data);
                tw.Close();
            }
            else if (File.Exists(path))
            {
                {
                    using (TextWriter tw = new StreamWriter(path, true))
                    {
                        tw.Write(data);
                        tw.Close();
                    }
                }
            }
        }
        public void AppendToLogFile(string filename, string loglines)
        {
            if (string.IsNullOrWhiteSpace(filename))
                filename = @"C:\\log.txt";
            if (string.IsNullOrWhiteSpace(loglines))
                loglines = "hello world\n";
            File.AppendAllText(filename, loglines);
        }
        public static string normalizeDir(string dir)
        {
            string retdir = dir.ToLower().Replace("\\", "/");
            int dirLength = retdir.Length;
            if (retdir[dirLength - 1].Equals('/'))
                retdir = retdir.Substring(0, dirLength - 1);

            return retdir;
        }
        public static string normalizeFileName(string file)
        {
            return file.ToLower().Replace("\\", "/");
        }
        public static string GetAdjustedFilePathIfFileExists(string filePath)
        {
            var duplicateCount = 2;
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            var folderUnc = Path.GetDirectoryName(filePath);

            while (File.Exists(filePath) || Directory.Exists(filePath))
            {
                var duplicateFileName = string.Format("{0}({1}){2}", fileName, duplicateCount++, extension);
                filePath = Path.Combine(folderUnc, duplicateFileName);
            }

            return filePath;
        }
        public static string removeWorkingDirectory(string path, string workingdirectory)
        {
            if (!string.IsNullOrWhiteSpace(workingdirectory) && !string.IsNullOrWhiteSpace(path))
                if (path.Length > workingdirectory.Length)
                {
                    string path_norm = FileManager.normalizeFileName(path);
                    if (path_norm.StartsWith(workingdirectory + "/"))
                        return path.Substring(workingdirectory.Length + 1);
                }
            return path;
        }
    }
    #endregion FileManager
    #region FileClases
    public enum KnownExtensions
    { 
        jpg = 1,
        gif = 2,
        jpeg = 3,
        png = 4,
        bmp = 5,
        webm = 6,
        db = 7,
        lnk = 8,
    }
    public class FileInfoType
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        //public DirectoryInfoType Directory { get; set; }
        public string DirectoryName { get; set; }
        public long Length { get; set; }
        public string Extension { get; set; }
        public KnownExtensions ExtensionType { get; set; }
        public FileInfoType()
        {

        }
        public FileInfoType(FileInfo fi)
        {
            Name = fi.Name;
            //Directory = directory;
            DirectoryName = fi.DirectoryName;
            FullName = fi.FullName;
            Length = fi.Length;
            Extension = fi.Extension;
            switch (fi.Extension)
            {
                case ".gif": ExtensionType = KnownExtensions.gif; break;
                case ".jpg": ExtensionType = KnownExtensions.jpg; break;
                case ".webm": ExtensionType = KnownExtensions.webm; break;
                case ".jpeg": ExtensionType = KnownExtensions.jpeg; break;
                case ".png": ExtensionType = KnownExtensions.png; break;
                case ".bmp": ExtensionType = KnownExtensions.bmp; break;
                case ".db": ExtensionType = KnownExtensions.db; break;
                case ".lnk": ExtensionType = KnownExtensions.lnk; break;
            }
        }
        //public FileInfoType(FileInfo fi, DirectoryInfoType directory)
        public string Show()
        {
            return FullName;
        }
        //public string Path { get; set; }
        //public string RelativeFilePath { get; set; }
    }
    public class DirectoryInfoType
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Extension { get; set; }
        //public DirectoryInfoType Parent { get; set; }
        //public DirectoryInfoType Root { get; set; }
        public List<DirectoryInfoType> ChildDirectories = null;
        public List<FileInfoType> ChildFiles = null;
        public List<FileInfoType> GetFileList
        {
            get
            {
                return GetFileListRecursive(this);
            }
        }


        public DirectoryInfoType()
        {

        }
        public DirectoryInfoType(DirectoryInfo d)
        {
            Name = d.Name;
            FullName = d.FullName;
            Extension = d.Extension;
            ChildFiles = new List<FileInfoType>();
            ChildDirectories = new List<DirectoryInfoType>();
        }
        public DirectoryInfoType(DirectoryInfo d, DirectoryInfoType parent, DirectoryInfoType root
            , List<DirectoryInfoType> childDirectories, List<FileInfoType> childFiles)
        {
            Name = d.Name;
            FullName = d.FullName;
            Extension = d.Extension;
            //Parent = parent;
            //Root = root;
            ChildDirectories = childDirectories;
            ChildFiles = childFiles;
        }
        public static List<FileInfoType> GetFileListRecursiveWithFilter(DirectoryInfoType dir)
        {
            List<FileInfoType> returnList = new List<FileInfoType>();
            foreach (DirectoryInfoType dirItem in dir.ChildDirectories)
                returnList.AddRange(GetFileListRecursiveWithFilter(dirItem));
            var imgList = dir.ChildFiles.Where(fi => !fi.Name.Contains("'") && !JsonJavaScriptRecord.ExclusionExtensionList.Contains(fi.Extension.ToLower())).ToList();
            foreach (FileInfoType fi in imgList)
                returnList.Add(fi);
            return returnList;
        }
        public static List<FileInfoType> GetFileListRecursive(DirectoryInfoType dir)
        {
            List<FileInfoType> returnList = new List<FileInfoType>();
            foreach (DirectoryInfoType dirItem in dir.ChildDirectories)
                returnList.AddRange(GetFileListRecursive(dirItem));
            foreach (FileInfoType fi in dir.ChildFiles)
                returnList.Add(fi);
            return returnList;
        }
        public string Show()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.AppendLine(string.Format("{0} filer:{1} dirs:{2} parent:{3} fuld:{4}", Name, ChildFiles.Count, ChildDirectories.Count, "", FullName));
            sb.AppendLine(string.Format("{0}", FullName));
            foreach (var f in ChildFiles)
                sb.AppendLine(f.Show());

            foreach (var c in ChildDirectories)
                sb.AppendLine(c.Show());

            return sb.ToString();
        }
        public DirectoryInfoType GetSubDir(string dirName)
        {
            dirName = dirName.ToLower();
            if (dirName.Equals(Name.ToLower()))
                return this;
            foreach (DirectoryInfoType c in ChildDirectories)
            {
                DirectoryInfoType d;
                if ((d = c.GetSubDir(dirName)) != null)
                    return d;
            }
            return null;
        }
        public static void checkDir(DirectoryInfoType dir)
        {
            //Maybe only for test
            Dictionary<string, string> extFilter = FileManager.GetImageExtensionFilter();
            Dictionary<string, int> allExtensions = new Dictionary<string, int>();
            Dictionary<string, string> ekskluded = new Dictionary<string, string>();


            if (dir.ChildDirectories.Count > 0)
            {
                foreach (DirectoryInfoType dirItem in dir.ChildDirectories)
                {
                    foreach (FileInfoType fi in dirItem.ChildFiles)
                    {
                        string ext_norm = fi.Extension.ToLower();
                        if (!FileManager.isExtensionExcluded(ext_norm, extFilter)
                                && !fi.Name.Contains("'"))
                        {
                            if (!allExtensions.ContainsKey(ext_norm))
                                allExtensions.Add(ext_norm, 1);
                            else
                                allExtensions[ext_norm]++;

                            if (FileManager.isExtensionExcluded(ext_norm, extFilter))
                            {
                                if (!ekskluded.ContainsKey(ext_norm))
                                    ekskluded.Add(ext_norm, fi.Extension);
                            }
                        }
                    }
                }
            }
            Console.WriteLine(Util.Show(ekskluded));
            Console.WriteLine(Util.Show(allExtensions));
        }
        //public string Path { get; set; }
        //public string RelativeFilePath { get; set; }
    }
    #endregion FileClases
    #region Other
    public class DiscEntry
    {
        public string DirPath { get; set; }
        public string Filter;
        DirectoryInfo di;
        FileInfo[] rgFiles;
        DirectoryInfo[] rgDirs;
        public string DirName { get; set; }
        //public List<FileInfoType> FileList { get; set; }
        //public List<FileInfoType> FolderList { get; set; }
        public DiscEntry()
        {
        }
        public void Load(string path, string filter)
        {
            this.DirPath = path;
            this.Filter = filter;
            if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
            {
                di = new DirectoryInfo(path);
                rgFiles = di.GetFiles(Filter + "*");
                rgDirs = di.GetDirectories(filter + "*");
            }
            else {
                di = null;
                rgFiles = null;
                rgDirs = null;
            }
        }
        public List<FileInfoType> GetFileList()
        {
            List<FileInfoType> filelist = new List<FileInfoType>();
            if (rgFiles != null && di != null)
                foreach (FileInfo fi in rgFiles)
                      filelist.Add(GetFileInfoType(fi));
            return filelist;
        }
        public List<DirectoryInfoType> GetDirList()
        {
            List<DirectoryInfoType> dirlist = new List<DirectoryInfoType>();
            if (rgDirs != null && di != null)
                foreach (DirectoryInfo d in rgDirs)
                    dirlist.Add(GetDirectoryInfoType(d));
            return dirlist;
        }
        public static FileInfoType GetFileInfoType(FileInfo fi)
        {
            return new FileInfoType(fi);
        }
        public static DirectoryInfoType GetDirectoryInfoType(DirectoryInfo dinfo)
        {
            return new DirectoryInfoType()
            {
                Name = dinfo.Name,//
                //Parent = GetDirectoryInfoType(dinfo.Parent),
                //Root = GetDirectoryInfoType(dinfo.Root),
                FullName = dinfo.FullName,
                Extension = dinfo.Extension,
            };
        }
    }
    public class Util
    {
        public static string Show(Dictionary<string,int> dict)
        {
            string ud = "";
            if (dict != null)
                foreach (var ext in dict)
                    ud += string.Format("{0} {1}\r\n",ext.Key,ext.Value);
            return ud;
        }
        public static string Show(Dictionary<string, string> dict)
        {
            string ud = "";
            if (dict != null)
                foreach (var ext in dict)
                    ud += string.Format("{0} ", ext.Key);
            return ud;
        }
    }
    #endregion Other
    #region NotUsed
    public class CreateFileOrFolder
    {
        static void Doit()
        {
            // Specify a name for your top-level folder.
            string folderName = @"c:\Top-Level Folder";

            // To create a string that specifies the path to a subfolder under your
            // top-level folder, add a name for the subfolder to folderName.
            string pathString = System.IO.Path.Combine(folderName, "SubFolder");

            // You can write out the path name directly instead of using the Combine
            // method. Combine just makes the process easier.
            string pathString2 = @"c:\Top-Level Folder\SubFolder2";

            // You can extend the depth of your path if you want to.
            //pathString = System.IO.Path.Combine(pathString, "SubSubFolder");

            // Create the subfolder. You can verify in File Explorer that you have this
            // structure in the C: drive.
            //    Local Disk (C:)
            //        Top-Level Folder
            //            SubFolder
            System.IO.Directory.CreateDirectory(pathString);

            // Create a file name for the file you want to create.
            string fileName = System.IO.Path.GetRandomFileName();

            // This example uses a random string for the name, but you also can specify
            // a particular name.
            //string fileName = "MyNewFile.txt";

            // Use Combine again to add the file name to the path.
            pathString = System.IO.Path.Combine(pathString, fileName);

            // Verify the path that you have constructed.
            Console.WriteLine("Path to my file: {0}\n", pathString);

            // Check that the file doesn't already exist. If it doesn't exist, create
            // the file and write integers 0 - 99 to it.
            // DANGER: System.IO.File.Create will overwrite the file if it already exists.
            // This could happen even with random file names, although it is unlikely.
            if (!System.IO.File.Exists(pathString))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(pathString))
                {
                    for (byte i = 0; i < 100; i++)
                    {
                        fs.WriteByte(i);
                    }
                }
            }
            else
            {
                Console.WriteLine("File \"{0}\" already exists.", fileName);
                return;
            }

            // Read and display the data from your file.
            try
            {
                byte[] readBuffer = System.IO.File.ReadAllBytes(pathString);
                foreach (byte b in readBuffer)
                {
                    Console.Write(b + " ");
                }
                Console.WriteLine();
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
            }

            // Keep the console window open in debug mode.
            System.Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }
        // Sample output:

        // Path to my file: c:\Top-Level Folder\SubFolder\ttxvauxe.vv0

        //0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29
        //30 31 32 33 34 35 36 37 38 39 40 41 42 43 44 45 46 47 48 49 50 51 52 53 54 55 56
        // 57 58 59 60 61 62 63 64 65 66 67 68 69 70 71 72 73 74 75 76 77 78 79 80 81 82 8
        //3 84 85 86 87 88 89 90 91 92 93 94 95 96 97 98 99
    }

    /*
    class WriteAllLines
    {
        public static async Task ExampleAsync()
        {
            string[] lines =
            {
            "First line", "Second line", "Third line"
        };

            await File.WriteAllLinesAsync("WriteLines.txt", lines);
        }
    }
    class WriteAllText
    {
        public static async Task ExampleAsync()
        {
            string text =
                "A class is the most powerful data type in C#. Like a structure, " +
                "a class defines the data and behavior of the data type. ";

            await File.WriteAllTextAsync("WriteText.txt", text);
        }
    }
    class StreamWriterOne
    {
        public static async Task ExampleAsync()
        {
            string[] lines = { "First line", "Second line", "Third line" };
            using StreamWriter file = new ("WriteLines2.txt");

            foreach (string line in lines)
            {
                if (!line.Contains("Second"))
                {
                    await file.WriteLineAsync(line);
                }
            }
        }
    }

    class StreamWriterTwo
    {
        public static async Task ExampleAsync()
        {
            using StreamWriter file = new ("WriteLines2.txt", append: true);
            await file.WriteLineAsync("Fourth line");
        }
    }
    */
    #endregion NotUsed
}