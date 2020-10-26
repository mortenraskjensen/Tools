using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace Nedarvning
{
    class Program
    {
        static void Main(string[] args)
        {
            string a = UpdateByRegex("\"ClientId\":\"1234\",\"FkId\":\"1234\"");
            Run r = new Run();
        }


        private static string UpdateByRegex(string input)
        {
            string ret = "";
            //string pattern = Regex.Escape("\"");
            //return pattern;
            string pattern = "\"(\\w+)\":\"([^\"]+)\"";
            string tmp;
            int l;
            Regex r = new Regex(@"" + pattern);
            Match match = r.Match(input);
            if (match.Success)
            {
                int i = match.Index;
                ret += match.Groups[1].Value + "=" + match.Groups[2].Value;
                tmp = match.Groups[0].Value;
                l = tmp.Length;
                Match match2 = r.Match(input,i+l);
                if (match2.Success)
                {
                    int j = match.Index;
                    tmp = match2.Groups[0].Value;
                    l = tmp.Length;
                    ret += match2.Groups[1].Value + "=" + match2.Groups[2].Value;
                }
                return ret;
            }
            else
            {
                return null;
            }
        }
        private void ValidMobileNumber()
        {
            Regex r = new Regex(@"^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}");
            //class Regex Repesents an immutable regular expression.  

            string[] str = { "+91-9678967101", "9678967101", "+91-9678-967101", "+91-96789-67101", "+919678967101" };
            //Input strings for Match valid mobile number.  
            foreach (string s in str)
            {
                Console.WriteLine("{0} {1} a valid mobile number.", s,
                r.IsMatch(s) ? "is" : "is not");
                //The IsMatch method is used to validate a string or  
                //to ensure that a string conforms to a particular pattern.  
            }

        }
    }
    class Run
    {
        public Run()
        {
            TestGenericList2 c = new TestGenericList2();
            TestGenericList1 d = new TestGenericList1();
            BilExempler a = new BilExempler();
            Pro b = new Pro();
            AbstrakteKlasser k = new AbstrakteKlasser();
        }
    }
}
