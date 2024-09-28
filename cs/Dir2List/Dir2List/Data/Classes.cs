using System.Collections.Generic;

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
        public static string Data(string id, string title, string src, string name)
        {
            return "{\"id\":\"" + id + "\", \"title\":\"" + title + "\", \"src\":\"" + src + "\", \"name\":\"" + name + "\"}";
        }
        public static string Data(string id, string title, string src, string name, long size)
        {
            return "{\"id\":\"" + id + "\", \"title\":\"" + title + "\", \"src\":\"" + src + "\", \"name\":\"" + name + "\"" + ", \"size\":\"" + size.ToString() + "\"}";
            return JavaScriptFileItem.GetJson(id, title, src, name, size.ToString());
        }
        public static string GetItem(string name, List<FileInfoType> fileList, ref int nextDirId, ref int nextFileId, string sWorkingDir)
        {
            string ret = "";
            foreach (var fi in fileList)
                ret += (ret == "" ? "" : ",") + JsonJavaScriptRecord.Data((nextFileId++).ToString(), "", FileManager.removeWorkingDirectory(fi.FullName.Replace("\\", "/"), sWorkingDir), fi.Name, fi.Length);
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
        //public string GetJson()
        //{
            //return System.Text.Json.JsonSerializer.Serialize(this);
        //}
        public static string GetJson(string id, string title, string src, string name, string size)
        {
            return "{\"id\":\"" + id + "\", \"title\":\"" + title + "\", \"src\":\"" + src + "\", \"name\":\"" + name + "\"" + ", \"size\":\"" + size + "\"}";
        }
    }
    public class Data
    {
    }

    public class DirItem
    {
        public int ListId { get; set; }
        public string ListName { get; set; }
        public List<DirItem> DirList { get; set; }
        public List<JavaScriptFileItem> FileList { get; set; }
    }
    public class ListDirItem
    {
        public List<DirItem> Lists { get; set; }
    }
}