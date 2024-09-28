using System.Collections.Generic;
using System.Drawing;

namespace Tools.Dir
{
    public class JsonJavaScriptRecord
    {
        public const string RootName = "Lists";
        private const string ListId = "ListId";
        private const string ListName = "ListName";
        private const string DirList = "DirList";
        private const string FileList = "FileList";
        public static readonly List<string> ExclusionExtensionList = new List<string>() { ".webm", ".db", ".lnk" };
        //public const string StartField = "+'";
        //public const string EndField = "'";
        //public const string StartJSON = "JSON.parse('";
        //public const string EndJSON = "')";
        public static string RootField(string listRootContent)
        {
            return "{\"" + RootName + "\":[" + listRootContent + "]}";
        }
        public static string EntryFields(int listId, string listName)
        {
            return "\"" + ListId + "\":" + listId.ToString() + ",\"" + ListName + "\":\"" + listName + "\"";
        }
        public static string EntryFields(int listId, string listName, string dirList, string fileList)
        {
            return "\"" + ListId + "\":" + listId.ToString() + ",\"" + ListName + "\":\"" + listName + "\"" + ",\"" + DirList + "\":[" + dirList + "]" + ",\"" + FileList + "\":[" + fileList + "]";
        }
        public static string DirListStart()
        {
            return ",\"" + DirList + "\":[";
        }
        public static string FileStart()
        {
            return ",\"" + FileList + "\":[";
        }
        public static string ListEnd()
        {
            return "]";
        }
        public static string GetItem(string name, List<FileInfoType> fileList, ref int nextDirId, ref int nextFileId, string sWorkingDir)
        {
            string ret = "";
            foreach (var fi in fileList)
            {
                ret += (ret == "" ? "" : ",")
                        + JavaScriptFileItem.GetJson(
                            (nextFileId++).ToString(),
                            "",
                            FileManager.removeWorkingDirectory(fi.FullName.Replace("\\", "/"), sWorkingDir),
                            fi.Name,
                            fi.Length.ToString()
                        );
            }
            return "{" + JsonJavaScriptRecord.EntryFields(nextDirId++, name, "", ret) + "}";
        }
        public static string GetItem(Item item, ref int nextDirId, ref int nextFileId, ref bool first, string sWorkingDir)
        {
            string commaIfFirst = (first ? "" : ",");
            first = false;
            return commaIfFirst + GetItem(item.Name, item.FileList, ref nextDirId, ref nextFileId, sWorkingDir);
        }
    }
    public class JavaScriptFileItem
    {
        public string id { get; set; }
        public string title { get; set; }
        public string src { get; set; }
        public string name { get; set; }
        public string size { get; set; }
        public static JavaScriptFileItem GetFileItem(string id, string title, string src, string name, string size)
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
        //public string GetJson()
        //{
        //return System.Text.Json.JsonSerializer.Serialize(this);
        //}
        public static string GetJson(string id, string title, string src, string name, string size)
        {
            return "{\"id\":\"" + id + "\", \"title\":\"" + title + "\", \"src\":\"" + src + "\", \"name\":\"" + name + "\"" + ", \"size\":\"" + size + "\"}";
        }
        public string Json()
        {
            return "{\"id\":\"" + this.id + "\", \"title\":\"" + this.title + "\", \"src\":\"" + this.src + "\", \"name\":\"" + this.name + "\"" + ", \"size\":\"" + this.size + "\"}";
        }
    }
    public class Data
    {
    }

    public class JavaScriptDirItem
    {
        public int ListId { get; set; }
        public string ListName { get; set; }
        public List<JavaScriptDirItem> DirList { get; set; }
        public List<JavaScriptFileItem> FileList { get; set; }
        public static JavaScriptDirItem GetDirItem(int pListId, string pListName)
        {
            return new JavaScriptDirItem()
            {
                ListId = pListId,
                ListName = pListName,
                DirList = new List<JavaScriptDirItem>(),
                FileList = new List<JavaScriptFileItem>()
            };
        }
        public static JavaScriptDirItem GetDirItem(DirectoryInfoType dir, ref int pDirId, ref int pFileId, string pWorkingDir)
        {
            JavaScriptDirItem ret = new JavaScriptDirItem()
            {
                ListId = pDirId++,
                ListName = dir.Name,
                DirList = new List<JavaScriptDirItem>(),
                FileList = new List<JavaScriptFileItem>()
            };

            foreach (var fi in dir.ChildFiles)
            {
                //if (!FileManager.isExtensionExcluded(ext_norm, extFilter) && !fi.Name.Contains("'"))
                {
                    string src = FileManager.removeWorkingDirectory(fi.FullName.Replace("\\", "/"), pWorkingDir);
                    ret.FileList.Add(JavaScriptFileItem.GetFileItem((pFileId++).ToString(), "", src, fi.Name, fi.Length.ToString()));
                }
            }

            foreach (var d in dir.ChildDirectories)
                ret.DirList.Add(GetDirItem(d, ref pDirId, ref pFileId, pWorkingDir));

            return ret;
        }
    }
    public class ListDirItem
    {
        public List<JavaScriptDirItem> Lists { get; set; }
        public static ListDirItem GetDirItem0(DirectoryInfoType dir)
        {
            JavaScriptDirItem d = JavaScriptDirItem.GetDirItem(0, "Test");

            ListDirItem ret = new ListDirItem() { Lists = new List<JavaScriptDirItem>() { d } };

            d.FileList.Add(JavaScriptFileItem.GetFileItem("900060", "", "Download2/Test/AnyaMozok_face.jpg", "AnyaMozok_face.jpg", "6960"));
            d.FileList.Add(JavaScriptFileItem.GetFileItem("900061", "", "Download2/Test/MaciWinslett_tnzoom.gif", "MaciWinslett_tnzoom.gif", "16757"));
            d.DirList.Add(JavaScriptDirItem.GetDirItem(1, "Piger0"));
            d.DirList.Add(JavaScriptDirItem.GetDirItem(2, "Piger1"));
            d.DirList.Add(JavaScriptDirItem.GetDirItem(6, "Piger3"));

            return ret;
        }
        public static ListDirItem GetDirItem(DirectoryInfoType dir, int dirIdStart)
        {
            JavaScriptDirItem d = JavaScriptDirItem.GetDirItem(dirIdStart, dir.Name);

            ListDirItem ret = new ListDirItem() { Lists = new List<JavaScriptDirItem>() { d } };

            d.FileList.Add(JavaScriptFileItem.GetFileItem("900060", "", "Download2/Test/AnyaMozok_face.jpg", "AnyaMozok_face.jpg", "6960"));
            d.FileList.Add(JavaScriptFileItem.GetFileItem("900061", "", "Download2/Test/MaciWinslett_tnzoom.gif", "MaciWinslett_tnzoom.gif", "16757"));
            d.DirList.Add(JavaScriptDirItem.GetDirItem(1, "Piger0"));
            d.DirList.Add(JavaScriptDirItem.GetDirItem(2, "Piger1"));
            d.DirList.Add(JavaScriptDirItem.GetDirItem(6, "Piger3"));

            return ret;
            /*
            StringBuilder sb = new StringBuilder();
            if (dir.ChildDirectories.Count > 0)
            {
                sb.Append(manager.PreLine(dir, level, isFirst));
                bool isFirstInList = true;
                foreach (DirectoryInfoType dirItem in dir.ChildDirectories)
                {
                    sb.Append(jsonString(dirItem, manager, isFirstInList, (level + 1)));
                    isFirstInList = false;
                }
            }

            return sb.ToString() + manager.PostJson(dir, level, isFirst);










                        
            if (imgList.Count > 0)
            {
                bool isFirstInList = true;
                foreach (FileInfoType fi in imgList)
                {
                    //string ext_norm = fi.Extension.ToLower();
                }
                l.Add(sp + start + JsonJavaScriptRecord.ListEnd() + "}" + end);
            }

            */

        }
    }
}
