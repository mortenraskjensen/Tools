using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sort
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                Run r = new Run(args);
            }
            else
            {
                Console.WriteLine("Sort line in filename");
                Console.WriteLine("Sort filename\n");
                Console.WriteLine("Sort distinct line in filename");
                Console.WriteLine("Sort filename -d\n");
            }

        }
    }
}
