using System;
using System.Collections.Generic;
using System.Text;
using Com.Zfrong.Common.Command;
namespace Com.Zfrong.Common.Command
{
    class MessageCommand:CommandBase
    {
        public override CommandFamily Family
        {
            get { return CommandFamily.Command_App; }
        }

        public override void CommandBody(object sender, params object[] paras)
        {
            //Core.CoreData[CoreDataType.App_Message] = DateTime.Now.ToString("HH:mm:ss fff") + ":" + paras[0];//
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fff") + ":" +Unescape(paras[0].ToString()));
        }
       
//        public static string ascii(string str){
//return str.replace(/[^\u0000-\u00FF]/g,function($0){return escape($0).replace(/(%u)(\w{4})/gi,"\\u$2")});
//  }
//public static string unascii(string str){
//  return unescape(str.replace(/\\u/g,"%u"));
//  }

        public static string Escape(string str)
    {
        if (str == null)
            return String.Empty;

        StringBuilder sb = new StringBuilder();
        int len = str.Length;

        for (int i = 0; i <len; i++)
        {
            char c = str[i];

            //everything other than the optionally escaped chars _must_ be escaped
            if (Char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == '/' || c == '\"' || c == '.')
                sb.Append(c);
            else
                sb.Append(Uri.HexEscape(c));
        }

        return sb.ToString();
    }

        public static string Unescape(string str)
    {
        return System.Text.RegularExpressions.Regex.Unescape(str);
    }
        public static string UnEscape(string str)
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
