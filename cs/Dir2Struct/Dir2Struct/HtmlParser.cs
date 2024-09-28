using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Common;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections;

namespace Tools.Dir
{
    class HtmlParser
    {
        public static Dictionary<string, string> GetInnerTag(string pTagString)
        {
            Dictionary<string, string> tag = new Dictionary<string, string>();
            int idx = 0; int count = 10000;
            int innerTagEnd = pTagString.IndexOf(">", 0);
            string innerTag = (innerTagEnd > 0 ? pTagString.Substring(0, innerTagEnd) : pTagString).ToLower();
            int idxSpace = GetNextSpace(innerTag, 0);
            tag.Add("TagName", innerTag.Substring(0, idxSpace));

            while (idxSpace >= 0 && idxSpace <= innerTag.Length && count-- > 0)
            {
                int idxEqual = innerTag.IndexOf("=", AdjustOffsetIfWrong(innerTag, idxSpace + 1));
                if (idxEqual != -1)
                {
                    string attripName = pTagString.Substring(idxSpace, idxEqual - idxSpace).Trim(' ').ToLower();
                    int valueStartIdx, valueEndIdx;
                    string attripValue = GetValueStart(innerTag, idxEqual + 1, out valueStartIdx, out valueEndIdx);

                    if (attripValue != null)
                        if (!tag.ContainsKey(attripName))
                            tag.Add(attripName, attripValue);
                        else
                            tag[attripName] += "," + attripValue;
                    idxSpace = innerTag.IndexOf(" ", AdjustOffsetIfWrong(innerTag, valueEndIdx + 1));
                    idxSpace = SkipSpaces(innerTag, idxSpace);
                }
                else
                    idxSpace = -1;
            }
            return tag;
        }
        private static string GetValueStart(string str, int afterEqual, out int valueStartIdx, out int valueEndIdx)
        {
            string attribValue = null;
            valueStartIdx = afterEqual;
            int len = str.Length;
            //Remove spaces
            valueStartIdx = SkipSpaces(str, valueStartIdx);
            if (valueStartIdx < len && str.ElementAt(valueStartIdx) == '"')
            {
                valueStartIdx++;
                valueEndIdx = str.IndexOf("\"", AdjustOffsetIfWrong(str, valueStartIdx)) - 1;
                if (valueEndIdx > 0 && valueEndIdx < len)
                {
                    attribValue = str.Substring(valueStartIdx, valueEndIdx - valueStartIdx + 1);
                }
            }
            else
            {
                valueEndIdx = str.IndexOf(" ", AdjustOffsetIfWrong(str, valueStartIdx)) - 1;
                if (valueEndIdx < 0)
                    valueEndIdx = len - 1;
                if (valueEndIdx >= 0 && valueEndIdx < len)
                {
                    attribValue = str.Substring(valueStartIdx, valueEndIdx - valueStartIdx + 1);
                }
            }
            return attribValue;
        }
        private static int GetNextSpace(string str, int offset)
        {
            return str.IndexOf(" ", AdjustOffsetIfWrong(str, offset));
        }
        private static int SkipSpaces(string str, int idx)
        {
            int len = str.Length;
            while (idx >= 0 && idx < len && str.ElementAt(idx) == ' ')
                idx++;
            return idx;
        }
        private static int GetNextEqual(string str, int offset)
        {
            return str.IndexOf("=", AdjustOffsetIfWrong(str, offset));
        }
        private static int AdjustOffsetIfWrong(string str, int offset)
        {
            return (offset >= str.Length ? str.Length - 1 : (offset < 0 ? 0 : offset));
        }

    }
    public class Log
    {
        StringBuilder sb = new StringBuilder();
        public string ReturnLog
        {
            get
            {
                return sb.ToString();
            }
        }
        public void WriteLine(string format, params object[] args)
        {
            System.Console.WriteLine(format, args);
            sb.AppendLine(String.Format(format, args));
        }
    }
    public class matchLib {
        const int Other = -1;
        const int Nomatch = -1;
        const int Dublica = -2;//MoreThanTwoDublicates
        const int NotUnde = -3;//StartCharNotUnderScore
        const int NoWebm = -4;
        const int NoGif = -5;
        public static List<string> matchFileList(List<string> fileList)
        {
            var dict0 = matchFileDict(fileList);
            List<string> list0 = new List<string>();
            foreach (var p in dict0.OrderBy(g => g.Key))
            {
                string key;
                foreach (var p2 in p.Value)
                {
                    switch (p2.Key)
                    {
                        case Nomatch: key = "Nomatch"; break;
                        case Dublica: key = "Dublica"; break;
                        case NotUnde: key = "NotUnde"; break;
                        case NoWebm:  key = "NoWebm "; break;
                        case NoGif:   key = "NoGif  "; break;
                        default: key = p.Key.ToString(); break;
                    }
                    foreach (string s in p2.Value)
                        list0.Add(string.Format("{0} {1}", key, s));
                }
            }
            return list0;
        }
        public static Dictionary<int, Dictionary<int, List<string>>> matchFileDict(List<string> input)
        {
            int tal;
            char tegn;
            string end = "";
            ext endCode;
            Dictionary<int, Dictionary<int, List<string>>> dict = new Dictionary<int, Dictionary<int, List<string>>>();
            dict.Add(-1, new Dictionary<int, List<string>>() { 
                { Nomatch, new List<string>() }, 
                { Dublica, new List<string>() }, 
                { NotUnde, new List<string>() },
                { NoWebm, new List<string>() },
                { NoGif, new List<string>() }
            });
            foreach (string s in input)
                if (matchFileName(s, out tal, out tegn, out end, out endCode))
                {
                    if (!dict.ContainsKey(tal))
                        dict.Add(tal, new Dictionary<int, List<string>>());

                    if (dict[tal].ContainsKey((int)endCode))
                    {
                        dict[tal][(int)endCode].Add(s);
                    }
                    else
                        dict[tal].Add((int)endCode, new List<string>() { s });

                    if (tegn != ')' && tegn != '_')
                        dict[Other][NotUnde].Add(s);

                }
                else
                {
                    dict[Other][Nomatch].Add(s);
                }
            List<string> list = new List<string>();
            foreach (var p in dict.OrderBy(g => g.Key))
                if (p.Key > 0)
                {
                    bool webm = false;
                    bool gif = false;
                    bool dublicates = false;
                    foreach (var p2 in p.Value)
                    {
                        switch (p2.Key)
                        {
                            case (int)ext.webm:
                                webm = true; break;
                            case (int)ext.gif:
                                gif = true; break;
                        }
                        if (p2.Value.Count > 1)
                            dublicates = true;
                    }
                    foreach (var p2 in p.Value)
                       foreach (string s in p2.Value)
                       {
                            if (dublicates)
                                dict[Other][Dublica].Add(s);
                            if (!webm && gif)
                                dict[Other][NoWebm].Add(s);
                            if (!gif && webm)
                                dict[Other][NoGif].Add(s);
                       }
                }

            return dict;
        }

        public static void test()
        {
            int tal;
            string fileEnd;
            char c;
            ext endCode;
            matchLib.matchFileName("fassvcxde7563546", out tal, out c, out fileEnd, out endCode);
        }

        private static bool matchFileName(string input, out int tal, out char start, out string end, out ext endCode)
        {
            tal = 0;
            start = '0';
            end = "b.gif";
            endCode = ext.smallgif;
            if (input.EndsWith(end))
                return SearchWithPattern(input.Substring(0, input.Length - end.Length), @"^\d+$", @"[^\d]\d+$", out tal, out start);
            else
            {
                endCode = ext.webm;
                end = "a.webm";
                if (input.EndsWith(end))
                    return SearchWithPattern(input.Substring(0, input.Length - end.Length), @"^\d+$", @"[^\d]\d+$", out tal, out start);
                else
                {
                    endCode = ext.gif;
                    end = ".gif";
                    if (input.EndsWith(end))
                        return SearchWithPattern(input.Substring(0, input.Length - end.Length), @"^\d+$", @"[^\d]\d+$", out tal, out start);
                    else
                        end = "";
                }
            }
            endCode = ext.unknown;
            return false;
        }
        public static bool SearchWithPattern(string input, string pattern1, string pattern2, out int tal, out char c)
        {
            Match m1 = Regex.Match(input, pattern1, RegexOptions.IgnoreCase);
            if (m1.Success)
            {
                //Console.WriteLine("Found '{0}' at position {1}.", m.Value, m.Index);
                c = '_';
                if (int.TryParse(m1.Value.Substring(0), out tal))
                    return true;
            }
            else
            {
                Match m2 = Regex.Match(input, pattern2, RegexOptions.IgnoreCase);
                if (m2.Success)
                {
                    //Console.WriteLine("Found '{0}' at position {1}.", m.Value, m.Index);
                    c = m2.Value.ElementAt(0);
                    if (int.TryParse(m2.Value.Substring(1), out tal))
                        return true;
                }
            }

            tal = -1;
            c = 'a';
            return false;
        }
    }
    public enum ext : int
    {
        gif = 2,
        webm = 1,
        smallgif = 3,
        thumb = 4,
        unknown = 0
    }
}
