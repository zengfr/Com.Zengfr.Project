using System;
using System.Reflection;
using System.Text;
namespace Com.Zengfr.CodeGenerator.Common
{
    public class ConvertUtils
    {
        public static void CopyProperties(object srcInstance, object destInstance)
        {
            PropertyInfo[] srcPropInfos = srcInstance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] destPropInfos = destInstance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pi in srcPropInfos)
            {
                if (pi.CanRead)
                {
                    PropertyInfo destPI = Array.Find<PropertyInfo>(destPropInfos, delegate(PropertyInfo dpi)
                    {
                        return (0 == string.Compare(dpi.Name, pi.Name, true));
                    });
                    if (destPI != null && destPI.CanWrite)
                    {
                        destPI.SetValue(destInstance, pi.GetValue(srcInstance, null), null);
                    }
                }
            }
        }
        /// <summary>
        /// 前字符大写
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public static string ToPascal(string phrase)
        {
            return ConvertCaseString(phrase, "Pascal");
        }
        /// <summary>
        /// 前字符小写
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public static string ToCamel(string phrase)
        {
            return ConvertCaseString(phrase, "Camel");
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
