using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Common;

namespace Dir2List
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
}