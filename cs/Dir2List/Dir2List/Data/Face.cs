using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tools.Dir
{
    public class Face
    {
        public string name { get; set; }
        public string path { get; set; }
        public string file { get; set; }
        public Face() { }
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