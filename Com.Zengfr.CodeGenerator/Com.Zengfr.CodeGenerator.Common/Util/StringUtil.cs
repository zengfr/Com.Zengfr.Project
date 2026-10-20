using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.CodeGenerator.Common.Util
{
    public class StringUtil
    {
        private static StringUtil instance;
        public static StringUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StringUtil();
                }
                return instance;
            }
        }
        /// <summary>
        /// 前字符大写
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public virtual string ToPascal(string phrase)
        {
            return ConvertCaseString(phrase, "Pascal");
        }
        /// <summary>
        /// 前字符小写
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public virtual string ToCamel(string phrase)
        {
            return ConvertCaseString(phrase, "Camel");
        }
        public virtual string ToLower(string phrase)
        {
            return phrase.ToLower();
        }
        public virtual string ToUpper(string phrase)
        {
            return phrase.ToUpper();
        }
        public virtual string HtmlEncode(string str)
        {
            return System.Net.WebUtility.HtmlEncode(str);
        }
        public virtual string HtmlDecode(string str)
        {
            return System.Net.WebUtility.HtmlDecode(str);
        }
        public virtual string UrlEncode(string str)
        {
            return System.Net.WebUtility.UrlEncode(str);
        }
        public virtual string UrlDecode(string str)
        {
            return System.Net.WebUtility.UrlDecode(str);
        }
        public virtual StringBuilder CreateStringBuilder()
        {
            return new StringBuilder();
        }
        public virtual StringWriter CreateStringWriter()
        {
            return new StringWriter();
        }
        private static string ConvertCaseString(string phrase, string type)
        {
            string[] splittedPhrase = phrase.Split(' ', '-', '.', '_');
            var sb = new StringBuilder();
            char[] splittedPhraseChars;
            for (int index = 0; index < splittedPhrase.Length; index++)
            {
                splittedPhraseChars = splittedPhrase[index].ToCharArray();

                if (splittedPhraseChars.Length > 0)
                {
                    if (index == 0 && type == "Camel")
                    {
                        splittedPhraseChars[0] = ((new String(splittedPhraseChars[0], 1)).ToLower())[0];
                    }
                    else if (type == "Pascal" || type == "Camel")
                        splittedPhraseChars[0] = ((new String(splittedPhraseChars[0], 1)).ToUpper())[0];
                }
                sb.Append(new String(splittedPhraseChars)); splittedPhraseChars = null;
            }
            splittedPhrase = null;
            return sb.ToString();
        }
    }
}
