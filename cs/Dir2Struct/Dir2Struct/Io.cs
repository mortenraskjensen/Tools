using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Common;

namespace Tools.Dir
{
    class Io
    {
        public static List<string> Read2List(string fn)
        {
            List<string> l = new List<string>();
            using (var reader = new StreamReader(fn))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    l.Add(line);
                }
            }
            return l;
        }
        public static string Read2String(string fn)
        {
            string l = "";
            using (var reader = new StreamReader(fn))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    l += line;
                }
            }
            return l;
        }
    }
    public class csv
	{
		public static string[] parseLine(string line, char seperator)
		{
            return line.Split(seperator);
		}
        public static List<string[]> getCsvCulumns(List<string> lines, char seperator)
        {
            List<string[]> ret = new List<string[]>();
            foreach (string line in lines)
                ret.Add(parseLine(line, seperator));
            return ret;
        }
        public static Dictionary<string, List<string[]>> groupByColumn(List<string[]> csvFileData, int columnNr)
        {
            Dictionary<string, List<string[]>> ret = new Dictionary<string, List<string[]>>();
            foreach (string[] cols in csvFileData)
            {
                if (cols.Length >= columnNr)
                {
                    string key = cols[columnNr - 1];
                    if (ret.ContainsKey(key))
                        ret[key].Add(cols);
                    else
                        ret.Add(key, new List<string[]>() { cols });
                }
            }
            return ret;
        }
	}
}
