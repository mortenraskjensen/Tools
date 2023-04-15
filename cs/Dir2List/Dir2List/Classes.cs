using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dir2List
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
    public class Face
    {
        public string name { get; set; }
        public string path { get; set; }
        public string file { get; set; }
        public Face() {}
        public Face(string pName, string pPath, string pFile) 
        { 
            name = pName;
            path = pPath;
            file = pFile;
        }
        public string GetJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
    public class FaceType
    {
        [JsonInclude]
        public List<Face> faces = new List<Face>();
        public FaceType()
        {
        }
        public FaceType(string json)
        {
            string json2 = json.Replace("\r\n", "");
            var f = JsonSerializer.Deserialize<FaceType>(json);
            faces = f.faces;
        }
        public string Serialize()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}