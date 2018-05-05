
namespace Com.Zengfr.Proj.Common
{
    static public class ChineseUtils
    {
        /// <summary>
        /// 转化为半角字符串
        /// </summary>
        /// <param name="input">要转化的字符串</param>
        /// <returns>半角字符串</returns>
        public static string ToSBC(this string input)//single byte charactor
        {
            if (!string.IsNullOrEmpty(input))
            {
                char[] c = input.ToCharArray();
                for (int i = 0; i < c.Length; i++)
                {
                    if (c[i] == 12288)//全角空格为12288，半角空格为32
                    {
                        c[i] = (char)32;
                        continue;
                    }
                    if (c[i] > 65280 && c[i] < 65375)//其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
                        c[i] = (char)(c[i] - 65248);
                }
                return new string(c);
            }
            return input;
        }

        /// <summary>
        /// 转化为全角字符串
        /// </summary>
        /// <param name="input">要转化的字符串</param>
        /// <returns>全角字符串</returns>
        public static string ToDBC(this string input)//double byte charactor 
        {
            if (!string.IsNullOrEmpty(input))
            {
                char[] c = input.ToCharArray();
                for (int i = 0; i < c.Length; i++)
                {
                    if (c[i] == 32)
                    {
                        c[i] = (char)12288;
                        continue;
                    }
                    if (c[i] < 127)
                        c[i] = (char)(c[i] + 65248);
                }
                return new string(c);
            }
            return input;
        }
    }

}
