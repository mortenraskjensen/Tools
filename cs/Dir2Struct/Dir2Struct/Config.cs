using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Configuration;
using static System.Net.WebRequestMethods;
using System.Xml.Linq;


namespace Dir2Struct
{
    public class ConfigValueType
    {
        public int Type { get; set; }
        public string outputFile { get; set; }
        public string externalPath { get; set; }
        public List<string> externalPathList { get; set; }
    }
    public class Config
    {
        public const int ExternalPathIdx = 1;
        public const int OutputFileIdx = 2;
        public const int OptionIdx = 4;
        public Dictionary<string,string> Conf = new Dictionary<string, string>();
        public Dictionary<string, List<string>> Conf2 = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string[]>> Conf3 = new Dictionary<string, List<string[]>>();
        public Dictionary<string, int> Conf4 = new Dictionary<string, int>();
        //private List<DirStruct> outputStructs = new List<DirStruct>();
        public Config()
        {

            // Get values from the config given their key and their target type.
            //Settings settings = config.GetRequiredSection("Settings").Get<Settings>();

            // Write the values to the console.
            //Console.WriteLine($"KeyOne = {settings.KeyOne}");
            //Console.WriteLine($"KeyTwo = {settings.KeyTwo}");
            //Console.WriteLine($"KeyThree:Message = {settings.KeyThree.Message}");

            //var conf = new System.Configuration.ConfigurationSettings();
            //string configValue = System.Configuration.ConfigurationSettings.AppSettings["ConfigValueName"];
            //var arr = System.Configuration.ConfigurationSettings.AppSettings.AllKeys;
            //configValue = ConfigurationManager.AppSettings["ConfigValueName"].ToString();

            foreach (var ckey in System.Configuration.ConfigurationSettings.AppSettings.AllKeys)
            {
                string internalPath;
                string externalPath;
                string outputFile;
                int type;
                string value = ConfigurationSettings.AppSettings[ckey];
                if (!Conf.ContainsKey(ckey))
                    Conf.Add(ckey, value);

                if (ckey.Substring(0, 5).Equals("Path_"))
                {
                    string[] arr = GetArray(value);
                    if (arr.Count() > OutputFileIdx)
                    //if (SplitValue(value, out internalPath, out externalPath, out outputFile))
                    {
                        externalPath = arr[ExternalPathIdx];
                        outputFile = arr[OutputFileIdx];
                        if (arr.Count() > OptionIdx && int.TryParse(arr[OptionIdx], out type))
                            if (!Conf4.ContainsKey(outputFile))
                                Conf4.Add(outputFile, type);
                        //outputStructs.Add(new DirStruct(internalPath, externalPath));
                        if (!Conf2.ContainsKey(outputFile))
                        {
                            Conf2.Add(outputFile, new List<string>());
                            Conf3.Add(outputFile, new List<string[]>());
                        }
                        Conf2[outputFile].Add(externalPath);
                        Conf3[outputFile].Add(arr);
                        //Console.WriteLine("Key: {0} Value: {1}  Ok!", ckey, value);
                    }
                    else
                        Console.WriteLine("Key: {0} Value: {1}  Failed!", ckey, value);
                }
            }



            //configValue = System.Configuration.AppSettings["ConfigValueName"];
            //string a = ConfigurationManager.AppSettings["ConfigValueName"].ToString();
            //Console.WriteLine("Name: {0}, Code: {1}", configValue, configValue);
            //string b = ConfigurationManager.AppSettings["ConfigValueName"].ToString();
            //string data = ConfigurationManager.AppSettings["Name"].Split(new[] { "," });
            /*
            var config = ConfigurationManager.GetSection("registerCompanies")
                 as MyConfigSection;


            foreach (MyConfigInstanceElement e in config.Instances) { 
                Console.WriteLine("Name: {0}, Code: {1}", e.Name, e.Code); 
            }*/
        }
        /*
        private OutputStruct GetOutputs(string name, string internalPath, string externalPath, string filter = "")
        {
            //NameValueSectionHandler nv = new NameValueSectionHandler();
            List<OutputStruct> ret = new List<OutputStruct>();
            foreach (var ckey in System.Configuration.ConfigurationSettings.AppSettings)
            {

                ret.Add(GetOutputs(ckey));
            }
            return new OutputStruct(internalPath, externalPath, filter);
        }*/
        public bool IfSet(string key)
        {
            return (key != null && Conf.ContainsKey(key) ? Conf[key].Equals("Yes") : false);
        }
        public string[] GetKeyArray(string key)
        {
            if (key != null && Conf.ContainsKey(key))
            {
                string outValue = Conf[key];
                if (outValue != null)
                    return outValue.Split(';');
            }
            return new string[0];
        }
        public string[] GetArray(string value)
        {
           if (value != null)
               return value.Split(';');
            return new string[0];
        }
        public bool GetKey(string key, out string Value)
        {
            if (key != null && Conf.ContainsKey(key))
            {
                Value = Conf[key];
                return true;
            }
            else
            {
                Value = null;
                return false;
            }
        }
        private bool SplitValue(string value, out string internalPath, out string externalPath, out string outputFile)
        {
            {
                string[] arr = GetArray(value);
                if (arr.Count() > 2)
                {
                    internalPath = arr[0];
                    externalPath = arr[1];
                    outputFile = arr[2];
                    return true;
                }
            }

            {
                internalPath = null;
                externalPath = null;
                outputFile = null;
                return false;
            }
        }
    }
}