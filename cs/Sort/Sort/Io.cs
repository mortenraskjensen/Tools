using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Common;

namespace Sort
{
    class Io
    {
        private static List<string> Read2List(string fn)
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
        private void WriteLines(List<string> l)
        {
            foreach (string line2 in l)
            {
                Console.WriteLine(line2);
            }
        }
        private void Sort(string fn)
        {
            WriteLines(Read2List(fn).OrderBy(h => h).ToList());
        }
        private void ToJson(string fn)
        {
            WriteLines(ToJson(Read2List(fn)));
        }
        private void Dict(string fn)
        {
            WriteLines(Read2Dictionary(fn).Select(h => h.Key).OrderBy(h => h).ToList());
        }
        private List<string> ToJson(List<string> l)
        {
            List<string> lo = new List<string>() { "[" };
            int linenr = 0;
            string[] colNames = null;
            string[] colValues = null;
            int colAntal = 0;
            foreach (string line in l)
            {
                linenr++;
                if (linenr == 1)
                {
                    colNames = line.Split(';');//.Split('\u002C');
                    colAntal = colNames.Length;
                }
                else
                {
                    colValues = line.Split(';');
                    string jline = (linenr == 2 ? "" : ",");
                    int colnr = 0;
                    foreach (string col in colValues)
                    {
                        if (colnr <= colAntal)
                            jline += (colnr == 0 ? "{" : ",") + "\"" + colNames[colnr] + "\":\"" + col + "\"";
                        colnr++;
                    }
                    lo.Add(jline + "}");
                }

            }
            lo.Add("]");
            return lo;
        }
        private Dictionary<string, int> Read2Dictionary(string fn)
        {
            Dictionary<string, int> d = new Dictionary<string, int>();
            using (var reader = new StreamReader(fn))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!d.ContainsKey(line))
                        d.Add(line, 1);
                    else
                        d[line]++;
                }
            }
            return d;
        }
    }
}