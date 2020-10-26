using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tail
{
    class Run
    {
        public Run(string filename, string leng)
        {
            int n;
            if (int.TryParse(leng, out n))
                Tail(filename, n);
        }

        private void Tail(string fn, int n)
        {
            using (var reader = new StreamReader(fn))
            {
                if (reader.BaseStream.Length > n)
                {
                    reader.BaseStream.Seek(-n, SeekOrigin.End);
                }
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}
