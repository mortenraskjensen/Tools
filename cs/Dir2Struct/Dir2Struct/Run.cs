using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Collections;
using System.IO;

namespace Dir2Struct
{
    public interface IRun
    {
    }
    public abstract class RunBase : IRun
    {
        public RunBase(string[] args)
        {
            if (args.Length >= 2)
            {
                GetToDoIt(args[0], "Thumbs/", args[1]);
            }
            else
            {
                DefaultRun();
            }
            //Console.WriteLine("dir ouputfile");
        }
        protected abstract void DefaultRun();

        /// <summary>
        /// I need a file created with lines created from filenames in a directory
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="n"></param>
        protected void GetToDoIt(string path, string prePath, string outputFilenameWithFullPath)
        {
            path = DirStruct.AddLastBackSlash(path);
            List<string> l = DirStruct.AddHeaderAndFooter(DirStruct.GetFileNames(path, prePath + DirStruct.GetOutputPath(path)));
            string outFile = DirStruct.WriteLines(l, outputFilenameWithFullPath);
            System.Console.WriteLine("Output writen to {0}", outFile);
        }


        protected void GetToDoIt2(string path, string prePath, string outputFilenameWithFullPath)
        {
            GetToDoIt2(new List<string> { path }, prePath, outputFilenameWithFullPath);
        }

        protected void GetToDoIt2(List<string> pathList, string prePath, string outputFilenameWithFullPath, int minDynamic = 10)
        {
            List<DirStruct> ouList = new List<DirStruct>();
            foreach (string p in pathList)
            {
                string path = DirStruct.AddLastBackSlash(p);
                DirStruct ou = new DirStruct(path, prePath + DirStruct.GetOutputPath(path));
                DirStruct.AddFileNames(ou, new List<string>(), minDynamic);
                ouList.Add(ou);
            }
            List<string> l = DirStruct.AddHeaderAndFooter(DirStruct.GetFileNames(ouList));
            string outFile = DirStruct.WriteLines(l, outputFilenameWithFullPath);
            System.Console.WriteLine("Output writen to {0}", outFile);
        }
        protected void DoThumbs(string pathInput, string pathOutput, string pathDelete)
        {
            var tn = new ThumpNailImage();

            long StartTime = DateTime.Now.Ticks;
            List<string[]> fl = DirStruct.GetFileListPath(pathInput, pathOutput);
            long MidTime = DateTime.Now.Ticks;
            double res = (MidTime - StartTime) / 10000000.0;
            System.Console.WriteLine("Time:{0}", (MidTime - StartTime) / 10000000.0);

            System.Console.WriteLine("Antal filer {0}", fl.Count());

            int counter = 0;
            int total = fl.Count();
            if (fl.Count() > 0)
                foreach (string[] f in fl)
                    if (counter++ <= 5001)
                        if (f.Count() == 3)
                        {
                            if (counter % 500 == 0)
                                System.Console.WriteLine("{1}/{0} {2}", total, counter, f[0]);

                            if (tn.IsValidImage(f[0]))
                                tn.WriteThumpNail(f[0], f[1], f[2]);
                        }
                        else
                            if (f.Count() == 4)
                            if (f[3].Equals("delete"))
                            {
                                if (tn.MoveFile(f[0], pathDelete + f[1]))
                                    System.Console.WriteLine("Moved {0}", f[0]);
                            }
            long EndTime = DateTime.Now.Ticks;
            System.Console.WriteLine("Time1:{0} Time2:{1}", (MidTime - StartTime) / 10000000.0, (EndTime - MidTime) / 10000000.0);
        }
    }
    class Run : RunBase
    {
        public Run(string[] args)
            : base(args)
        {
        }
        protected override void DefaultRun()
        {
            //GetToDoIt2("C:\\Temp\\Ny mappe\\Rod\\", "Thumbs/", "C:\\Temp\\Rod.js");
            DoThumbs("C:\\Temp\\Ny mappe\\Rod", "C:\\Temp\\Thumbs\\Rod", "C:\\Temp\\ThumbsDelete\\");
        }
    }
}
