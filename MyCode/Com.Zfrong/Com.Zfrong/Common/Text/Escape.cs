using System;
using System.Collections.Generic;
using System.Text;
namespace Com.Zfrong.Common.Text
{
 public   class Escape
    {
     /// <summary>
    /// Escape的加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToEscape(string str)
    {
        if (str == null)
            return String.Empty;

        StringBuilder sb = new StringBuilder();
        int len = str.Length;

        for (int i = 0; i < len; i++)
        {
            char c = str[i];

            //everything other than the optionally escaped chars _must_ be escaped 
            if (Char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == '/' || c == '\\' || c == '.')
                sb.Append(c);
            else
                sb.Append(Uri.HexEscape(c));
        }

        return sb.ToString();
    }
    /// <summary>
    /// Escape的解密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToUnEscape(string str)
    {
        if (str == null)
            return String.Empty;

        StringBuilder sb = new StringBuilder();
        int len = str.Length;
        int i = 0;
        while (i != len)
        {
            if (Uri.IsHexEncoding(str, i))
                sb.Append(Uri.HexUnescape(str, ref i));
            else
                sb.Append(str[i++]);
        }

        return sb.ToString();
    }

    }


}