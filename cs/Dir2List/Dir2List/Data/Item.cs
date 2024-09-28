using System.Collections.Generic;

namespace Tools.Dir
{
    public class Item
    {
        const string KeyField = "Key";
        const string IdField = "Id";
        const string NameField = "Name";
        const string FileListField = "FileList";
        public string Key { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        //public List<string> Names { get; set; }
        public List<FileInfoType> FileList { get; set; }
        public Item()
        {
            //Names = new List<string>();
            FileList = new List<FileInfoType>();
        }
        public Item(string key, string pName, FileInfoType fi)
        {
            Key = key;
            Name = pName;
            //Names = new List<string>() { pName };
            FileList = new List<FileInfoType>() { fi };
        }
        public string GetJson(ref int nextDirId, ref int nextFileId, ref bool first, string sWorkingDir)
        {
            string ret = "";
            foreach (var fi in FileList)
                ret += (ret == "" ? "" : ",") + JsonJavaScriptRecord.Data((nextFileId++).ToString(), "", FileManager.removeWorkingDirectory(fi.FullName.Replace("\\", "/"), sWorkingDir), fi.Name, fi.Length);
            //ret = (first ? "" : ",") + "{\"" + KeyField + "\":\"" + Key + "\",\"" + IdField + "\":" + (nextDirId++).ToString() + ",\"" + NameField + "\":\"" + Name + "\",\"" + FileListField + "\":[" + ret + "]}";
            ret = (first ? "" : ",") + "{\"" + NameField + "\":\"" + Name + "\",\"" + KeyField + "\":\"" + Key + "\",\"" + IdField + "\":" + (nextDirId++).ToString() + ",\"" + FileListField + "\":[" + ret + "]}";
            first = false;
            return ret;
        }
    }
}