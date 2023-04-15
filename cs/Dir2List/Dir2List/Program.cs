using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Reflection.Emit;
using System.Data;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Text.Json;

namespace Dir2List
{
    class Program
    {
        static void Main(string[] args)
        {
            Run r = new Run(args);
        }
    }
    class Run
    {
        public Run(string[] args)
        {
            Console.WriteLine("Hello World!");
            IWork arbejde = new Work();

            arbejde.test3(arbejde.SubDir2, arbejde.RootPath);
            arbejde.test4(arbejde.TestDir, arbejde.RootPath);
            arbejde.test6(arbejde.AndetDir, arbejde.RootPath);
        }
    }
}
