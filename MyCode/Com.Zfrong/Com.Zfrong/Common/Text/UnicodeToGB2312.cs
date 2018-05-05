using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
namespace Com.Zfrong.Common.Text
{
    public partial class UnicodeToGB2312
{
    public static string Encode(string inputString)
    {
        MatchEvaluator matchEvaluator = new MatchEvaluator(EncodeFindSubString); //初始化一个委托,该委托用于处理Regex.Repalce中每次匹配的match对象
        string result = System.Text.RegularExpressions.Regex.Replace(inputString, @"[^\u0000-\u00FF]", matchEvaluator);
        return result;
    }
    public static string UnEncode(string inputString)
    {
        MatchEvaluator matchEvaluator = new MatchEvaluator(UnEncodeFindSubString);
        string result = System.Text.RegularExpressions.Regex.Replace(inputString, @"&#x([0-9a-fA-F]{4});", matchEvaluator);
        return result;
    }
        /// <summary>
        /// \\u问题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
    public static string ReplaceUnicode2Str(string input)
    {
        Regex regex = new Regex("(?i)\\\\u[0-9a-f]{4}");
        MatchEvaluator matchAction = delegate(Match m)
        {
            string str = m.Groups[0].Value;
            byte[] bytes = new byte[2];
            bytes[1] = byte.Parse(int.Parse(str.Substring(2, 2), NumberStyles.HexNumber).ToString());
            bytes[0] = byte.Parse(int.Parse(str.Substring(4, 2), NumberStyles.HexNumber).ToString());
            return Encoding.Unicode.GetString(bytes);
        };
        return regex.Replace(input, matchAction);
    }
    protected static string EncodeFindSubString(Match match) //编码的时候委托处理函数
    {
        byte[] bytes = Encoding.Unicode.GetBytes(match.Value);
        string result = "";
        for (int i = bytes.Length - 1; i >= 0; i--)
        {
            result += ToHexString(bytes[i]);
        }
        result = "&#x" + result + ";";
        return result;
    }
    protected static string UnEncodeFindSubString(Match match) //解码的时候委托处理函数
    {
        string result = match.Groups[1].Value;
        byte[] bytes = new byte[2];  //4E 2D
        bytes[1] = byte.Parse(result.Substring(0,2),System.Globalization.NumberStyles.AllowHexSpecifier);
        bytes[0] = byte.Parse(result.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
        result = result.Replace(result, Encoding.Unicode.GetString(bytes));
        return result;
    }
    protected static string ToHexString(byte a) //返回一个16进制表示的数
    {
        string result = a.ToString("X");
        if (result.Length == 2)
        {
            return result;
        }
        else
        {
            return "0" + result; //如果就一位比如"7"的16进制返回的是"7"而我们需要 "07"
        }
    }
}
}
