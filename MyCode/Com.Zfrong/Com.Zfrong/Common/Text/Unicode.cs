using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Zfrong.Common.Text
{
    public class MyEncoding
    {
        /// <summary>
        /// 中文转为UNICODE 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUNICODE(string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    //將中文轉為10進制整數，然後轉為16進制unicode
                    if (IsChineseLetter(str[i]))
                        outStr += "%u" + ((int)str[i]).ToString("x");
                    else
                        outStr += str[i];
                }
            }
            return outStr;//
        }
        //UNICODE转为中文(最直接的方法Regex.Unescape(input);)
        public static string ToChinese(string[] str)
        {
            string outStr = "";
           foreach(string s in str)
           {
               outStr+=ToChinese(s);
           }
               return outStr;
        }
        public static string ToChinese(string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\", "").Replace("%", "").Split('u');

                for (int i = 1; i < strlist.Length; i++)
                {
                    try
                    {
                        //将unicode转为10进制整数，然后转为char中文
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                    catch (FormatException ex)
                    {
                        outStr += strlist[i];
                    }
                }
            }
            return outStr;
        }
        protected static bool IsChineseLetter(char input)
        {
            int code = 0;
            int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
            int chend = Convert.ToInt32("9fff", 16);

            code = (int)(input);    //获得字符串input中指定索引index处字符unicode编码

            if (code >= chfrom && code <= chend)
            {
                return true;     //当code在中文范围内返回true

            }
            else
            {
                return false;    //当code不在中文范围内返回false
            }
            return false;
        }
    }
}
