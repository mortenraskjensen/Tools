using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tail
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                Run r = new Run(args[0], args[1]);
            }
            else
                Console.WriteLine("filename bytes");
        }
    }
}
