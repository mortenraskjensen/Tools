using System.Collections.Generic;
using System.Text;

namespace Tools.Dir
{
    public class JsonJavaScriptRecord0
    {
        public static string BeginStruct(string VarName)
        {
            return "const " + VarName + " = JSON.parse('{\"Lists\":['";
        }
        public static string ListLine(int listId, string listName, bool isFirst)
        {
            return (isFirst ? "+'" : "+',") + "{\"ListId\":" + listId.ToString() + ",\"ListName\":\"" + listName + "\",\"ListData\":['";
        }
        public static string DataLine(string id, string title, string src, string name, bool isFirst)
        {
            return (isFirst ? "+'" : "+',") + "{\"id\":\"" + id + "\", \"title\":\"" + title + "\", \"src\":\"" + src + "\", \"name\":\"" + name + "\"}'";
        }
        public static string EndList()
        {
            return "+']}'";
        }
        public static string EndStruct()
        {
            return "+']}');";
        }
        public static string JsonStringOld(DirectoryInfoType dir, string sWorkingDir, Dictionary<string, string> extFilter)
        {
            int fileId = 90001;
            int listId = 0;
            bool isFirstInList = true;
            int fileInList = 0;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(JsonJavaScriptRecord0.BeginStruct("data1"));
            sb.AppendLine(JsonJavaScriptRecord0.ListLine(listId++, dir.Name, true));
            {
                foreach (FileInfoType fi in dir.ChildFiles)
                {
                    string ext_norm = fi.Extension.ToLower();
                    if (!FileManager.isExtensionExcluded(ext_norm, extFilter)
                            && !fi.Name.Contains("'"))
                    {
                        string src = FileManager.removeWorkingDirectory(fi.FullName.Replace("\\", "/"), sWorkingDir);
                        sb.AppendLine(JsonJavaScriptRecord0.DataLine((fileId++).ToString(), "", src, fi.Name, isFirstInList));
                        isFirstInList = false;
                        fileInList++;
                    }
                }
                foreach (DirectoryInfoType dirItem in dir.ChildDirectories)
                {
                    if (fileInList + dirItem.ChildFiles.Count > 300 || fileInList > 100)
                    {
                        sb.AppendLine(JsonJavaScriptRecord0.EndList());
                        sb.AppendLine(JsonJavaScriptRecord0.ListLine(listId++, dirItem.Name, false));
                        isFirstInList = true;
                        fileInList = 0;
                    }
                    foreach (FileInfoType fi in dirItem.ChildFiles)
                    {
                        string ext_norm = fi.Extension.ToLower();
                        if (!FileManager.isExtensionExcluded(ext_norm, extFilter)
                                && !fi.Name.Contains("'"))
                        {
                            string src = FileManager.removeWorkingDirectory(fi.FullName.Replace("\\", "/"), sWorkingDir);
                            sb.AppendLine(JsonJavaScriptRecord0.DataLine((fileId++).ToString(), "", src, fi.Name, isFirstInList));
                            isFirstInList = false;
                            fileInList++;
                        }
                    }
                }
            }
            sb.AppendLine(JsonJavaScriptRecord0.EndList());
            sb.AppendLine(JsonJavaScriptRecord0.EndStruct());
            return sb.ToString();
        }
    }
}
