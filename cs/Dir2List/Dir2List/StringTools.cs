using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Dir2List
{
    public class StringTools
    {
        public const string yyyyMMdd = "yyyyMMdd";
        public const string HHmmss = "HHmmss";
        public static string lbr = string.Concat((char)13, (char)10);
        public const string lb = "\r\n";
        public static string pling = "'";


        public static string DateyyyyMMddHHmmss(DateTime d)
        {
            return d.ToString(yyyyMMdd) + d.ToString(HHmmss);
        }

        public static string NumberFixedLengthWithLeadingZeros(int value, int length)
        {
            double numberOfDigits = Math.Log10(Math.Abs(value)) + 1;//10^n has n+1 digits

            if ((value >= 0 && length < numberOfDigits) 
                || (value < 0 && length-1 < numberOfDigits) //There need to be room for the sign
                )
                return "".PadLeft(length, '0');

            return value.ToString("D" + length.ToString());
        }

        public static string FixedColumns(string[] args, int length, bool isAlignedLeft = true)
        {
            // args[i].PadRight(10, ' ') == string.Format("{0,-10}", args[i])
            // args[i].PadLeft(10, ' ') == string.Format("{0,10}", args[i])
            string ret = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (isAlignedLeft)
                {
                    if (args[i].Length <= length)
                        ret += args[i].PadRight(length, ' ');
                    else
                        ret += args[i].Substring(0, length);
                }
                else
                {
                    if (args[i].Length <= length)
                        ret += args[i].PadLeft(length, ' ');
                    else
                        ret += args[i].Substring(args[i].Length - length);
                }
            }

            return ret;
        }
    }
}