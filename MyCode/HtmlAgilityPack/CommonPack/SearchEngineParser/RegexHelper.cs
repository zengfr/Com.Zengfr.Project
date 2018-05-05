using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ParserEngine
{
    class RegexHelper
    {
        static RegexOptions defaultRegexOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline;
        public static List<string> GetList(string input, string action, int groupIndex)
        {
            return DoRegex(input, action, groupIndex, 99999);
        }
        public static string GetOne(string input, string action, int groupIndex, int listIndex)
        {
            List<string> objs = DoRegex(input, action, groupIndex, listIndex);
            if (objs.Count != 0)
                return objs[0];
            return string.Empty;
        }
        private static List<string> DoRegex(string input, string action,int groupIndex,int listIndex)
        {
            List<string> objs = new List<string>();
            Regex regex = new Regex(action, defaultRegexOptions);
            MatchCollection matchColl = regex.Matches(input);
            regex = null; int index =listIndex;
            if (matchColl.Count > listIndex)
            {
                if (listIndex < 0)
                { 
                    index = matchColl.Count + listIndex; 
                }
                if (index >= 0)
                {
                    objs.Add(matchColl[index].Groups[groupIndex].Value);
                    matchColl = null;
                    return objs;
                } 
            }
            foreach (Match m in matchColl)
                  objs.Add(m.Groups[groupIndex].Value);
            matchColl = null;
            return objs;
        }
        /// <summary>
        /// 去除 HTML tag  google "StripHTML" 得到
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string StripHTML(string HTML) //google "StripHTML" 得到
{ string[] Regexs =
                                {
                                    @"<script[^>]*?>.*?</script>",
                                    @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                                    @"([\r\n])[\s]+",
                                    @"&(quot|#34);",
                                    @"&(amp|#38);",
                                    @"&(lt|#60);",
                                    @"&(gt|#62);",
                                    @"&(nbsp|#160);",
                                    @"&(iexcl|#161);",
                                    @"&(cent|#162);",
                                    @"&(pound|#163);",
                                    @"&(copy|#169);",
                                    @"&#(\d+);",
                                    @"-->",
                                    @"<!--.*\n"
                                };

            string[] Replaces =
                                {
                                    "",
                                    "",
                                    "",
                                    "\"",
                                    "&",
                                    "<",
                                    ">",
                                    " ",
                                    "\xa1", //chr(161),
                                    "\xa2", //chr(162),
                                    "\xa3", //chr(163),
                                    "\xa9", //chr(169),
                                    "",
                                    "\r\n",
                                    ""
                                };

            string s = HTML;
            for (int i = 0; i < Regexs.Length; i++)
            {
                s = new Regex(Regexs[i], RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(s, Replaces[i]);
            }
            s.Replace("<", "");
            s.Replace(">", "");
            s.Replace("\r\n", ""); Regexs = null; Replaces = null;
            return s;
        }
    }
}
