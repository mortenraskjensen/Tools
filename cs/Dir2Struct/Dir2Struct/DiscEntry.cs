using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// Not used jet
namespace Dir2Struct
{
    public class DiscEntry
    {
        public string DirPath { get; set; }
        public string Filter;
        private DirectoryInfo di;
        public FileInfo[] rgFiles;
        public DirectoryInfo[] rgDirs;
        public string DirName { get; set; }
        public DiscEntry(string path, string filter = "")
        {
            if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
            {
                di = new DirectoryInfo(path);
                load(di, filter);
            }
            else
            {
                di = null;
                rgFiles = null;
                rgDirs = null;
            }
        }
        public DiscEntry(DirectoryInfo pDi, string filter = "")
        {
            load(pDi, filter);
        }
        private bool load(DirectoryInfo pDi, string filter = "")
        {
            this.DirPath = di.FullName;
            this.Filter = filter;
            if (pDi.Exists)
            {
                di = pDi;
                rgFiles = di.GetFiles(Filter + "*");
                rgDirs = di.GetDirectories(filter + "*");
                DirName = di.Name;
                return true;
            }
            else
            {
                di = null;
                rgFiles = null;
                rgDirs = null;
                return false;
            }
        }
    }
}